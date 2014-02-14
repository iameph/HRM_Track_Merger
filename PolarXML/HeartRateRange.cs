using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class HeartRateRange : ShortRange,ICloneable {
        public HeartRateRange() { }
        public HeartRateRange(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            foreach (XmlElement el in elem.ChildNodes) {
                switch (el.LocalName) {
                    case "resting":
                        Restring = Byte.Parse(el.InnerXml);
                        break;
                    case "minimum":
                        Minimum = Byte.Parse(el.InnerXml);
                        break;
                    case "average":
                        Average= Byte.Parse(el.InnerXml);
                        break;
                    case "maximum":
                        Maximum= Byte.Parse(el.InnerXml);
                        break;
                    case "ending":
                        Ending = Byte.Parse(el.InnerXml);
                        break;
                    default:
                        break;
                }
            }
        }
        public static new HeartRateRange Parse(XmlElement elem) {
            return new HeartRateRange(elem);
        }
        //<xs:element name="resting" minOccurs="0">
        public byte? Restring;
        //<xs:element name="minimum" minOccurs="0">
        //public byte? Minimum;
        //<xs:element name="average" minOccurs="0">
        //public byte? Average;
        //<xs:element name="maximum" minOccurs="0">
        //public byte? Maximum;
        //<xs:element name="ending" minOccurs="0">
        public byte? Ending;

        public object Clone() {
            return base.MemberwiseClone();
        }
    }
}
