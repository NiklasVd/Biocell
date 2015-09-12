using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Biocell.Network
{
    public class GameServer<TServerPacket, TClientPacket> : Server<TServerPacket, TClientPacket>, IDisposable
        where TServerPacket : IServerPacket where TClientPacket : IClientPacket
    {
        private readonly UdpClient udpClient;
        private readonly IPEndPoint multicastEndPoint;

        public GameServer(int port, IPEndPoint multicastEndPoint)
            : base(port)
        {
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, multicastEndPoint.Port));

            this.multicastEndPoint = multicastEndPoint;
            udpClient.JoinMulticastGroup(multicastEndPoint.Address);
        }
        ~GameServer()
        {
            udpClient.DropMulticastGroup(multicastEndPoint.Address);
            udpClient.Close();
        }

        public void SendMulticastPacket(TServerPacket packet)
        {
            var packetBytes = PacketConverter.ToBytes(packet);
            var sentBytes = udpClient.Send(packetBytes, packetBytes.Length, multicastEndPoint);

            if (sentBytes == 0)
            { /* What happens then? */ }
        }

        public void Dispose()
        {
            udpClient.Close();
            Close();
        }
    }
}
