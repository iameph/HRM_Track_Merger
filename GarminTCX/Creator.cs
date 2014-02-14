using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class Creator {
        public string Name;
        public uint UnitID;
        public uint ProductID;
        public uint[] Version;

        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var elem = doc.CreateElement("Creator", xmlNS);
            elem.SetAttributeNode("type", xmlNS).Prefix = "xsi";
            elem.AppendChild(doc.CreateElement("Name", xmlNS)).InnerXml = Name;
            elem.AppendChild(doc.CreateElement("UnitID", xmlNS)).InnerXml = UnitID.ToString();
            elem.AppendChild(doc.CreateElement("ProductID", xmlNS)).InnerXml = ProductID.ToString();
            elem.AppendChild(doc.CreateElement("Version", xmlNS));
            elem["Version"].AppendChild(doc.CreateElement("VersionMajor", xmlNS)).InnerXml = Version[0].ToString();
            elem["Version"].AppendChild(doc.CreateElement("VersionMinor", xmlNS)).InnerXml = Version[1].ToString();
            if (Version.Length > 2) {
                elem["Version"].AppendChild(doc.CreateElement("BuildMajor", xmlNS)).InnerXml = Version[2].ToString();
            }
            if (Version.Length > 3) {
                elem["Version"].AppendChild(doc.CreateElement("BuildMinor", xmlNS)).InnerXml = Version[3].ToString();
            }
            return elem;
        }
    }
}
