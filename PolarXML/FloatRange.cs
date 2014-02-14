using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class FloatRange {
        public FloatRange() { }
        public FloatRange(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            if (elem["minimum"] != null) {
                Minimum = Double.Parse(elem["minimum"].InnerXml);
            }
            if (elem["average"] != null) {
                Average = Double.Parse(elem["average"].InnerXml);
            }
            if (elem["maximum"] != null) {
                Maximum = Double.Parse(elem["maximum"].InnerXml);
            }
        }
        public static FloatRange Parse(XmlElement elem) {
            return new FloatRange(elem);
        }
        public double? Minimum;
        public double? Maximum;
        public double? Average;
    }
}
