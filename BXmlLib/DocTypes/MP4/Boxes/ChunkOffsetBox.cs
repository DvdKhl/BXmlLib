using System;
using System.Buffers.Binary;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public readonly ref struct ChunkOffsetBox {
		private readonly ReadOnlySpan<byte> data;
		public ChunkOffsetBox(ReadOnlySpan<byte> data) {
			this.data = data;
		}

		public byte Version => data[0];
		public int Flags => BinaryPrimitives.ReadInt32BigEndian(data) & 0x00FFFFFF;

		public uint EntryCount => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(4));
		public ReadOnlySpan<ulong> ChunkOffsets {
			get {
				var chunkOffsets = new ulong[EntryCount];

				for (int i = 0; i < chunkOffsets.Length; i++) {
					chunkOffsets[i] = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(i * 4 + 8));
				}

				return chunkOffsets;
			}
		}
	}
}
