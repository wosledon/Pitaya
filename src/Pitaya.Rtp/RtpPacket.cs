using Pitaya.BasicType.BitTypes;
using Pitaya.Buffers;
using Pitaya.Buffers.Extensions;
using Pitaya.Core;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Pitaya.Rtp;

/// <summary>
/// RtpPacket
/// </summary>
public struct RtpPacket : IPitayaPacket<RtpPacket>, IPitayaAnalyzable
{
    public RtpPacket()
    {
    }

    #region Rtp Header

    /// <summary>
    /// Version (V): 2 bits default 2
    /// </summary>
    public Bit2 V { get; set; } = new Bit2(2);

    /// <summary>
    /// Padding (P): 1 bit
    /// </summary>
    public Bit P { get; set; } = Bit.Zero;

    /// <summary>
    /// Extension (X): 1 bit
    /// </summary>
    public Bit X { get; set; } = Bit.Zero;

    /// <summary>
    /// CSRC count (CC): 4 bits
    /// </summary>
    public byte CC { get; set; }

    /// <summary>
    /// Marker (M): 1 bit
    /// </summary>
    public Bit M { get; set; } = Bit.Zero;

    /// <summary>
    /// Payload type (PT): 7 bits
    /// </summary>
    public byte PT { get; set; }

    /// <summary>
    /// Sequence number: 16 bits
    /// </summary>
    public ushort SequenceNumber { get; set; }

    /// <summary>
    /// Timestamp: 32 bits
    /// </summary>
    public uint Timestamp { get; set; }

    /// <summary>
    /// Synchronization source (SSRC): 32 bits
    /// </summary>
    public uint SSRC { get; set; }

    /// <summary>
    /// Contributing source (CSRC): 32 bits
    /// <see cref="CC" />
    /// </summary>
    /// <remarks>
    /// CSRC 的个数由CC决定，最多可以有15个CSRC
    /// </remarks>
    public uint[] CSRC { get; set; } = [];
    #endregion

    #region RtpExtension
    /// <summary>
    /// Defined By Profile: 16 bits
    /// </summary>
    public ushort DefinedByProfile { get; set; }

    /// <summary>
    /// Extension length: 16 bits
    /// </summary>
    public ushort Length { get; set; }

    /// <summary>
    /// Header extension body: (unit 32 bit)
    /// </summary>
    public uint[] HeaderExtension { get; set; } = [];
    #endregion

    /// <summary>
    /// Payload
    /// </summary>
    public byte[] Payload { get; set; } = [];

    public byte[] Origin { get; set; } = [];

    ///<inheritdoc />
    public void Analyze(ref BufferReader reader, Utf8JsonWriter writer)
    {
        var header = reader.ReadArray(12);
        writer.WriteNumber(nameof(V), header[0] >> 6);
        writer.WriteNumber(nameof(P), header[0] >> 5);
        writer.WriteNumber(nameof(X), header[0] >> 4);
        writer.WriteNumber(nameof(CC), header[0] & 0x0F);
        writer.WriteNumber(nameof(M), header[1] >> 7);
        writer.WriteNumber(nameof(PT), header[1] & 0x7F);
        writer.WriteNumber(nameof(SequenceNumber), (header[2] << 8) | header[3]);
        writer.WriteNumber(nameof(Timestamp), (header[4] << 24) | (header[5] << 16) | (header[6] << 8) | header[7]);
        writer.WriteNumber(nameof(SSRC), (header[8] << 24) | (header[9] << 16) | (header[10] << 8) | header[11]);
        writer.WriteStartArray(nameof(CSRC));
        foreach (var csrc in CSRC)
        {
            writer.WriteNumberValue(reader.ReadUInt32());
        }
        writer.WriteEndArray();

        if ((header[0] >> 4) == Bit.One)
        {
            writer.WriteNumber(nameof(DefinedByProfile), reader.ReadUInt16());
            writer.WriteNumber(nameof(Length), reader.ReadUInt16());
            writer.WriteStartArray(nameof(HeaderExtension));
            foreach (var he in HeaderExtension)
            {
                writer.WriteNumberValue(reader.ReadUInt32());
            }
            writer.WriteEndArray();
        }

        writer.WriteString(nameof(Payload), reader.ReadContent().ToArray().ToHexString());
    }

    ///<inheritdoc/>
    public RtpPacket Decode(ref BufferReader reader)
    {
        Origin = reader.OriginBuffer.ToArray();

        var packet = new RtpPacket();
        var header = reader.ReadArray(12);
        packet.V = (Bit2)(header[0] >> 6);
        packet.P = (Bit)(header[0] >> 5);
        packet.X = (Bit)(header[0] >> 4);
        packet.CC = (byte)(header[0] & 0x0F);
        packet.M = (Bit)(header[1] >> 7);
        packet.PT = (byte)(header[1] & 0x7F);
        packet.SequenceNumber = (ushort)((header[2] << 8) | header[3]);
        packet.Timestamp = (uint)((header[4] << 24) | (header[5] << 16) | (header[6] << 8) | header[7]);
        packet.SSRC = (uint)((header[8] << 24) | (header[9] << 16) | (header[10] << 8) | header[11]);
        packet.CSRC = new uint[packet.CC];
        for (var i = 0; i < packet.CC; i++)
        {
            packet.CSRC[i] = reader.ReadUInt32();
        }

        if (packet.X == Bit.One)
        {
            packet.DefinedByProfile = (ushort)reader.ReadUInt16();
            packet.Length = reader.ReadUInt16();
            packet.HeaderExtension = new uint[packet.Length];
            for (var i = 0; i < packet.Length; i++)
            {
                packet.HeaderExtension[i] = reader.ReadUInt32();
            }
        }

        packet.Payload = reader.ReadContent().ToArray();
        return packet;
    }

    ///<inheritdoc/>
    public void Encode(ref BufferWriter writer)
    {
        writer.WriteByte((byte)((byte)V << 6 | (byte)P << 5 | (byte)X << 4 | CC), out _);
        writer.WriteByte((byte)((byte)M << 7 | PT), out _);
        writer.WriteUInt16(SequenceNumber, out _);
        writer.WriteUInt32(Timestamp, out _);
        writer.WriteUInt32(SSRC, out _);
        foreach (var csrc in CSRC)
        {
            writer.WriteUInt32(csrc, out _);
        }

        if (X == Bit.One)
        {
            writer.WriteUInt16(DefinedByProfile, out _);
            writer.WriteUInt16(Length, out _);

            foreach (var he in HeaderExtension)
            {
                writer.WriteUInt32(he, out _);
            }
        }
        writer.WriteArray(Payload, out _);
    }
}
