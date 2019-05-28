using BXmlLib.DataSource;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BXmlLib.DocTypes.MP4 {
	public abstract class MP4BlockDataSource : BXmlDataBlockSource {

		public override ReadOnlySpan<byte> ReadIdentifier(ref BXmlElementHeader header) {
			if (IsEndOfStream) {
				header.Mutate(
					~BXmlElementHeader.UnexpectedEndOfStreamError,
					~BXmlElementHeader.SubsquentError,
					~BXmlElementHeader.SubsquentError
				);
				return ReadOnlySpan<byte>.Empty;
			}

			var headerPos = Position;
			var data = GetDataBlock(4 + 4 + 8 + 16);
			
			long size = BinaryPrimitives.ReadInt32BigEndian(data);
			var boxType = data.Slice(4, 4);

			var headerLength = 8;
			if (size == 1) {
				size = BinaryPrimitives.ReadInt64BigEndian(data.Slice(8));
				headerLength += 8;

			} else if(size == 0) {
				size = ~BXmlElementHeader.UnknownLength;
			}

			if (MemoryMarshal.Read<int>(boxType) == (('u' << 24) | ('u' << 16) | ('i' << 8) | ('d' << 0))) {
				boxType = data.Slice(headerLength, 16);
				headerLength += 16;
			}

			Advance(headerLength);

			header.Mutate(headerPos, headerPos + headerLength, size - headerLength);

			//var keyName = MP4DocType.KeyToString(boxType);

			return boxType;
		}
	}
}
