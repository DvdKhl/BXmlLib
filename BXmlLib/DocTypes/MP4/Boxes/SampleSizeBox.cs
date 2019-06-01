using System;
using System.Buffers.Binary;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public readonly ref struct SampleSizeBox {
		private readonly ReadOnlySpan<byte> data;
		public SampleSizeBox(ReadOnlySpan<byte> data) {
			this.data = data;
		}

		public byte Version => data[0];
		public int Flags => BinaryPrimitives.ReadInt32BigEndian(data) & 0x00FFFFFF;

		public uint SampleSize => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(4));
		public uint SampleCount => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(8));
		public ReadOnlySpan<uint> Samples {
			get {
				var samples = new uint[SampleSize != 0 ? 0 : SampleCount];

				for (int i = 0; i < samples.Length; i++) {
					samples[i] = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(i * 4 + 12, 4));
				}

				return samples;
			}
		}
	}
}
