using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class AltitudeInfo {
        public AltitudeInfo() { }
        public AltitudeInfo(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            if (elem["vertical-speed-up"] != null) {
                VerticalSpeedUp = FloatRange.Parse(elem["vertical-speed-up"]);
            }
            if (elem["vertical-speed-down"] != null) {
                VerticalSpeedDown = FloatRange.Parse(elem["vertical-speed-down"]);
            }
            if (elem["ascent"] != null) {
                Ascent = Double.Parse(elem["ascent"].InnerXml);
            }
            if (elem["descent"] != null) {
                Descent = Double.Parse(elem["descent"].InnerXml);
            }
        }
        public static AltitudeInfo Parse(XmlElement elem) {
            return new AltitudeInfo(elem);
        }
        //<xs:element name="vertical-speed-up" type="float-range" minOccurs="0"/>
        public FloatRange VerticalSpeedUp;
        //<xs:element name="vertical-speed-down" type="float-range" minOccurs="0"/>
        public FloatRange VerticalSpeedDown;
        //<xs:element name="ascent" type="xs:float" minOccurs="0"/>
        public double? Ascent;
        //<xs:element name="descent" type="xs:float" minOccurs="0"/>
        public double? Descent;
    }
}
