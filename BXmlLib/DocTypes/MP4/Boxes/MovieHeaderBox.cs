using System;
using System.Buffers.Binary;
using System.Linq;
using System.Runtime.InteropServices;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public readonly ref struct MovieHeaderBox {
		private readonly ReadOnlySpan<byte> data;
		public MovieHeaderBox(ReadOnlySpan<byte> data) {
			this.data = data;
		}
		public byte Version => data[0];
		public int Flags => BinaryPrimitives.ReadInt32BigEndian(data) & 0x00FFFFFF;

		public ulong CreationTime => Version == 1 ? BinaryPrimitives.ReadUInt64BigEndian(data.Slice(4)) : BinaryPrimitives.ReadUInt32BigEndian(data.Slice(4));
		public DateTime CreationDate => MP4DocType.DateOffset.AddSeconds(CreationTime);

		public ulong ModificationTime => Version == 1 ? BinaryPrimitives.ReadUInt64BigEndian(data.Slice(12)) : BinaryPrimitives.ReadUInt32BigEndian(data.Slice(8));
		public DateTime ModificationDate => MP4DocType.DateOffset.AddSeconds(ModificationTime);

		public uint Timescale => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(Version == 1 ? 20 : 12));
		public ulong Duration => Version == 1 ? BinaryPrimitives.ReadUInt64BigEndian(data.Slice(24)) : BinaryPrimitives.ReadUInt32BigEndian(data.Slice(16));
		public float Rate => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(Version == 1 ? 32 : 20)) / (float)0x00010000;
		public float Volume => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(Version == 1 ? 36 : 24)) / (float)0x0100;
		public ushort Reserved1 => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(Version == 1 ? 38 : 26));
		public ulong Reserved2 => BinaryPrimitives.ReadUInt64BigEndian(data.Slice(Version == 1 ? 40 : 28));

		public ReadOnlySpan<int> Matrix => MemoryMarshal.Cast<byte, int>(data.Slice(Version == 1 ? 48 : 36, 4 * 9)); //TODO BigEndian
		public ReadOnlySpan<int> PreDefined => MemoryMarshal.Cast<byte, int>(data.Slice(Version == 1 ? 84 : 72, 4 * 6)); //TODO BigEndian
		public uint NextTrack => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(Version == 1 ? 108 : 96));
	}
}
