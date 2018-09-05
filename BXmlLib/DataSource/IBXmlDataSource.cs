using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DataSource {


    public struct BXmlElementHeader {
        public const int UnknownLength = 1;

        public const int Error = 512;
        public const int InvalidLengthDescriptorError = Error | 1;
        public const int UnexpectedEndOfStreamError = Error | 2;
        public const int SubsquentError = Error | 3;

        public const int Mask = 1023;

        public long Offset { get; private set; }
        public long DataOffset { get; private set; }
        public long DataLength { get; private set; }

        public void Mutate(long offset, long dataOffset, long dataLength) {
            Offset = offset;
            DataOffset = dataOffset;
            DataLength = dataLength;
        }
        public void Mutate() {
            Offset = -1;
            DataOffset = -1;
            DataLength = -1;
        }
    }

    public interface IBXmlDataSource {
        long Position { get; set; }
        bool IsEndOfStream { get; }

        ReadOnlySpan<byte> ReadIdentifier(ref BXmlElementHeader header);
        ReadOnlySpan<byte> ReadData(int bytesToRead);
    }
}
