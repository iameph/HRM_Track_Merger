using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class Activity {
        public Sport Sport = Sport.Other;
        public DateTime Id;
        public List<Lap> Laps;
        public string Notes;
        public Creator Creator;
        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var actElem = doc.CreateElement("Activity", xmlNS);
            actElem.SetAttribute("Sport", Sport.ToString());
            actElem.AppendChild(doc.CreateElement("Id", xmlNS)).InnerXml = TCXFile.DateTimeToXmlString(Id);
            foreach (var lap in Laps) {
                actElem.AppendChild(lap.GenerateXML(doc));
            }
            if (Notes != null) {
                var note = Notes.Trim();
                if (note != "") {
                    actElem.AppendChild(doc.CreateElement("Notes", xmlNS)).InnerXml = Notes.Trim();
                }
            }
            if (Creator != null) {
                actElem.AppendChild(Creator.GenerateXML(doc));
            }
            return actElem;
        }
    }
}
