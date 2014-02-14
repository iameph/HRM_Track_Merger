using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class Position {
        public double Latitude;
        public double Longitude;
        public Position(double lat, double lon) {
            Latitude = lat;
            Longitude = lon;
        }
        public System.Xml.XmlNode GenerateXML(System.Xml.XmlDocument doc) {
            var xmlNS = doc.DocumentElement.NamespaceURI;
            var elem = doc.CreateElement("Position", xmlNS);
            elem.AppendChild(doc.CreateElement("LatitudeDegrees", xmlNS)).InnerXml = Latitude.ToString();
            elem.AppendChild(doc.CreateElement("LongitudeDegrees", xmlNS)).InnerXml = Longitude.ToString();
            return elem;
        }
    }
}
