using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Network
{
    // A requestable packet that relies on an answer from a server
    public interface IPacketRequestable
    {
    }

    // A packet, implemented by every type that can be sent over the network to be processed by client/server
    public interface IPacket
    {
    }

    // A packet sent from the server
    public interface IServerPacket : IPacket
    {
    }
    // A packet sent from the client
    public interface IClientPacket : IPacket
    {
    }

    // A packet sent from a game server
    public interface IServerGamePacket : IServerPacket
    {
        ServerGamePacketType Type { get; }
    }
    // A packet sent from a game client
    public interface IClientGamePacket : IClientPacket
    {
        ClientGamePacketType Type { get; }
    }
}
