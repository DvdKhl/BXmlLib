using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DocType {
	public class BXmlDocElement {
		public string Name { get; private set; }
		public bool IsContainer { get; private set; }

		public BXmlDocElement(string name, bool isContainer) {
			Name = name;
			IsContainer = isContainer;
		}
	}
}
