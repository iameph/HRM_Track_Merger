using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class Exercise {
        public Exercise() { }
        public Exercise(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {

        }
        public static Exercise Parse(XmlElement elem) {
            return new Exercise(elem);
        }
        //<xs:element name="sport" minOccurs="0">...</xs:element> string
        public string Sport;
        //<xs:element name="distance" minOccurs="0">...</xs:element> float, km
        public double? Distance;
        //<xs:element name="calories" minOccurs="0"> uint
        public uint Calories;
        //<xs:element name="duration" minOccurs="0"> string hh:mm:ss.xxx
        public DateTime Duration;
        //<xs:element ref="user-settings" minOccurs="0"/>
        public UserSettings UserSettingsData;
        //-<xs:element name="moves" minOccurs="0">...</xs:element>
        //-<xs:element name="zones" minOccurs="0">...</xs:element>

    }
}
