using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class PowerData {
        public PowerData() { }
        public PowerData(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {

        }
        public static PowerData Parse(XmlElement elem) {
            return new PowerData(elem);
        }
        //<xs:element name="power" type="short-range" minOccurs="0"/>
        public uint? Power;
        //<xs:element name="pedal-index" type="float-range" minOccurs="0"/>
        public Range<double> PedalIndex;
        //xs:element name="left-right-balance" minOccurs="0"> float
        public double? LeftRightBalance;
    }
}
