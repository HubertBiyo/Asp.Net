using System;
using System.IO;
using System.Text;

namespace QOL.Common.IO
{
    public static class AutoDetectEncodingStreamReader
    {
        public static bool IsUnicode(Encoding encoding)
        {
            int codepage = encoding.CodePage;
            // return true if codepage is any UTF codepage
            return codepage == 65001 || codepage == 65000 || codepage == 1200 || codepage == 1201;
        }

        /// <summary>
        /// Read the Specified File Content
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static string ReadAllText(Stream stream, Encoding defaultEncoding)
        {
            using (StreamReader reader = OpenStream(stream, null, defaultEncoding))
            {
                return reader.ReadToEnd();
            }
        }

        private static StreamReader OpenStream(Stream stream, Encoding suggestedEncoding, Encoding defaultEncoding)
        {
            if (stream.Length > 3)
            {
                // the autodetection of StreamReader is not capable of detecting the difference
                // between ISO-8859-1 and UTF-8 without BOM.
                int firstByte = stream.ReadByte();
                int secondByte = stream.ReadByte();
                switch ((firstByte << 8) | secondByte)
                {
                    case 0x0000: // either UTF-32 Big Endian or a binary file; use StreamReader
                    case 0xfffe: // Unicode BOM (UTF-16 LE or UTF-32 LE)
                    case 0xfeff: // UTF-16 BE BOM
                    case 0xefbb: // start of UTF-8 BOM
                        // StreamReader autodetection works
                        stream.Position = 0;
                        return new StreamReader(stream);
                    default:
                        return AutoDetect(stream, (byte)firstByte, (byte)secondByte, defaultEncoding);
                }
            }
            if (suggestedEncoding != null)
            {
                return new StreamReader(stream, suggestedEncoding);
            }
            return new StreamReader(stream);
        }

        private static StreamReader AutoDetect(Stream stream, byte firstByte, byte secondByte, Encoding defaultEncoding)
        {
            int max = (int)Math.Min(stream.Length, 500000); // look at max. 500 KB
            const int ASCII = 0;
            const int Error = 1;
            const int UTF8 = 2;
            const int UTF8Sequence = 3;
            int state = ASCII;
            int sequenceLength = 0;
            for (int i = 0; i < max; i++)
            {
                byte b;
                switch (i)
                {
                    case 0:
                        b = firstByte;
                        break;
                    case 1:
                        b = secondByte;
                        break;
                    default:
                        b = (byte)stream.ReadByte();
                        break;
                }
                if (b < 0x80)
                {
                    // normal ASCII character
                    if (state == UTF8Sequence)
                    {
                        state = Error;
                        break;
                    }
                }
                else if (b < 0xc0)
                {
                    // 10xxxxxx : continues UTF8 byte sequence
                    if (state == UTF8Sequence)
                    {
                        --sequenceLength;
                        if (sequenceLength < 0)
                        {
                            state = Error;
                            break;
                        }
                        if (sequenceLength == 0)
                        {
                            state = UTF8;
                        }
                    }
                    else
                    {
                        state = Error;
                        break;
                    }
                }
                else if (b >= 0xc2 && b < 0xf5)
                {
                    // beginning of byte sequence
                    if (state == UTF8 || state == ASCII)
                    {
                        state = UTF8Sequence;
                        if (b < 0xe0)
                        {
                            sequenceLength = 1; // one more byte following
                        }
                        else if (b < 0xf0)
                        {
                            sequenceLength = 2; // two more bytes following
                        }
                        else
                        {
                            sequenceLength = 3; // three more bytes following
                        }
                    }
                    else
                    {
                        state = Error;
                        break;
                    }
                }
                else
                {
                    // 0xc0, 0xc1, 0xf5 to 0xff are invalid in UTF-8 (see RFC 3629)
                    state = Error;
                    break;
                }
            }
            stream.Position = 0;
            switch (state)
            {
                case ASCII:
                case Error:
                    // when the file seems to be ASCII or non-UTF8,
                    // we read it using the user-specified encoding so it is saved again
                    // using that encoding.
                    if (IsUnicode(defaultEncoding))
                    {
                        // the file is not Unicode, so don't read it using Unicode even if the
                        // user has choosen Unicode as the default encoding.

                        // If we don't do this, SD will end up always adding a Byte Order Mark
                        // to ASCII files.
                        defaultEncoding = Encoding.Default; // use system encoding instead
                    }
                    return new StreamReader(stream, defaultEncoding);
                default:
                    return new StreamReader(stream);
            }
        }
    }
}
