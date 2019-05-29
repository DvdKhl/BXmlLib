using System;
using System.Buffers.Binary;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public ref struct CompactSampleSizeBox {
		private readonly ReadOnlySpan<byte> data;
		public CompactSampleSizeBox(ReadOnlySpan<byte> data) {
			this.data = data;
		}

		public byte Version => data[0];
		public int Flags => BinaryPrimitives.ReadInt32BigEndian(data) & 0x00FFFFFF;

		public uint FieldSize => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(4)) & 0xFF;
		public uint SampleCount => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(8));
		public ReadOnlySpan<ushort> Samples {
			get {
				//var samples = new ushort[SampleCount * 8 / FieldSize + (SampleCount * 8 % FieldSize != 0 ? 1 : 0)];
				var samples = new ushort[SampleCount];
				switch (FieldSize) {
					case 4:
						int pairCount = samples.Length / 2;
						for (int i = 0; i < pairCount; i += 2) {
							samples[i * 2 + 0] = (ushort)((data[i + 12] & 0xF0) >> 4);
							samples[i * 2 + 1] = (ushort)(data[i + 12] & 0x0F);
						}
						if (pairCount * 2 != samples.Length) {
							samples[samples.Length - 1] = (ushort)((data[pairCount + 1] & 0xF0) >> 4);
						}
						break;
					case 8:
						for (int i = 0; i < samples.Length; i++) {
							samples[i] = data[i + 12];
						}
						break;
					case 16:
						for (int i = 0; i < samples.Length; i++) {
							samples[i] = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(i * 2 + 12, 2));
						}
						break;
					default: throw new Exception();
				}

				return samples;
			}
		}
	}
}
