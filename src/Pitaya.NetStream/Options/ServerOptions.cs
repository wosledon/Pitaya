using Pitaya.NetStream.Enums;
using System.Net;
using System.Text;

namespace Pitaya.NetStream.Options;

public class ServerOptions
{
    public IPEndPoint LocalEndPoint { get; set; } = new IPEndPoint(IPAddress.Any, 0);

    public int Backlog { get; set; } = 100;

    public Encoding Encoding { get; set; } = Encoding.UTF8;

    public int ReceiveBufferSize { get; set; } = 8192;

    public int SendBufferSize { get; set; } = 8192;
    public NetStreamType NetStreamType { get; }

    public ServerOptions()
    {
    }

    public ServerOptions(string host, ushort port, NetStreamType netStreamType = NetStreamType.TCP)
    {
        if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));

        if (port == 0) throw new ArgumentOutOfRangeException(nameof(port));

        if (host.Equals("any", StringComparison.OrdinalIgnoreCase))
        {
            LocalEndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        if (host.Equals("loopback", StringComparison.OrdinalIgnoreCase))
        {
            LocalEndPoint = new IPEndPoint(IPAddress.Loopback, port);
        }

        if (IPAddress.TryParse(host, out var ip))
        {
            LocalEndPoint = new IPEndPoint(ip, port);
        }
        else
        {
            var ipAddresses = Dns.GetHostAddresses(host);
            if (ipAddresses.Length == 0) throw new ArgumentException("Invalid host", nameof(host));

            LocalEndPoint = new IPEndPoint(ipAddresses[0], port);
        }

        NetStreamType = netStreamType;
    }

    public ServerOptions(IPEndPoint localEndPoint)
    {
        LocalEndPoint = localEndPoint;
    }

    public void Validate()
    {
        if (LocalEndPoint == null) throw new ArgumentNullException(nameof(LocalEndPoint));

        if (LocalEndPoint.Port == 0) throw new ArgumentOutOfRangeException(nameof(LocalEndPoint.Port));
    }
}