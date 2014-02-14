using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class HeartRateRange {
        public HeartRateRange() { }
        public HeartRateRange(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {

        }
        public static HeartRateRange Parse(XmlElement elem) {
            return new HeartRateRange(elem);
        }
        //<xs:element name="resting" minOccurs="0">
        public byte? Restring;
        //<xs:element name="minimum" minOccurs="0">
        public byte? Minimum;
        //<xs:element name="average" minOccurs="0">
        public byte? Average;
        //<xs:element name="maximum" minOccurs="0">
        public byte? Maximum;
        //<xs:element name="ending" minOccurs="0">
        public byte? Ending;
    }
}
