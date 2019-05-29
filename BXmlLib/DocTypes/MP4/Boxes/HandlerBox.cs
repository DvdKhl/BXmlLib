using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace BXmlLib.DocTypes.MP4.Boxes {
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


}
