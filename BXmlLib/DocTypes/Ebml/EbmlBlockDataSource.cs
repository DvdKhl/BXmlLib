using BXmlLib.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXmlLib.DocTypes.Ebml {
    public static class VIntConsts {
        public static readonly long[] VIntReserved = { 0x7F, 0x3FFF, 0x1FFFFF, 0x0FFFFFFF, 0x07FFFFFFFF, 0x03FFFFFFFFFF, 0x01FFFFFFFFFFFF, 0x00FFFFFFFFFFFFFF };
        public static readonly byte[] VIntLength = Enumerable.Range(0, 256).Select(x => (byte)Math.Ceiling(8 - Math.Log(x, 2))).ToArray();
        public static readonly byte[] VIntValue = Enumerable.Range(0, 256).Select(x => (byte)(x & ((1 << (8 - VIntLength[x])) - 1))).ToArray();

    }

    public abstract class EbmlBlockDataSource : BXmlDataBlockSource {
        private static long ReadVInt(ReadOnlySpan<byte> data, int maxLength, out int vintLength) {
            if(data.Length == 0) {
                vintLength = -1;
                return ~BXmlElementHeader.UnexpectedEndOfStreamError;
            }
            byte encodedSize = data[0];

            vintLength = VIntConsts.VIntLength[encodedSize];
            int bytesToRead = vintLength - 1;
            if(bytesToRead == maxLength) return ~BXmlElementHeader.InvalidLengthDescriptorError;
            if(bytesToRead + 1 > data.Length) return ~BXmlElementHeader.UnexpectedEndOfStreamError;

            long value = 0;
            for(int i = 0; i < bytesToRead; i++) {
                value |= (long)data[i + 1] << ((bytesToRead - i - 1) << 3);
            }
            value += VIntConsts.VIntValue[encodedSize] << (bytesToRead << 3);

            return value == VIntConsts.VIntReserved[bytesToRead] ? ~BXmlElementHeader.UnknownLength : value;
        }

        public override ReadOnlySpan<byte> ReadIdentifier(ref BXmlElementHeader header) {
            if(IsEndOfStream) {
                header.Mutate(
                    ~BXmlElementHeader.UnexpectedEndOfStreamError, 
                    ~BXmlElementHeader.SubsquentError, 
                    ~BXmlElementHeader.SubsquentError
                );
                return ReadOnlySpan<byte>.Empty;
            }

            var data = GetDataBlock(4 + 8);

            var headerPos = Position;
            int identLength = VIntConsts.VIntLength[data[0]];
            var encodedIdent = data.Slice(0, identLength);

            if(identLength > 4) {
                header.Mutate(
                    identLength,
                    ~BXmlElementHeader.InvalidLengthDescriptorError,
                    ~BXmlElementHeader.SubsquentError
                );
                return encodedIdent;
            }

            var dataLength = ReadVInt(data.Slice(identLength), 8, out int dataVIntLength);

            if(dataLength < 0 && (~dataLength & BXmlElementHeader.Mask & BXmlElementHeader.Error) != 0) {
                header.Mutate(
                    identLength,
                    dataLength, //Contains BXmlElementHeader.*Error
                    ~BXmlElementHeader.SubsquentError
                );
                return encodedIdent;
            }

            Advance(identLength + dataVIntLength);

            var dataPos = headerPos + identLength + dataVIntLength;
            header.Mutate(headerPos, dataPos, dataLength);
            return encodedIdent;
        }
    }
}
