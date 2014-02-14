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
        public Range<double> GetNotNullableRange() {
            List<double> values = new List<double>();
            if (Minimum.HasValue) {
                values.Add(Minimum.Value);
            }
            if (Average.HasValue) {
                values.Add(Average.Value);
            }
            if (Maximum.HasValue) {
                values.Add(Maximum.Value);
            }
            if (values.Count == 0) {
                return new Range<double>(0, 0, 0);
            }
            return new Range<double>(
                values.Min(),
                Average.HasValue ? Average.Value : (values.Min() + values.Max()) / 2,
                values.Max()
                );
        }
    }
}
