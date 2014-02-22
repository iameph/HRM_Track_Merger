using System;

namespace HRM_Track_Merger.GarminTCX {
    class TrackPoint {

        public DateTime Time;
        public Position Position;
        public double? AltitudeMeters;
        public double? DistanceMeters;
        public HeartRate HeartRateBpm;
        public byte? Cadence;
        public TrackPointExtension Extension;

        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var elem = doc.CreateElement("Trackpoint", xmlNS);
            elem.AppendChild(doc.CreateElement("Time", xmlNS)).InnerXml = TCXFile.DateTimeToXmlString(Time);
            if (Position != null) {
                elem.AppendNotNullChild(Position.GenerateXML(doc));
            }
            if (AltitudeMeters != null) {
                elem.AppendChild(doc.CreateElement("AltitudeMeters", xmlNS)).InnerXml = AltitudeMeters.ToString();
            }
            if (DistanceMeters != null) {
                elem.AppendChild(doc.CreateElement("DistanceMeters", xmlNS)).InnerXml = DistanceMeters.ToString();
            }
            if (HeartRateBpm != null) {
                elem.AppendNotNullChild(HeartRateBpm.GenerateXML(doc, "HeartRateBpm"));
            }
            if (Cadence != null) {
                elem.AppendChild(doc.CreateElement("Cadence", xmlNS)).InnerXml = Cadence.ToString();
            }
            if (Extension != null) {
                elem.AppendNotNullChild(Extension.GenerateXML(doc));
            }
            return elem;
        }
    }
}
