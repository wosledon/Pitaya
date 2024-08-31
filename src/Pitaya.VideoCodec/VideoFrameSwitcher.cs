using Pitaya.Buffers;
using Pitaya.Core;
using Pitaya.VideoCodec.Frames;

namespace Pitaya.VideoCodec;

public class VideoFrameSwitcher
{
    public VideoFrameSwitcher()
    {

    }

    public IPitayaPayloadPacket Switch(byte[] frame)
    {
        return new H264Packet
        {
            Origin = frame
        };
    }
}
