using Pitaya.Buffers;
using Pitaya.NetStream.Core;

namespace Pitaya.NetStream.Sockets;

public class SocketServer<TPacket> : IMediaServer
    where TPacket : IPitayaPacket<TPacket>
{

}