
namespace HRM_Track_Merger.GarminTCX {
    public class TrackPointExtension {
        public const string TrackPointExtensionNS = "http://www.garmin.com/xmlschemas/ActivityExtension/v2";
        public double? Speed { get; set; }
        public uint? Watts { get; set; }

        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var elem = doc.CreateElement("Extensions", xmlNS);
            var elemTPX = elem.AppendChild(doc.CreateElement("TPX", TrackPointExtensionNS));
            if (Speed != null) {
                elemTPX.AppendChild(doc.CreateElement("Speed", TrackPointExtensionNS)).InnerXml = Speed.ToString();
            }
            if (Watts != null) {
                elemTPX.AppendChild(doc.CreateElement("Watts", TrackPointExtensionNS)).InnerXml = Watts.ToString();
            }
            return elem;
        }
    }
}
