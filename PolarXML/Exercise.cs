using System;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class Exercise {
        public Exercise() { }
        public Exercise(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            if (elem["sport"] != null) {
                Sport = elem["sport"].InnerXml;
            }
            if (elem["distance"]!=null) {
                Distance = Double.Parse(elem["distance"].InnerXml);
            }
            if (elem["calories"]!=null) {
                Calories = UInt32.Parse(elem["calories"].InnerXml);
            }
            if (elem["duration"]!=null) {
                Duration = TimeSpan.Parse(elem["duration"].InnerXml);
            }
            if (elem["user-settings"]!=null) {
                UserSettingsData = UserSettings.Parse(elem["user-settings"]);
            }
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
        public TimeSpan Duration;
        //<xs:element ref="user-settings" minOccurs="0"/>
        public UserSettings UserSettingsData;
        //-<xs:element name="moves" minOccurs="0">...</xs:element>
        //-<xs:element name="zones" minOccurs="0">...</xs:element>

    }
}
