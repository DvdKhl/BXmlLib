using System;
using System.Buffers.Binary;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public ref struct HintSampleEntry {
		private readonly ReadOnlySpan<byte> data;
		public HintSampleEntry(ReadOnlySpan<byte> data) {
			this.data = data;
		}

		public ReadOnlySpan<byte> Format => data.Slice(0, 4);
		public ReadOnlySpan<byte> Reserved => data.Slice(4, 6);
		public ushort DataReferenceIndex => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(10));
		public ReadOnlySpan<byte> Data => data.Slice(12);
	}
}
