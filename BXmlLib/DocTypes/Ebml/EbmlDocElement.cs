using BXmlLib.DocType;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DocTypes.Ebml {
	public class EbmlDocElement : BXmlDocElement {
		public EbmlElementType Type { get; private set; }
		public int Id { get; private set; }

		public EbmlDocElement(int id, EbmlElementType type, string name) : base(name, type == EbmlElementType.Master) {
			Type = type;
			Id = id;
		}

		public static EbmlDocElement Unknown { get; } = new EbmlDocElement(-1, EbmlElementType.Unknown, "Unknown");

        public override string ToString() => $"(Id={Id:X} Name={Name} Type={Type})";
    }
}
