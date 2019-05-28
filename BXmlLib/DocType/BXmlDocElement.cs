using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DocType {
	public abstract class BXmlDocElement {
		public string Name { get; private set; }
		public bool IsContainer { get; private set; }

		public BXmlDocElement(string name, bool isContainer) {
			Name = name;
			IsContainer = isContainer;
		}

        public abstract string IdentifierString { get; }
        public abstract string TypeString { get; }
    }
}
