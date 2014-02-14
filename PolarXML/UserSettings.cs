using System;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class UserSettings {
        public UserSettings() { }
        public UserSettings(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            foreach (XmlElement el in elem) {
                switch (el.LocalName) {
                    case "heart-rate":
                        HeartRate = HeartRateRange.Parse(el);
                        break;
                    case "vo2max":
                        VO2Max = Byte.Parse(el.InnerXml);
                        break;
                    case "weight":
                        Weight = Double.Parse(el.InnerXml);
                        break;
                    case "height":
                        Height = Double.Parse(el.InnerXml);
                        break;
                    default:
                        break;
                }
            }
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
        public double? Height;
    }
}
