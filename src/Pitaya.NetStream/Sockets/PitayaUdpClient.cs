using Pitaya.Buffers;
using Pitaya.NetStream.Core;
using Pitaya.NetStream.Options;
using System.Net.Sockets;

namespace Pitaya.NetStream.Sockets;

public class PitayaUdpClient<TPacket> 
    : IMediaClient<TPacket>
    where TPacket : IPitayaPacket<TPacket>
{
    readonly UdpClient _udpClient;

    public PitayaUdpClient(SocketOptions options)
    {
        Options = options;

        Options.Validate();
        _udpClient = new UdpClient();
    }

    public SocketOptions Options { get; }

    public Task ConnectAsync()
    {
        _udpClient.Connect(Options.RemoteEndPoint);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _udpClient.Close();
    }

    public async Task ReceiveAsync(Action<byte[]> receiveAction)
    {
        var buffer = await _udpClient.ReceiveAsync();

        receiveAction(buffer.Buffer);
    }

    public async Task SendAsync(TPacket packet)
    {
        var buffer = packet.Serialize();

        await _udpClient.SendAsync(buffer, buffer.Length);
    }
}
