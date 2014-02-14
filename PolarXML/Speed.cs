using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class SpeedData {
        public SpeedData() { }
        public SpeedData(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {

        }
        public static SpeedData Parse(XmlElement elem) {
            return new SpeedData(elem);
        }
    //<xs:element name="type" type="speedType" minOccurs="0"/>
        public SpeedType? SpeedType;
    //<xs:element name="speed" type="float-range" minOccurs="0"/>
        public Range<double> Speed;
    //<xs:element name="cadence" type="short-range" minOccurs="0"/>
        public Range<uint> Cadence;
    }
}
