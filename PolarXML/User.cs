using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class User : UserSettings {
        public User() { }
        public User(XmlElement elem)
            : base(elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            foreach (XmlElement el in elem) {
                switch (el.LocalName) {
                    case "email":
                        Email = el.InnerXml;
                        break;
                    case "nickname":
                        NickName = el.InnerXml;
                        break;
                    default:
                        break;
                }
            }
        }
        public static new User Parse(XmlElement elem) {
            return new User(elem);
        }
        //<xs:element name="email" type="xs:string" minOccurs="0"/>
        public string Email;
        //<xs:element name="nickname" type="xs:string" minOccurs="0"/>
        public string NickName;
    }
}
