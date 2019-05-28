using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DocTypes.MP4 {
	public enum MP4Version { Unknown = 0, V1 = 1 }

	public class MP4DocMetaElement {
		public bool IsMandatory { get; }
		public bool Multiple { get; }
		public object DefaultValue { get; }
		public MP4DocElement[] ParentIds { get; }
		public Predicate<object> RangeCheck { get; }
		public MP4DocElement DocElement { get; }
		public string Description { get; }
		public MP4Version Versions { get; }

		public MP4DocMetaElement(string options, MP4DocElement docElement, object defaultValue, Predicate<object> rangeCheck, MP4DocElement[] parentIds, string description) {
			DocElement = docElement;
			IsMandatory = options.Contains("Ma");
			Multiple = options.Contains("Mu");
			DefaultValue = defaultValue;
			ParentIds = parentIds;
			RangeCheck = rangeCheck;
			Description = description;

			Versions = (options[0] == '1' ? MP4Version.V1 : 0);
		}

		internal bool CompatibleTo(MP4DocElement version) {
			throw new NotImplementedException();
		}
	}
}
