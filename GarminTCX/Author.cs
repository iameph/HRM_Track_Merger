using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class Author {
        public string Name = "";
        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var elem = doc.CreateElement("Author", xmlNS);
            elem.AppendChild(doc.CreateElement("Name", xmlNS)).InnerXml = Name;
            return elem;
        }
    }
}
