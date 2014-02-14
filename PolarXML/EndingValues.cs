using System;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class EndingValues {
        public EndingValues() { }
        public EndingValues(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            if (elem["heart-rate"] != null) {
                HeartRate = Byte.Parse(elem["heart-rate"].InnerXml);
            }
            if (elem["speed"] != null) {
                Speed = Double.Parse(elem["speed"].InnerXml);
            }
            if (elem["cadence"] != null) {
                Cadence = Byte.Parse(elem["cadence"].InnerXml);
            }
        }
        public static EndingValues Parse(XmlElement elem) {
            return new EndingValues(elem);
        }
        //xs:element name="heart-rate" minOccurs="0">
        public byte? HeartRate;
        //<xs:element name="speed" type="xs:float" minOccurs="0"/>
        public double? Speed;
        //<xs:element name="cadence" minOccurs="0">
        public byte? Cadence;
    }
}
