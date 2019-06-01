using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public readonly ref struct FileTypeBox {
		private ReadOnlySpan<byte> Data { get; }

		public int MajorBrands => BinaryPrimitives.ReadInt32BigEndian(Data) ;
		public int MinorVersion => BinaryPrimitives.ReadInt32BigEndian(Data.Slice(4)) ;
		public ReadOnlySpan<int> CompatibleBrands => MemoryMarshal.Cast<byte, int>(
			Data.Slice(8).ToArray().Reverse().ToArray()
		).ToArray().Reverse().ToArray();

		public FileTypeBox(ReadOnlySpan<byte> data) {
			Data = data;
		}
	}
}
