using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BXmlLib.DocTypes.MP4.Boxes {
	public ref struct FileTypeBox {
		private ReadOnlySpan<byte> Data { get; }

		public int MajorBrands => BinaryPrimitives.ReadInt32BigEndian(Data) & 0x00FFFFFF;
		public int MinorVersion => BinaryPrimitives.ReadInt32BigEndian(Data) & 0x00FFFFFF;
		public ReadOnlySpan<int> CompatibleBrands => MemoryMarshal.Cast<byte, int>(Data.Slice(8)); //TODO BigEndian

		public FileTypeBox(ReadOnlySpan<byte> data) {
			Data = data;
		}
	}

	public ref struct MovieHeaderBox {
		private ReadOnlySpan<byte> Data { get; }
		public MovieHeaderBox(ReadOnlySpan<byte> data) {
			Data = data;
		}
		public byte Version => Data[0];
		public int Flags => BinaryPrimitives.ReadInt32BigEndian(Data) & 0x00FFFFFF;

		public ulong CreationTime => Version == 1 ? BinaryPrimitives.ReadUInt64BigEndian(Data.Slice(4)) : BinaryPrimitives.ReadUInt32BigEndian(Data.Slice(4));
		public ulong ModificationTime => Version == 1 ? BinaryPrimitives.ReadUInt64BigEndian(Data.Slice(12)) : BinaryPrimitives.ReadUInt32BigEndian(Data.Slice(8));
		public uint Timescale => BinaryPrimitives.ReadUInt32BigEndian(Data.Slice(Version == 1 ? 20 : 12));
		public ulong Duration => Version == 1 ? BinaryPrimitives.ReadUInt64BigEndian(Data.Slice(24)) : BinaryPrimitives.ReadUInt32BigEndian(Data.Slice(16));
		public float Rate => BinaryPrimitives.ReadUInt32BigEndian(Data.Slice(Version == 1 ? 32 : 20)) / (float)0x00010000;
		public float Volume => BinaryPrimitives.ReadUInt16BigEndian(Data.Slice(Version == 1 ? 36 : 24)) / (float)0x0100;
		public ushort Reserved1 => BinaryPrimitives.ReadUInt16BigEndian(Data.Slice(Version == 1 ? 38 : 26));
		public ulong Reserved2 => BinaryPrimitives.ReadUInt64BigEndian(Data.Slice(Version == 1 ? 40 : 28));

		public ReadOnlySpan<int> Matrix => MemoryMarshal.Cast<byte, int>(Data.Slice(Version == 1 ? 48 : 36, 4 * 9)); //TODO BigEndian
		public ReadOnlySpan<int> PreDefined => MemoryMarshal.Cast<byte, int>(Data.Slice(Version == 1 ? 84 : 72, 4 * 6)); //TODO BigEndian
		public uint NextTrack => BinaryPrimitives.ReadUInt32BigEndian(Data.Slice(Version == 1 ? 108 : 96));
	}

	public ref struct HandlerBox {
		private ReadOnlySpan<byte> Data { get; }
		public HandlerBox(ReadOnlySpan<byte> data) {
			Data = data;
		}

		public byte Version => Data[0];
		public int Flags => BinaryPrimitives.ReadInt32BigEndian(Data) & 0x00FFFFFF;

		public uint PreDefined => BinaryPrimitives.ReadUInt32BigEndian(Data.Slice(4));
		public ReadOnlySpan<byte> HandlerType => Data.Slice(8, 4);

		public ReadOnlySpan<int> Reserved => MemoryMarshal.Cast<byte, int>(Data.Slice(12));

	}


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
	public ref struct VisualSampleEntry {
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
		public ushort PreDefined2 => BinaryPrimitives.ReadUInt16BigEndian(data.Slice(80));
	}
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

			return data.Slice(offset + 4, size- 4);
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
