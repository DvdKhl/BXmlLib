using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DocTypes.Ebml {
	[Flags]
	public enum EbmlElementType {
		Unknown = 0,
		Master = 1 << 0,
		Binary = 1 << 1,
		SInteger = 1 << 2,
		UInteger = 1 << 3,
		Float = 1 << 4,
		UTF8 = 1 << 6,
		ASCII = 1 << 7,
		Date = 1 << 8,

		Custom = 1 << 31,
	}
}
