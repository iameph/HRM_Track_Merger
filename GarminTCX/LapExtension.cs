using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class LapExtension {
        public const string LapExtensionNS = "http://www.garmin.com/xmlschemas/ActivityExtension/v2";
        public double? AvgSpeed;
        public double? MaxBikeCadence;
        public uint? AvgWatts;
        public uint? MaxWatts;
        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var elem = doc.CreateElement("Extensions", xmlNS);
            var elemLX = elem.AppendChild(doc.CreateElement("LX", LapExtensionNS));
            if (AvgSpeed != null) {
                elemLX.AppendChild(doc.CreateElement("AvgSpeed", LapExtensionNS)).InnerXml = AvgSpeed.ToString();
            }
            if (MaxBikeCadence != null) {
                elemLX.AppendChild(doc.CreateElement("MaxBikeCadence", LapExtensionNS)).InnerXml = MaxBikeCadence.ToString();
            }
            if (AvgWatts != null) {
                elemLX.AppendChild(doc.CreateElement("AvgWatts", LapExtensionNS)).InnerXml = AvgWatts.ToString();
            }
            if (MaxWatts != null) {
                elemLX.AppendChild(doc.CreateElement("MaxWatts", LapExtensionNS)).InnerXml = MaxWatts.ToString();
            }
            return elem;
        }
    }
}
