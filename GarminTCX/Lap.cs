using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class Lap {
        public DateTime StartTime;
        public double TotalTimeSeconds;
        public double DistanceMeters;
        public double? MaximumSpeed;
        public uint Calories;
        public HeartRate AverageHeartRateBpm;
        public HeartRate MaximumHeartRateBpm;
        public string Intensity;
        public byte? Cadence;
        public string TriggerMethod;
        public List<TrackPoint> Track;
        public string Notes;
        public LapExtension Extension;
        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var lapElem = doc.CreateElement("Lap", xmlNS);
            lapElem.SetAttribute("StartTime", TCXFile.DateTimeToXmlString(StartTime));
            lapElem.AppendChild(doc.CreateElement("TotalTimeSeconds", xmlNS));
            lapElem["TotalTimeSeconds"].InnerXml = TotalTimeSeconds.ToString();
            lapElem.AppendChild(doc.CreateElement("DistanceMeters", xmlNS));
            lapElem["DistanceMeters"].InnerXml = DistanceMeters.ToString();
            if (MaximumSpeed != null) {
                lapElem.AppendChild(doc.CreateElement("MaximumSpeed", xmlNS));
                lapElem["MaximumSpeed"].InnerXml = MaximumSpeed.ToString();
            }
            lapElem.AppendChild(doc.CreateElement("Calories", xmlNS));
            lapElem["Calories"].InnerXml = Calories.ToString();
            if (AverageHeartRateBpm != null) {
                lapElem.AppendChild(AverageHeartRateBpm.GenerateXML(doc, "AverageHeartRateBpm"));
            }
            if (MaximumHeartRateBpm != null) {
                lapElem.AppendChild(MaximumHeartRateBpm.GenerateXML(doc, "MaximumHeartRateBpm"));
            }
            lapElem.AppendChild(doc.CreateElement("Intensity", xmlNS));
            lapElem["Intensity"].InnerXml = Intensity;
            if (Cadence != null) {
                lapElem.AppendChild(doc.CreateElement("Cadence", xmlNS));
                lapElem["Cadence"].InnerXml = Cadence.ToString();
            }
            lapElem.AppendChild(doc.CreateElement("TriggerMethod", xmlNS));
            lapElem["TriggerMethod"].InnerXml = TriggerMethod;
            if (Track != null) {
                lapElem.AppendChild(doc.CreateElement("Track", xmlNS));
                foreach (var trkPoint in Track) {
                    lapElem["Track"].AppendChild(trkPoint.GenerateXML(doc));
                }
            }
            if (Notes != null) {
                lapElem.AppendChild(doc.CreateElement("Notes", xmlNS));
                lapElem["Notes"].InnerXml = Notes;
            }
            if (Extension != null) {
                lapElem.AppendChild(Extension.GenerateXML(doc));
            }
            return lapElem;
        }
    }
}
