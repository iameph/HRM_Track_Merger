using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM_Track_Merger {
    public static class ClassExtensions {
        public static System.Xml.XmlNode AppendNotNullChild(this System.Xml.XmlElement parent, System.Xml.XmlNode child) {
            if (child == null)
                return null;
            parent.AppendChild(child);
            return child;
        }
    }
}
