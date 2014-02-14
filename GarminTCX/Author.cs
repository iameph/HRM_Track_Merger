
namespace HRM_Track_Merger.GarminTCX {
    class Author {
        public string Name = "";
        public uint[] Version;
        public string LangID;
        public string PartNumber;

        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var elem = doc.CreateElement("Author", xmlNS);
            elem.AppendChild(doc.CreateElement("Name", xmlNS)).InnerXml = Name;
            if (Version != null && LangID != null && PartNumber != null) {
                var attr = doc.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
                attr.Value = "Application_t";
                elem.SetAttributeNode(attr);

                elem.AppendChild(doc.CreateElement("Build", xmlNS));
                elem["Build"].AppendChild(doc.CreateElement("Version", xmlNS));
                elem["Build"]["Version"].AppendChild(doc.CreateElement("VersionMajor", xmlNS));
                elem["Build"]["Version"].AppendChild(doc.CreateElement("VersionMinor", xmlNS));
                elem["Build"]["Version"]["VersionMajor"].InnerXml = (Version.Length > 0) ? Version[0].ToString() : "0";
                elem["Build"]["Version"]["VersionMinor"].InnerXml = (Version.Length > 1) ? Version[1].ToString() : "0";
                if (Version.Length > 2) {
                    elem["Build"]["Version"].AppendChild(doc.CreateElement("BuildMajor", xmlNS)).InnerXml = Version[2].ToString();
                }
                elem["Build"]["Version"].AppendChild(doc.CreateElement("BuildMinor", xmlNS)).InnerXml = Version[3].ToString(); ;
                elem.AppendChild(doc.CreateElement("LangID", xmlNS)).InnerXml = LangID;
                elem.AppendChild(doc.CreateElement("PartNumber", xmlNS)).InnerXml = PartNumber;
            }
            return elem;
        }
    }
}
