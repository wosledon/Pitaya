namespace Pitaya.Buffers.Core;

ref partial struct BufferWriterAux
{
    private Span<byte> _buffer;

    public BufferWriterAux(Span<byte> buffer)
    {
        _buffer = buffer;
        WrittenPosition = 0;
        BeforeCodingWrittenPosition = 0;
    }

    public int BeforeCodingWrittenPosition { get; internal set; }

    public int WrittenPosition { get; private set; }

    public void Advance(int count) => WrittenPosition += count;

    public Span<byte> Written => _buffer[..WrittenPosition];

    public Span<byte> Free => _buffer[WrittenPosition..];
}
