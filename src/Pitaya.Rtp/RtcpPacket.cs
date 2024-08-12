using Pitaya.Buffers;
using Pitaya.Core;
using System.Text.Json;

namespace Pitaya.Rtp;

/// <summary>
/// RtcpPacket
/// </summary>
/// TODO: Implement IPitayaPacket<RtcpPacket>, IPitayaAnalyzable
public struct RtcpPacket : IPitayaPacket<RtcpPacket>, IPitayaAnalyzable
{
    public RtcpPacket()
    {
    }
    #region Doc
    /*
    版本（V）：2位，通常为2。
    填充（P）：1位，如果设置，则报文末尾有填充字节。
    接收报告计数（RC）：5位，表示接收报告块的数量。
    包类型（PT）：8位，表示RTCP报文的类型。
    长度（Length）：16位，表示报文的长度，以32位字为单位，不包括报头。
     */
    #region 发送报告 SR
    /*
     0                   1                   2                   3
     0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |V=2|P|    RC   |   PT=SR=200   |             length            |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                         SSRC of sender                        |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |              NTP timestamp, most significant word             |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |             NTP timestamp, least significant word             |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                         RTP timestamp                         |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                     sender's packet count                     |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                      sender's octet count                     |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                 Report block 1 (if RC > 0)                    |
    |                                                               |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                 Report block 2 (if RC > 1)                    |
    |                                                               |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    */
    #endregion

    #region 接收报告 RP
    /*
     0                   1                   2                   3
     0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |V=2|P|    RC   |   PT=RR=201   |             length            |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                         SSRC of sender                        |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                 Report block 1 (if RC > 0)                    |
    |                                                               |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                 Report block 2 (if RC > 1)                    |
    |                                                               |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     */
    #endregion

    #region 源描述 SDES
    /*
     0                   1                   2                   3
     0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |V=2|P|    SC   |  PT=SDES=202  |             length            |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                          SSRC/CSRC_1                          |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                           SDES items                          |
    |                              ...                              |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     */

    #endregion

    #region 离开 Bye
    /*
     0                   1                   2                   3
     0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |V=2|P|    SC   |   PT=BYE=203  |             length            |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                           SSRC/CSRC                           |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    :                              ...                              :
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |     length    |               reason for leaving              |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     */
    #endregion

    #region 应用程序特定 App
    /*
     0                   1                   2                   3
     0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |V=2|P| subtype |   PT=APP=204  |             length            |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                           SSRC/CSRC                           |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                          name (ASCII)                         |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    |                   application-dependent data                  |
    |                              ...                              |
    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     */
    #endregion

    #endregion Doc

    public byte[] Origin { get; set; } = [];

    /// <summary>
    /// Header
    /// </summary>
    public HeaderPacket Header { get; set; } = new();

    /// <summary>
    /// Rtcp Body
    /// </summary>
    public struct BodyPacket { }

    /// <summary>
    /// SR
    /// </summary>
    public struct SenderReport : BodyPacket
    {
        public uint SSRC { get; set; }
        public ulong NTP { get; set; }
        public uint RTP { get; set; }
        public uint PacketCount { get; set; }
        public uint OctetCount { get; set; }
    }

    /// <summary>
    /// RP
    /// </summary>
    public struct ReceiverReport : BodyPacket
    {
        public uint SSRC { get; set; }
    }

    /// <summary>
    /// SDES
    /// </summary>
    public struct SourceDescription : BodyPacket
    {
        public uint SSRC { get; set; }
        public string SDES { get; set; }
    }

    /// <summary>
    /// Bye
    /// </summary>
    public struct Bye : BodyPacket
    {
        public uint SSRC { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// App
    /// </summary>
    public struct ApplicationSpecific : BodyPacket
    {
        public uint SSRC { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }

    public BodyPacket Body { get; set; } = new BodyPacket();

    /// <summary>
    /// RtcpPacket Header
    /// </summary>
    public struct HeaderPacket : IPitayaPacket<HeaderPacket>, IPitayaAnalyzable
    {
        public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
        {
            throw new NotImplementedException();
        }

        public HeaderPacket Decode(ref BufferReader reader)
        {
            throw new NotImplementedException();
        }

        public void Encode(ref BufferWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        throw new NotImplementedException();
    }

    public RtcpPacket Decode(ref BufferReader reader)
    {
        throw new NotImplementedException();
    }

    public void Encode(ref BufferWriter writer)
    {
        throw new NotImplementedException();
    }
}
