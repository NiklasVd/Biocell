using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Biocell.Network
{
    public class Client<TClientPacket, TServerPacket> : IDisposable where TClientPacket : IClientPacket
        where TServerPacket : IServerPacket
    {
        public delegate void OnReceivePacketHandler(TServerPacket packet);
        public event OnReceivePacketHandler OnReceivePacket;

        private Socket socket;
        private Thread receivePacketsThread;

        private byte[] packetReceivingBuffer;
        private object packetReceivingLock;

        private bool isConnected;
        public bool IsConnected { get { return isConnected; } }

        public Client()
        {
            packetReceivingLock = new object();
        }

        public void Connect(IPEndPoint serverEndPoint)
        {
            if (!isConnected)
            {
                OnConnect(serverEndPoint);
            }
        }
        public void Disconnect()
        {
            if (isConnected)
            {
                OnDisconnect();
            }
        }

        public virtual void Dispose()
        {
            Disconnect();
        }

        protected virtual void OnConnect(IPEndPoint serverEndPoint)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            InitializeSocket();

            socket.Connect(serverEndPoint);
            isConnected = true;

            receivePacketsThread = new Thread(ReceivePacketsLoop);
            receivePacketsThread.Start();
        }
        protected virtual void OnDisconnect()
        {
            isConnected = false;
            receivePacketsThread.Join();

            socket.Close();
        }

        public void SendPacket(TClientPacket packet)
        {
            if (isConnected)
            {
                var packetBytes = PacketConverter.ToBytes(packet);
                socket.Send(packetBytes);
            }
        }

        public TServerAnswer SendRequest<TClientRequest, TServerAnswer>(TClientRequest requestPacket) where TClientRequest : TClientPacket, IPacketRequestable where TServerAnswer : TServerPacket
        {
            if (isConnected)
            {
                SendPacket(requestPacket);
                return (TServerAnswer)ReceivePacket();
            }
            else return default(TServerAnswer);
        }
        public bool TrySendRequest<TClientRequest, TServerAnswer>(TClientRequest requestPacket, out TServerAnswer answerPacket) where TClientRequest : TClientPacket, IPacketRequestable
            where TServerAnswer : TServerPacket
        {
            answerPacket = SendRequest<TClientRequest, TServerAnswer>(requestPacket);
            return !answerPacket.Equals(default(TServerAnswer));
        }
        public TServerAnswer SendRequestAsync<TClientRequest, TServerAnswer>(TClientRequest requestPacket) where TClientRequest : TClientPacket, IPacketRequestable
            where TServerAnswer : TServerPacket
        {
            TServerAnswer serverAnswerPacket = default(TServerAnswer);
            object lockObj = new object();
            var rcvThread = new Thread(() =>
            {
                lock (lockObj)
                    serverAnswerPacket = (TServerAnswer)ReceivePacket();
            });
            rcvThread.Start();

            lock (lockObj)
                return serverAnswerPacket;
        }
        public bool TrySendRequestAsync<TClientRequest, TServerAnswer>(TClientRequest requestPacket, out TServerAnswer answerPacket) where TClientRequest : TClientPacket, IPacketRequestable
            where TServerAnswer : TServerPacket
        {
            answerPacket = SendRequestAsync<TClientRequest, TServerAnswer>(requestPacket);
            return answerPacket.Equals(default(TServerAnswer));
        }

        private void ReceivePacketsLoop()
        {
            while (isConnected)
            {
                var availableBytes = socket.Available;
                if (availableBytes != 0)
                {
                    var serverPacket = ReceivePacket();
                    if (serverPacket != null)
                    {
                        if (OnReceivePacket != null && serverPacket.Equals(default(TServerPacket))) // TODO: Find a better solution than the equals/default call
                            OnReceivePacket(serverPacket);
                    }
                    else break;
                }
            }
        }

        private TServerPacket ReceivePacket()
        {
            lock (packetReceivingLock)
            {
                var receivedBytes = socket.Receive(packetReceivingBuffer);

                if (receivedBytes == 0)
                {
                    Disconnect();
                    Console.WriteLine("Disconnected client: 0 bytes received");

                    return default(TServerPacket);
                }
                else
                {
                    var packetBuffer = new byte[receivedBytes];
                    Buffer.BlockCopy(packetReceivingBuffer, 0, packetBuffer, 0, receivedBytes);

                    return PacketConverter.ToPacket<TServerPacket>(packetBuffer);
                }
            }
        }

        private void InitializeSocket()
        {
            socket.LingerState = new LingerOption(true, 1);
            socket.NoDelay = true;

            socket.SendBufferSize = 16384;
            socket.SendTimeout = 6000;

            socket.ReceiveBufferSize = 16384;
            socket.ReceiveTimeout = 6000;

            packetReceivingBuffer = new byte[socket.ReceiveBufferSize];
        }
    }
}
