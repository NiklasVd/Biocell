using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Biocell.Network
{
    public class Server<TServerPacket, TClientPacket> where TServerPacket : IServerPacket
        where TClientPacket : IClientPacket
    {
        public delegate void OnReceiveClientPacketHandler(int connectionId, TClientPacket packet);
        public event OnReceiveClientPacketHandler OnReceiveClientPacket;

        private TcpListener server;
        private Thread acceptSocketsThread;

        protected readonly Dictionary<int, ClientConnection> clientConnections;

        private bool isHosting;
        public bool IsHosting { get { return isHosting; } }

        public Server(int port)
        {
            server = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            clientConnections = new Dictionary<int, ClientConnection>();
        }
        ~Server()
        {
            Close();
        }

        public void Host()
        {
            if (!isHosting)
            {
                OnHost();
            }
        }
        public void Close()
        {
            if (isHosting)
            {
                OnClose();
            }
        }

        public void DisconnectClient(int connectionId)
        {
            var clientConnection = clientConnections[connectionId];

            clientConnection.terminate = true;
            clientConnection.receivePacketsThread.Join();

            clientConnection.socket.Close();
            clientConnections.Remove(connectionId);
        }

        public void SendPacket(TServerPacket packet, params int[] connectionIds)
        {
            for (int i = 0; i < connectionIds.Length; i++)
            {
                SendPacket(packet, clientConnections[connectionIds[i]]);
            }
        }
        public void SendPacket(TServerPacket packet)
        {
            var currentClientConnections = clientConnections.Keys.ToArray();
            SendPacket(packet, currentClientConnections);
        }
        protected void SendPacket(TServerPacket packet, params ClientConnection[] clientConnections)
        {
            for (int i = 0; i < clientConnections.Length; i++)
            {
                var packetBytes = PacketConverter.ToBytes(packet);
                var sentBytes = clientConnections[i].socket.Send(packetBytes);

                if (sentBytes == 0)
                {
                    DisconnectClient(clientConnections[i].connectionId);
                    Console.WriteLine("Kicked client: Peer not reachable");
                }
            }
        }

        protected virtual void OnHost()
        {
            server.Start();
            isHosting = true;

            acceptSocketsThread = new Thread(AcceptSocketsLoop);
            acceptSocketsThread.Start();
        }
        protected virtual void OnClose()
        {
            isHosting = false;
            acceptSocketsThread.Join();

            foreach (var connectionId in clientConnections.Keys)
                DisconnectClient(connectionId);

            server.Stop();
        }

        private void AcceptSocketsLoop()
        {
            while (isHosting)
            {
                var socket = server.AcceptSocket();
                RegisterSocket(socket);
            }
        }
        private void RegisterSocket(Socket sock)
        {
            InitializeSocket(sock);

            var receivePacketsThread = new Thread(ReceivePacketsLoop);
            var connectionId = sock.GetHashCode();

            var clientConnection = new ClientConnection(connectionId, sock, receivePacketsThread);
            clientConnections.Add(connectionId, clientConnection);

            receivePacketsThread.Start(connectionId);
        }

        private void ReceivePacketsLoop(object connectionIdObj)
        {
            var connectionId = (int)connectionIdObj;
            var clientConnection = clientConnections[connectionId];

            while (isHosting && !clientConnection.terminate)
            {
                var availableBytes = clientConnection.socket.Available;
                if (availableBytes != 0)
                {
                    var clientPacket = ReceivePacket(clientConnection);
                    if (OnReceiveClientPacket != null && clientPacket != null)
                        OnReceiveClientPacket(connectionId, clientPacket);
                }
            }
        }

        private TClientPacket ReceivePacket(ClientConnection clientConnection)
        {
            var receivedBytes = clientConnection.socket.Receive(clientConnection.packetReceivingBuffer);

            if (receivedBytes == 0)
            {
                DisconnectClient(clientConnection.connectionId);
                Console.WriteLine("Kicked client: 0 bytes received");

                return default(TClientPacket);
            }
            else
            {
                var packetBuffer = new byte[receivedBytes];
                Buffer.BlockCopy(clientConnection.packetReceivingBuffer, 0, packetBuffer, 0, receivedBytes);

                return PacketConverter.ToPacket<TClientPacket>(packetBuffer);
            }
        }

        private void InitializeSocket(Socket sock)
        {
            sock.LingerState = new LingerOption(true, 1);
            sock.NoDelay = true;

            sock.SendBufferSize = 16384;
            sock.SendTimeout = 6000;

            sock.ReceiveBufferSize = 16384;
            sock.ReceiveTimeout = 6000;
        }

        private bool CheckConnection(ClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }

        private void HeartbeatConnection(ClientConnection clientConnection) // Async?
        {
            if (CheckConnection(clientConnection))
            {
                // Heartbeat packet to client
                // If successful, return to caller
            }

            DisconnectClient(clientConnection.connectionId);
        }
        private void HeartbeatConnections()
        {
            var clientConnectionsCopy = clientConnections.Values.ToList();
            foreach (var clientConnection in clientConnectionsCopy)
                HeartbeatConnection(clientConnection);
        }
    }

    public class ClientConnection
    {
        public int connectionId;

        public Socket socket;
        public Thread receivePacketsThread;
        public byte[] packetReceivingBuffer;

        public bool terminate;

        public ClientConnection(int connectionId, Socket socket, Thread receivePacketsThread)
        {
            this.connectionId = connectionId;
            this.socket = socket;
            this.receivePacketsThread = receivePacketsThread;
            packetReceivingBuffer = new byte[socket.ReceiveBufferSize];
        }
    }
}
