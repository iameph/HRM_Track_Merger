using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class ShortRange {
        public ShortRange() { }
        public ShortRange(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            if (elem["minimum"] != null) {
                Minimum = Byte.Parse(elem["minimum"].InnerXml);
            }
            if (elem["average"] != null) {
                Average = Byte.Parse(elem["average"].InnerXml);
            }
            if (elem["maximum"] != null) {
                Maximum = Byte.Parse(elem["maximum"].InnerXml);
            }
        }
        public static ShortRange Parse(XmlElement elem) {
            return new ShortRange(elem);
        }
        public byte? Minimum { get; set; }
        public byte? Maximum { get; set; }
        public byte? Average { get; set; }

        public Range<byte> GetNotNullableRange() {
            List<byte> values = new List<byte>();
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
                return new Range<byte>(0, 0, 0);
            }
            return new Range<byte>(
                values.Min(),
                (byte)(Average.HasValue ? Average.Value : (values.Min() + values.Max()) / 2),
                values.Max()
                );
        }
    }
}
