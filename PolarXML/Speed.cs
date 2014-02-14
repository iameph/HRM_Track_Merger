using System;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class SpeedData {
        public SpeedData() { }
        public SpeedData(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            foreach (XmlElement el in elem) {
                switch (el.LocalName) {
                    case "type":
                        SpeedType st;
                        if (Enum.TryParse(el.InnerXml, out st)) {
                            SpeedType = st;
                        }
                        break;
                    case "speed":
                        Speed = FloatRange.Parse(el);
                        break;
                    case "cadence":
                        Cadence = ShortRange.Parse(el);
                        break;
                    default:
                        break;
                }
            }
        }
        public static SpeedData Parse(XmlElement elem) {
            return new SpeedData(elem);
        }
        //<xs:element name="type" type="speedType" minOccurs="0"/>
        public SpeedType? SpeedType;
        //<xs:element name="speed" type="float-range" minOccurs="0"/>
        public FloatRange Speed;
        //<xs:element name="cadence" type="short-range" minOccurs="0"/>
        public ShortRange Cadence;
    }
}
