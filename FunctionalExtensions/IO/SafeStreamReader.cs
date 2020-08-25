using System.IO;
using System.Text;
using LanguageExt;

namespace FunctionalExtensions.IO
{
  public class SafeStreamReader : StreamReader
  {
    public SafeStreamReader(Stream stream) : base(stream)
    {
    }

    public SafeStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks) : base(stream, detectEncodingFromByteOrderMarks)
    {
    }

    public SafeStreamReader(Stream stream, Encoding encoding) : base(stream, encoding)
    {
    }

    public SafeStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks) : base(stream, encoding, detectEncodingFromByteOrderMarks)
    {
    }

    public SafeStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize) : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
    {
    }

    public SafeStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen) : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
    {
    }

    public SafeStreamReader(string path) : base(path)
    {
    }

    public SafeStreamReader(string path, bool detectEncodingFromByteOrderMarks) : base(path, detectEncodingFromByteOrderMarks)
    {
    }

    public SafeStreamReader(string path, Encoding encoding) : base(path, encoding)
    {
    }

    public SafeStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks) : base(path, encoding, detectEncodingFromByteOrderMarks)
    {
    }

    public SafeStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize) : base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize)
    {
    }

    public new Option<int> Read() =>
      EndOfStream ?
        Option<int>.None :
        Option<int>.Some(base.Read());

    public new Option<int> Peek() =>
      EndOfStream ?
        Option<int>.None :
        Option<int>.Some(base.Peek());
  }
}