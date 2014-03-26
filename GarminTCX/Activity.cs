using System;
using System.Collections.Generic;

namespace HRM_Track_Merger.GarminTCX {
    public class Activity {
        public Sport Sport { get; set; }
        public DateTime Id { get; set; }
        public List<Lap> Laps { get; set; }
        public string Notes { get; set; }
        public Creator Creator { get; set; }
        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var actElem = doc.CreateElement("Activity", xmlNS);
            actElem.SetAttribute("Sport", Sport.ToString());
            actElem.AppendChild(doc.CreateElement("Id", xmlNS)).InnerXml = TCXFile.DateTimeToXmlString(Id);
            foreach (var lap in Laps) {
                actElem.AppendNotNullChild(lap.GenerateXML(doc));
            }
            if (Notes != null) {
                var note = Notes.Trim();
                if (note != "") {
                    actElem.AppendChild(doc.CreateElement("Notes", xmlNS)).InnerXml = Notes.Trim();
                }
            }
            if (Creator != null) {
                actElem.AppendNotNullChild(Creator.GenerateXML(doc));
            }
            return actElem;
        }
        public Activity() {
            Sport = Sport.Other;
        }
    }
}
