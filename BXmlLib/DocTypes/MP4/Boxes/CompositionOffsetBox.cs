using System;
using System.Buffers.Binary;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public ref struct CompositionOffsetBox {
		public struct SampleItem {
			public uint Count { get; internal set; }
			public uint Offset { get; internal set; }
		}


		private readonly ReadOnlySpan<byte> data;
		public CompositionOffsetBox(ReadOnlySpan<byte> data) {
			this.data = data;
		}

		public byte Version => data[0];
		public int Flags => BinaryPrimitives.ReadInt32BigEndian(data) & 0x00FFFFFF;

		public uint EntryCount => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(4));
		public ReadOnlySpan<SampleItem> Samples {
			get {
				var samples = new SampleItem[EntryCount];

				for (int i = 0; i < samples.Length; i++) {
					samples[i] = new SampleItem {
						Count = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(i * 4 + 8, 4)),
						Offset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(i * 4 + 12, 4)),
					};
				}

				return samples;
			}
		}
	}
}
