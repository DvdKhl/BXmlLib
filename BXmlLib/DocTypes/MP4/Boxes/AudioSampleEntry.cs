using System;
using System.Buffers.Binary;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public ref struct AudioSampleEntry {
		private readonly ReadOnlySpan<byte> data;
		public AudioSampleEntry(ReadOnlySpan<byte> data) {
			this.data = data;
		}

		public ReadOnlySpan<byte> Format => data.Slice(0, 4);
		public ReadOnlySpan<byte> Reserved => data.Slice(4, 6);
		public ushort DataReferenceIndex => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(10));
		public ulong Reserved1 => BinaryPrimitives.ReadUInt64BigEndian(data.Slice(12));
		public ushort ChannelCount => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(20));
		public ushort SampleSize => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(22));
		public ushort PreDefined => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(24));
		public ushort Reserved2 => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(26));
		public uint Samplerate => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(28)) >> 16;

	}
}
