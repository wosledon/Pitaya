using Pitaya.NetStream.Enums;
using System.Net;
using System.Text;

namespace Pitaya.NetStream.Options;

public interface IPitayaOptions
{
    void Validate();
}

public class ClientOptions
{
    public IPEndPoint RemoteEndPoint { get; set; } = new IPEndPoint(IPAddress.Any, 0);
    public NetStreamType NetStreamType { get; set; } = NetStreamType.TCP;
    public string UserAgaent { get; set; } = "Pitaya";
    public int ReceiveBufferSize { get; set; } = 8192;
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    public ClientOptions()
    {
    }

    public ClientOptions(string host, ushort port, NetStreamType netStreamType = NetStreamType.TCP)
    {
        if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));

        if (port == 0) throw new ArgumentOutOfRangeException(nameof(port));

        if (host.Equals("any", StringComparison.OrdinalIgnoreCase))
        {
            RemoteEndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        if (host.Equals("loopback", StringComparison.OrdinalIgnoreCase))
        {
            RemoteEndPoint = new IPEndPoint(IPAddress.Loopback, port);
        }

        if (IPAddress.TryParse(host, out var ip))
        {
            RemoteEndPoint = new IPEndPoint(ip, port);
        }
        else
        {
            var ipAddresses = Dns.GetHostAddresses(host);
            if (ipAddresses.Length == 0) throw new ArgumentException("Invalid host", nameof(host));

            RemoteEndPoint = new IPEndPoint(ipAddresses[0], port);
        }

        NetStreamType = netStreamType;
    }

    public ClientOptions(IPEndPoint remoteEndPoint, NetStreamType netStreamType = NetStreamType.TCP)
    {
        RemoteEndPoint = remoteEndPoint;
        NetStreamType = netStreamType;
    }

    public void Validate()
    {
        if (RemoteEndPoint == null) throw new ArgumentNullException(nameof(RemoteEndPoint));

        if (RemoteEndPoint.Port == 0) throw new ArgumentOutOfRangeException(nameof(RemoteEndPoint.Port));
    }
}

public class SocketOptions : IPitayaOptions
{
    public IPEndPoint RemoteEndPoint { get; internal set; }

    public void Validate()
    {
        throw new NotImplementedException();
    }
}