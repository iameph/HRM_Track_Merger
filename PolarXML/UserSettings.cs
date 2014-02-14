using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class UserSettings {
        public UserSettings() { }
        public UserSettings(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {

        }
        public static UserSettings Parse(XmlElement elem) {
            return new UserSettings(elem);
        }
        //<xs:element ref="heart-rate" minOccurs="0"/>
        public HeartRateRange HeartRate;
        //<xs:element name="vo2max" minOccurs="0"> byte
        public byte? VO2Max;
        //<xs:element name="weight" minOccurs="0"> float
        public double? Weight;
        //<xs:element name="height" minOccurs="0"> float
        public double Height;
    }
}
