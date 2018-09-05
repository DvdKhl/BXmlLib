//Mod. BSD License (See LICENSE file) DvdKhl (DvdKhl@web.de)
using System;

namespace BXmlLib.DocTypes.Matroska {
    public ref struct MatroskaBlock {
        private static long ReadVInt(ReadOnlySpan<byte> data, ref int length) {
            byte bytesToRead = 0;
            byte mask = 1 << 7;
            var encodedSize = data[length++];

            while((mask & encodedSize) == 0 && bytesToRead++ < 8) mask = (byte)(mask >> 1);

            long value = 0;
            for(int i = 0; i < bytesToRead; i++, length++) {
                if(length == data.Length) return 0;
                value += (long)data[length] << ((bytesToRead - i - 1) << 3);
            }

            return value + ((encodedSize ^ mask) << (bytesToRead << 3));
        }

        private static byte GetVIntSize(byte encodedSize) {
            byte mask = 1 << 7;
            byte vIntLength = 0;
            while((mask & encodedSize) == 0 && vIntLength++ < 8) mask = (byte)(mask >> 1);
            if(vIntLength == 9) return 0; //TODO Add Warning
            return ++vIntLength;
        }

        public MatroskaBlock(ReadOnlySpan<byte> data) {
            int offset = 0;
            TrackNumber = (int)ReadVInt(data, ref offset);
            TimeCode = (short)((data[offset] << 8) + data[offset + 1]); offset += 2;

            Flags = (BlockFlag)data[offset++];
            var laceType = (LaceType)(Flags & BlockFlag.LaceMask);
            if(laceType != LaceType.None) {
                FrameCount = data[offset++];
                if(laceType == LaceType.Ebml) {
                    for(int i = 0; i < FrameCount; i++) offset += GetVIntSize(data[offset]);

                } else if(laceType == LaceType.Xiph) {
                    int i = 0;
                    while(i++ != FrameCount) if(data[offset++] != 0xFF) break;
                }
            } else FrameCount = 1;

            RawData = data;
            Data = data.Slice(offset);
        }

        public int TrackNumber { get; }
        public short TimeCode { get; }
        public BlockFlag Flags { get; }
        public LaceType LacingType => (LaceType)(Flags & BlockFlag.LaceMask);
        public byte FrameCount { get;}
        public ReadOnlySpan<byte> RawData { get; }
        public ReadOnlySpan<byte> Data { get; }

        public enum BlockFlag : byte {
            Discardable = 0x01,
            LaceMask = 0x06,
            Invisible = 0x08,
            Keyframe = 0x80
        }
        public enum LaceType : byte {
            None = 0x00,
            Xiph = 0x02,
            Fixed = 0x04,
            Ebml = 0x06
        }
    }
}
