using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class EndingValues {
        public EndingValues() { }
        public EndingValues(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {

        }
        public static EndingValues Parse(XmlElement elem) {
            return new EndingValues(elem);
        }
        //xs:element name="heart-rate" minOccurs="0">
        public byte? HeartRate;
        //<xs:element name="speed" type="xs:float" minOccurs="0"/>
        public float? Speed;
        //<xs:element name="cadence" minOccurs="0">
        public byte? Cadence;
    }
}
