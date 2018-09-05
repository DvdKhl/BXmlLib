using BXmlLib.DataSource;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DocType {
	public interface IBXmlDocType {
		object DecodeData(BXmlDocElement docElement, ReadOnlySpan<byte> encodedData);
		BXmlDocElement GetDocElement(ReadOnlySpan<byte> encodedIdentifier, ref BXmlElementHeader header, ReadOnlySpan<BXmlReader.PositionData> parents);
	}
}
