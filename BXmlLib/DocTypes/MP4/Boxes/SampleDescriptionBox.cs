using System;
using System.Buffers.Binary;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public ref struct SampleDescriptionBox {
		private readonly ReadOnlySpan<byte> data;
		public SampleDescriptionBox(ReadOnlySpan<byte> data) {
			this.data = data;
		}

		public byte Version => data[0];
		public int Flags => BinaryPrimitives.ReadInt32BigEndian(data) & 0x00FFFFFF;

		public uint EntryCount => BinaryPrimitives.ReadUInt32BigEndian(data.Slice(4));


		private ReadOnlySpan<byte> GetEntry(int index) {
			if (index >= EntryCount) {
				throw new Exception();
			}

			int size;
			int offset = 8;
			for (int i = 0; i < index; i++) {
				size = BinaryPrimitives.ReadInt32BigEndian(data.Slice(offset));
				offset += size;
			}
			size = BinaryPrimitives.ReadInt32BigEndian(data.Slice(offset));

			return data.Slice(offset + 4, size - 4);
		}

		public VisualSampleEntry GetVideoEntry(int index) {
			return new VisualSampleEntry(GetEntry(index));
		}
		public AudioSampleEntry GetAudioEntry(int index) {
			return new AudioSampleEntry(GetEntry(index));
		}
		public HintSampleEntry GetHintEntry(int index) {
			return new HintSampleEntry(GetEntry(index));
		}
	}
}
