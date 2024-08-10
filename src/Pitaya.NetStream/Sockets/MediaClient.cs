using Pitaya.Buffers;
using Pitaya.NetStream.Core;
using Pitaya.NetStream.Enums;
using Pitaya.NetStream.Options;
using System.Net.Sockets;

namespace Pitaya.NetStream.Sockets;

public class MediaClient<TPacket> : IMediaClient<TPacket>
    where TPacket : IPitayaPacket<TPacket>
{
    public MediaClient(ClientOptions clientOption)
    {
        Options = clientOption;

        Options.Validate();

        if (Options.NetStreamType == NetStreamType.UDP)
        {
            _udpClient = new UdpClient();
        }
        else if(Options.NetStreamType == NetStreamType.TCP)
        {
            _tcpClient = new TcpClient();
        }
        else
        {
            throw new InvalidOperationException("Invalid NetStreamType");
        }
    }

    private readonly UdpClient? _udpClient;
    private readonly TcpClient? _tcpClient;

    public ClientOptions Options { get; }

    public async Task ConnectAsync() 
    {
        if(Options.NetStreamType == NetStreamType.UDP)
        {
            UdpConnect();
        }
        else if(Options.NetStreamType == NetStreamType.TCP)
        {
            await TcpConnectAsync();
        }
    }

    async Task TcpConnectAsync()
    {

       if(_tcpClient == null) return;

        await _tcpClient.ConnectAsync(Options.RemoteEndPoint);
    }

    void UdpConnect()
    {
        if(_udpClient == null) return;

        _udpClient.Connect(Options.RemoteEndPoint);
    }

    public void Disconnect()
    {
        _udpClient?.Close();
        _tcpClient?.Close();
    }

    public async Task SendAsync(TPacket packet)
    {
        if(Options.NetStreamType == NetStreamType.UDP)
        {
            await UdpSendAsync(packet);
        }
        else if(Options.NetStreamType == NetStreamType.TCP)
        {
            await TcpSendAsync(packet);
        }
        else
        {
            throw new InvalidOperationException("Invalid NetStreamType");
        }
    }

    async Task UdpSendAsync(TPacket packet)
    {
        if (_udpClient == null) throw new InvalidOperationException("UdpClient is null");

        var buffer = PitayaConvert.Serialize(packet);

        await _udpClient.SendAsync(buffer, buffer.Length, Options.RemoteEndPoint);
    }

    async Task TcpSendAsync(TPacket packet)
    {
        if (_tcpClient == null) throw new InvalidOperationException("TcpClient is null");

        var buffer = PitayaConvert.Serialize(packet);

        await _tcpClient.GetStream().WriteAsync(buffer, 0, buffer.Length);
    }

    public async Task ReceiveAsync(Action<byte[]> receiveAction)
    {
        if(Options.NetStreamType == NetStreamType.UDP)
        {
            await UdpReceiveAsync(receiveAction);
        }
        else if(Options.NetStreamType == NetStreamType.TCP)
        {
            await TcpReceiveAsync(receiveAction);
        }
        else
        {
            throw new InvalidOperationException("Invalid NetStreamType");
        }
    }

    async Task UdpReceiveAsync(Action<byte[]> receiveAction)
    {
        if (_udpClient == null) throw new InvalidOperationException("UdpClient is null");

        var result = await _udpClient.ReceiveAsync();

        receiveAction(result.Buffer);
    }

    async Task TcpReceiveAsync(Action<byte[]> receiveAction)
    {
        if (_tcpClient == null) throw new InvalidOperationException("TcpClient is null");

        var buffer = new byte[Options.ReceiveBufferSize];

        var read = await _tcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length);

        receiveAction(buffer[..read]);
    }

    public void Dispose()
    {
        Disconnect();
    }
}
