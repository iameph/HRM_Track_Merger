using System;
using System.Collections.Generic;

namespace HRM_Track_Merger.GarminTCX {
    public class Lap {
        public DateTime StartTime { get; set; }
        public double TotalTimeSeconds { get; set; }
        public double DistanceMeters { get; set; }
        public double? MaximumSpeed { get; set; }
        public uint Calories { get; set; }
        public HeartRate AverageHeartRateBpm { get; set; }
        public HeartRate MaximumHeartRateBpm { get; set; }
        public string Intensity { get; set; }
        public byte? Cadence { get; set; }
        public string TriggerMethod { get; set; }
        public List<TrackPoint> Track { get; set; }
        public string Notes { get; set; }
        public LapExtension Extension { get; set; }

        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var lapElem = doc.CreateElement("Lap", xmlNS);
            lapElem.SetAttribute("StartTime", TCXFile.DateTimeToXmlString(StartTime));
            lapElem.AppendChild(doc.CreateElement("TotalTimeSeconds", xmlNS)).InnerXml = TotalTimeSeconds.ToString();;
            lapElem.AppendChild(doc.CreateElement("DistanceMeters", xmlNS));
            lapElem["DistanceMeters"].InnerXml = DistanceMeters.ToString();
            if (MaximumSpeed != null) {
                lapElem.AppendChild(doc.CreateElement("MaximumSpeed", xmlNS)).InnerXml = MaximumSpeed.ToString();;
            }
            lapElem.AppendChild(doc.CreateElement("Calories", xmlNS)).InnerXml = Calories.ToString();;
            if (AverageHeartRateBpm != null) {
                lapElem.AppendNotNullChild(AverageHeartRateBpm.GenerateXML(doc, "AverageHeartRateBpm"));
            }
            if (MaximumHeartRateBpm != null) {
                lapElem.AppendNotNullChild(MaximumHeartRateBpm.GenerateXML(doc, "MaximumHeartRateBpm"));
            }
            lapElem.AppendChild(doc.CreateElement("Intensity", xmlNS)).InnerXml = Intensity;;
            if (Cadence != null) {
                lapElem.AppendChild(doc.CreateElement("Cadence", xmlNS)).InnerXml = Cadence.ToString();;
            }
            lapElem.AppendChild(doc.CreateElement("TriggerMethod", xmlNS)).InnerXml = TriggerMethod;;
            if (Track != null) {
                lapElem.AppendChild(doc.CreateElement("Track", xmlNS));
                foreach (var trkPoint in Track) {
                    lapElem["Track"].AppendNotNullChild(trkPoint.GenerateXML(doc));
                }
            }
            if (Notes != null) {
                var note = Notes.Trim();
                if (note != "") {
                    lapElem.AppendChild(doc.CreateElement("Notes", xmlNS)).InnerXml = Notes.Trim();
                }
            }
            if (Extension != null) {
                lapElem.AppendNotNullChild(Extension.GenerateXML(doc));
            }
            return lapElem;
        }
    }
}
