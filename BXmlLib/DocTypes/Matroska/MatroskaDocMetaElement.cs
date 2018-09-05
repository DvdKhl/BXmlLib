//Mod. BSD License (See LICENSE file) DvdKhl (DvdKhl@web.de)
using System;
using System.Collections.Generic;
using System.Text;

namespace BXmlLib.DocTypes.Matroska {
    public enum MatroskaVersion { Unknown = 0, V1 = 1, V2 = 2, V3 = 4, V4 = 8, WebM = 1024 }

    public class MatroskaDocMetaElement {
        public bool IsMandatory { get; }
        public bool Multiple { get; }
        public object DefaultValue { get; }
        public int[] ParentIds { get; }
        public Predicate<object> RangeCheck { get; }
        public MatroskaDocElement DocElement { get; }
        public string Description { get; }
        public MatroskaVersion Versions { get; }

        public MatroskaDocMetaElement(string options, MatroskaDocElement docElement, object defaultValue, Predicate<object> rangeCheck, int[] parentIds, string description) {
            DocElement = docElement;
            IsMandatory = options[5] == 'M' && options[6] == 'a';
            Multiple = options[7] == 'M' && options[8] == 'u';
            DefaultValue = defaultValue;
            ParentIds = parentIds;
            RangeCheck = rangeCheck;
            Description = description;

            Versions =
                (options[0] == '1' ? MatroskaVersion.V1 : 0) |
                (options[1] == '2' ? MatroskaVersion.V2 : 0) |
                (options[2] == '3' ? MatroskaVersion.V3 : 0) |
                (options[3] == '4' ? MatroskaVersion.V4 : 0) |
                (options[4] == 'W' ? MatroskaVersion.WebM : 0);
        }

        internal bool CompatibleTo(MatroskaVersion version) {
            throw new NotImplementedException();
        }
    }
}
