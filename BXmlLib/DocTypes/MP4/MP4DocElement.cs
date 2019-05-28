using BXmlLib.DocType;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DocTypes.MP4 {
	public class MP4DocElement : BXmlDocElement {
		public MP4DocElement(string id, bool isContainer, string name) : base(name, isContainer) {
			Id = id ?? throw new ArgumentNullException(nameof(id));
		}

		public override string IdentifierString => Id;

		public override string TypeString => IsContainer ? "Container" : "Data";

		public MP4DocElement Parent { get; }
		public string Id { get; }
	}
}
