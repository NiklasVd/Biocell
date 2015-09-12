using Biocell.Network.Utility;
using NetSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Biocell.Network
{
    public static class PacketConverter
    {
        private static Serializer serializer;

        static PacketConverter()
        {
            var packetTypes = Assembly.GetAssembly(typeof(IPacket)).
                GetTypes().
                Where(t =>
                {
                    return t.GetInterface(typeof(IPacket).Name) != null && t.IsValueType;
                });

            serializer = new Serializer(packetTypes);
        }

        public static byte[] ToBytes<T>(T packet) where T : IPacket
        {
            using (var mStream = new MemoryStream())
            {
                serializer.Serialize(mStream, packet);
                return mStream.ToArray();
            }
        }

        public static T ToPacket<T>(byte[] bytes) where T : IPacket
        {
            using (var mStream = new MemoryStream(bytes))
            {
                var deserializedObj = serializer.Deserialize(mStream);
                return deserializedObj != null ? (T)deserializedObj : default(T);
            }
        }

        public static byte[] ToCompressedBytes<T>(T packet) where T : IPacket
        {
            var packetBytes = ToBytes(packet);

            var compressedPacketBytes = new byte[packetBytes.Length];
            HuffmanCompressor.Compress(packetBytes, out compressedPacketBytes);

            return compressedPacketBytes;
        }
        public static T ToDecompressedPacket<T>(byte[] compressedBytes) where T : IPacket
        {
            var decompressedPacketBytes = new byte[compressedBytes.Length];
            HuffmanCompressor.Decompress(compressedBytes, out decompressedPacketBytes);

            return ToPacket<T>(decompressedPacketBytes);
        }
    }
}
