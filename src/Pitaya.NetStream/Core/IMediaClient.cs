using Pitaya.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pitaya.NetStream.Core;

public interface IMediaClient<TPacket> : IDisposable
    where TPacket : IPitayaPacket<TPacket>
{
    Task ConnectAsync();
    Task ReceiveAsync(Action<byte[]> receiveAction);
    Task SendAsync(TPacket packet);
}