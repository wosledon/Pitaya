namespace Pitaya.Flv;

public interface IAmf3MetaData
{
    ushort FieldNameLength { get; set; }
    string FieldName { get; set; }
    /// <summary>
    /// ref:video_file_format_spec_v10.pdf scriptdatavalue  type
    /// </summary>
    byte DataType { get; set; }
    object Value { get; set; }

    ReadOnlySpan<byte> ToBuffer();
}