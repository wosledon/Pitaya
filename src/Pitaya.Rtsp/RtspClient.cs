using System.Net;

namespace Pitaya.Rtsp;

public enum RtspConnectType
{
    TCP,
    UDP,
    Http
}

public class RtspClient
{
    public RtspClient(string url, RtspConnectType connectType = RtspConnectType.TCP)
    { }

    public RtspClient(IPEndPoint iPEndPoint, string username, string passwaord)
    { }

    public Task ConnectAsync()
    {
        throw new NotImplementedException();
    }

    public Task PlayAsync()
    {
        throw new NotImplementedException();
    }

    public Task PauseAsync()
    {
        throw new NotImplementedException();
    }

    public Task StopAsync()
    {
        throw new NotImplementedException();
    }

    public Task CloseAsync()
    {
        throw new NotImplementedException();
    }

    public void OnMessageReceived(Action<string> action)
    {
        throw new NotImplementedException();
    }

    public void OnError(Action<Exception> action)
    {
        throw new NotImplementedException();
    }
}

public class RtspServer
{
}