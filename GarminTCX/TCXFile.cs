using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.GarminTCX {
    class TCXFile {
        private string defaultFile = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
            + "<TrainingCenterDatabase xsi:schemaLocation=\"http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2 http://www.garmin.com/xmlschemas/TrainingCenterDatabasev2.xsd\" "
            + "xmlns:ns5=\"http://www.garmin.com/xmlschemas/ActivityGoals/v1\" xmlns:ns3=\"http://www.garmin.com/xmlschemas/ActivityExtension/v2\"  "
            + "xmlns:ns2=\"http://www.garmin.com/xmlschemas/UserProfile/v2\" xmlns=\"http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2\" "
            + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:ns4=\"http://www.garmin.com/xmlschemas/ProfileExtension/v1\"></TrainingCenterDatabase>";
        private string xmlNS = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2";
        private XmlDocument doc;
        public TCXFile() {
            Activities = new List<Activity>();
        }
        public void Save(string fileName) {
            Save(fileName, new XmlWriterSettings() { 
                Indent=true
            });
        }
        public void Save(string fileName, XmlWriterSettings ws) {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            doc = new XmlDocument();
            doc.LoadXml(defaultFile);
            doc.DocumentElement.AppendChild(doc.CreateElement("Activities", xmlNS));
            foreach (var act in Activities) {
                doc.DocumentElement["Activities"].AppendChild(act.GenerateXML(doc));
            }
            if (Author != null) {
                doc.DocumentElement.AppendChild(author.GenerateXML(doc));
            }
            using (var writer = XmlWriter.Create(fileName, ws)) {
                doc.Save(writer);
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
        }
        public static string DateTimeToXmlString(DateTime time) {
            return time.ToUniversalTime().ToString("s") + String.Format(".{0,3:D3}", time.Millisecond) + "Z";
        }
        private List<Activity> activities;
        public List<Activity> Activities { get { return activities; } protected set { activities = value; } }
        private Author author;
        public Author Author { get { return author; } set { author = value; } }
        public void SetSport(Sport sport, int idx) {
            if (Activities != null && Activities.Count > idx) {
                Activities[idx].Sport = sport;
            }
        }
        public void SetSport(Sport sport) {
            foreach (var act in Activities) {
                act.Sport = sport;
            }
        }
        public void SetCreator(Creator creator) {
            foreach (var act in Activities) {
                act.Creator = (Creator)creator.Clone();
            }
        }
    }
}
