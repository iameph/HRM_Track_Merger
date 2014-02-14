using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class Sample {
        public Sample() { }
        public Sample(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            foreach (XmlElement el in elem.ChildNodes) {
                switch (el.LocalName) {
                    case "sampleType":
                        Enum.TryParse<SampleType>(el.InnerXml,out SampleType);
                        break;
                    case "values":
                        var valStrings = new List<string>(el.InnerXml.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                        Values = valStrings.ConvertAll(str => Double.Parse(str));
                        break;
                    default:
                        break;
                }
            }
        }
        public static Sample Parse(XmlElement elem) {
            return new Sample(elem);
        }
        //<xs:element name="type" type="sampleType"/>
        public SampleType SampleType;
        //<xs:element name="values" type="xs:string"/>
        public List<double> Values;
    }
}
