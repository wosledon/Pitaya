﻿using Pitaya.Buffers;

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
