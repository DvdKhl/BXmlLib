using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public readonly ref struct VisualSampleEntry {
		private readonly ReadOnlySpan<byte> data;
		public VisualSampleEntry(ReadOnlySpan<byte> data) {
			this.data = data;
		}

		public ReadOnlySpan<byte> Format => data.Slice(0, 4);
		public ReadOnlySpan<byte> Reserved => data.Slice(4, 6);
		public ushort DataReferenceIndex => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(10));
		public ushort Predefined1 => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(12));
		public ushort Reserved1 => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(14));
		public ReadOnlySpan<int> Predefined2 => MemoryMarshal.Cast<byte, int>(data.Slice(16, 12));
		public ushort Width => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(28));
		public ushort Height => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(30));
		public double HorizontalResolution => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(32)) / (double)0x00010000;
		public double VerticalResolution => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(36)) / (double)0x00010000;
		public uint Reserved2 => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(40));
		public ushort FrameCount => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(44));
		public ReadOnlySpan<byte> CompressorName => data.Slice(46, 32);
		public ushort Depth => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(78));
		public ushort Predefined3 => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(80));
	}
}
