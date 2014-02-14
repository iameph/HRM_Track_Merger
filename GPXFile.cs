using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace HRM_Track_Merger {
    class GPXFile {
        private XmlElement _currSegment;
        private bool isPolarStupidGPX = false;
        private string nameSpace = "";
        private string defaultXML = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>" +
            "<gpx version=\"1.0\" creator=\"Polar ProTrainer 5 - www.polar.fi\" " +
            "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.topografix.com/GPX/1/0\" " +
            "xsi:schemaLocation=\"http://www.topografix.com/GPX/1/0 http://www.topografix.com/GPX/1/0/gpx.xsd\">" +
            "<trk></trk></gpx>";
        public GPXFile(string filename) {
            File = new XmlDocument();
            File.Load(filename);
            nameSpace = File.DocumentElement.NamespaceURI;
            if (File.DocumentElement.GetAttribute("version", nameSpace) == "1.0" &&
                File.DocumentElement.GetAttribute("creator", nameSpace) == "Polar ProTrainer 5 - www.polar.fi" &&
                File.DocumentElement.GetAttribute("xmlns", nameSpace) == "http://www.topografix.com/GPX/1/0") {
                    isPolarStupidGPX = true;
            }
            _currSegment = null;
        }
        public GPXFile() {
            File = new XmlDocument();
            File.LoadXml(defaultXML);
            isPolarStupidGPX = true;
            _currSegment = null;
        }
        public XmlDocument File { get; private set; }
        public List<TrackPoint> GetTrackPoints(TimeSpan offset) {
            var segmentElements = File.DocumentElement["trk"].GetElementsByTagName("trkseg");
            List<TrackPoint> trackPoints = new List<TrackPoint>();
            for (int i = 0; i < segmentElements.Count; ++i) {
                var segm = (XmlElement)segmentElements[i];
                trackPoints.AddRange(GetTrackPointsFromSegment(segm.GetElementsByTagName("trkpt"), offset));

            }
            return trackPoints;
        }
        public List<TrackPoint> GetTrackPoints() {
            return GetTrackPoints(TimeSpan.Zero);
        }
        public void RemoveTrackPoints() {
            var segmentElements = File.DocumentElement["trk"].GetElementsByTagName("trkseg");
            for (int i = 0; i < segmentElements.Count; ++i) {
                var segm = (XmlElement)segmentElements[i];
                segm.ParentNode.RemoveChild(segm);
            }
        }
        private IEnumerable<TrackPoint> GetTrackPointsFromSegment(XmlNodeList XList, TimeSpan offset) {
            List<TrackPoint> list = new List<TrackPoint>();
            foreach (XmlNode node in XList) {
                TrackPoint point = new TrackPoint();
                XmlElement time = node["time"];
                XmlAttribute lon = (XmlAttribute)node.Attributes["lon"];
                XmlAttribute lat = (XmlAttribute)node.Attributes["lat"];
                XmlElement elevationElement = node["ele"];
                if (time == null || lon == null || lat == null) continue;
                IFormatProvider format = CultureInfo.InvariantCulture.NumberFormat;
                point.latitude = Double.Parse(lat.Value, format);
                point.longitude = Double.Parse(lon.Value, format);
                point.time = DateTime.Parse(time.InnerText);
                if (isPolarStupidGPX) {
                    point.time -= TimeZoneInfo.Local.BaseUtcOffset;
                }
                point.time += offset;
                if (elevationElement != null) {
                    point.elevation = Double.Parse(elevationElement.InnerText, format);
                }
                list.Add(point);
            }
            return list;
        }
        public XmlElement CreateNewSegment() {
            var segmEl = File.CreateElement("trkseg", File.DocumentElement["trk"].NamespaceURI);
            segmEl.RemoveAllAttributes();
            File.DocumentElement["trk"].AppendChild(segmEl);
            _currSegment = segmEl;
            return segmEl;
        }
        public void AddTrackPoint(TrackPoint point){
            if (_currSegment == null) {
                CreateNewSegment();
            }
            var doc = _currSegment.OwnerDocument;
            var trk = doc.CreateElement("trkpt", _currSegment.NamespaceURI);
            var lat = doc.CreateAttribute("lat");
            var lon = doc.CreateAttribute("lon");
            var time = doc.CreateElement("time", _currSegment.NamespaceURI);
            lat.Value = XmlConvert.ToString(point.latitude);
            lon.Value = XmlConvert.ToString(point.longitude);
            var xmlTime = point.time.ToUniversalTime();
            if (isPolarStupidGPX) {
                xmlTime += TimeZoneInfo.Local.BaseUtcOffset;
            }
            time.InnerText = String.Format("{0}-{1:D2}-{2:D2}T{3:D2}:{4:D2}:{5:D2}Z", xmlTime.Year, xmlTime.Month, xmlTime.Day, xmlTime.Hour, xmlTime.Minute, xmlTime.Second);
            trk.Attributes.Append(lat);
            trk.Attributes.Append(lon);
            trk.AppendChild(time);
            _currSegment.AppendChild(trk);
        }
        public void RemoveMetadata() {
            File.DocumentElement.RemoveChild(File.DocumentElement["metadata"]);
            var trkElem = File.DocumentElement["trk"];
            if (trkElem!=null) {
                trkElem.RemoveChild(trkElem["name"]);
                trkElem.RemoveChild(trkElem["desc"]);
            }
        }
    }
}
