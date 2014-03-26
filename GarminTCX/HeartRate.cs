
namespace HRM_Track_Merger.GarminTCX {
    public class HeartRate {
        public byte Value { get; set; }
        public HeartRate(byte val) {
            Value = val;
        }
        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc, string name) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var elem = doc.CreateElement(name, xmlNS);
            elem.AppendChild(doc.CreateElement("Value", xmlNS)).InnerXml = Value.ToString();
            return elem;
        }
        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            return GenerateXML(doc, "HeartRateBpm");
        }
    }
}
