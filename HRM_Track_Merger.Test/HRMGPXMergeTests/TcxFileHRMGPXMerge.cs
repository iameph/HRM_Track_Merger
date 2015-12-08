using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test.HRMGPXMergeTests {
    using System.Xml;
    using HRM_Track_Merger.GarminTCX;
    using System.IO;
    [TestFixture]
    [SetCulture("")]
    class TcxFileHRMGPXMergeTest {
        [Test]
        public void SavesTCXFile() {
            Assert.That(File.Exists(@"sample.tcx"), Is.True);
        }
        [Test]
        public void TCXFileNotEmpty() {
            var fileInfo = new FileInfo(@"sample.tcx");
            Assert.That(fileInfo.Length, Is.GreaterThan(0));
        }
        [Test]
        public void SavesExerciseInfo() {
            var doc = new System.Xml.XmlDocument();
            doc.Load(@"sample.tcx");
            Assert.That(doc.DocumentElement.LocalName, Is.EqualTo("TrainingCenterDatabase"));
            Assert.That(doc.DocumentElement.HasChildNodes, Is.True);
            Assert.That(doc.DocumentElement["Activities"], Is.Not.Null);
            Assert.That(doc.DocumentElement["Activities"].HasChildNodes, Is.True, "Activities element has childs");
            Assert.That(doc.DocumentElement["Activities"]["Activity"], Is.Not.Null);

        }
        [Test]
        public void ActivityHasSportAttribute() {
            var doc = new System.Xml.XmlDocument();
            doc.Load(@"sample.tcx");
            Assert.That(doc.DocumentElement["Activities"]["Activity"].GetAttribute("Sport"), Is.EqualTo("Other"));
        }
        [Test]
        public void ActivityHasIdChild() {
            var doc = new System.Xml.XmlDocument();
            doc.Load(@"sample.tcx");
            Assert.That(doc.DocumentElement["Activities"]["Activity"]["Id"], Is.Not.Null);
            Assert.That(doc.DocumentElement["Activities"]["Activity"]["Id"].InnerXml, Is.StringContaining("2014-01-17T06:16:32.000Z"));
        }
        [Test]
        public void LapCheckStartTime() {
            var lap = GetLap();
            Assert.That(lap.GetAttribute("StartTime"),
                Is.StringContaining("2014-01-17T06:16:32.000Z"), "StartTime correct");
        }
        [Test]
        public void LapCheckTotalTimeSeconds() {
            var lap = GetLap();
            Assert.That(Double.Parse(lap["TotalTimeSeconds"].InnerXml), Is.EqualTo(643.9).Within(1).Percent, "TotalTimeSeconds correct");
        }
        [Test]
        public void LapCheckDistanceMetersDistanceMeters() {
            var lap = GetLap();
            Assert.That(Double.Parse(lap["DistanceMeters"].InnerXml), Is.EqualTo(2872.92).Within(1).Percent, "DistanceMeters correct");
        }
        [Test]
        public void LapCheckMaximumSpeed() {
            var lap = GetLap();
            Assert.That(Double.Parse(lap["MaximumSpeed"].InnerXml), Is.EqualTo(6.22).Within(1).Percent, "MaximumSpeed correct");
        }
        [Test]
        public void LapCheckCalories() {
            var lap = GetLap();
            Assert.That(Int32.Parse(lap["Calories"].InnerXml), Is.EqualTo(0).Within(1).Percent, "Calories correct");
        }
        [Test]
        public void LapCheckAverageHeartRateBpm() {
            var lap = GetLap();
            Assert.That(Int32.Parse(lap["AverageHeartRateBpm"]["Value"].InnerXml), Is.EqualTo(144).Within(1).Percent, "AverageHearRateBpm correct");
        }
        [Test]
        public void LapCheckIntensity() {
            var lap = GetLap();
            Assert.That(lap["Intensity"].InnerXml, Is.EqualTo("Active"), "Intensity correct");
        }
        [Test]
        public void LapCheckCadence() {
            var lap = GetLap();
            Assert.That(Int32.Parse(lap["Cadence"].InnerXml), Is.EqualTo(64).Within(1).Percent, "Cadence correct");
        }
        [Test]
        public void LapCheckTriggerMethod() {
            var lap = GetLap();
            Assert.That(lap["TriggerMethod"].InnerXml, Is.EqualTo("Manual"), "TriggerMethod correct");
        }
        [Test]
        public void LapCheckTrack() {
            var lap = GetLap();
            Assert.That(lap["Track"].HasChildNodes, Is.True, "Track has childs");
        }
        [Test]
        public void LapCheckMaxBikeCadence() {
            var lap = GetLap();
            Assert.That(Int32.Parse(lap["Extensions"]["LX"]["MaxBikeCadence"].InnerXml), Is.EqualTo(86).Within(1).Percent, "MaxBikeCadence correct");
        }
        [Test]
        public void LapCheckAvgSpeed() {
            var lap = GetLap();
            Assert.That(Double.Parse(lap["Extensions"]["LX"]["AvgSpeed"].InnerXml), Is.EqualTo(4.61).Within(1).Percent, "AvgSpeed correct");
        }
        private static XmlElement GetLap() {
            var doc = new System.Xml.XmlDocument();
            doc.Load(@"sample.tcx");

            var lap = doc.DocumentElement["Activities"]["Activity"]["Lap"];
            return lap;
        }

        
        private static XmlElement GetTrackPoint() {
            var doc = new System.Xml.XmlDocument();
            doc.Load(@"sample.tcx");
            var point = doc.DocumentElement["Activities"]["Activity"]["Lap"]["Track"]["Trackpoint"];
            return point;
        }
        [Test]
        public void TrackPointCheckTime() {
            var point = GetTrackPoint();
            Assert.That(point["Time"].InnerXml, Is.EqualTo("2014-01-17T06:16:32.000Z"));
        }
        [Test]
        public void TrackPointCheckLatitudeDegrees() {
            var point = GetTrackPoint();
            Assert.That(Double.Parse(point["Position"]["LatitudeDegrees"].InnerXml), Is.EqualTo(56.8315688867916).Within(1).Percent);
        }
        [Test]
        public void TrackPointCheckLongitudeDegrees() {
            var point = GetTrackPoint();
            Assert.That(Double.Parse(point["Position"]["LongitudeDegrees"].InnerXml), Is.EqualTo(60.550544886862419).Within(1).Percent);
        }
        [Test]
        public void TrackPointCheckAltitudeMeters() {
            var point = GetTrackPoint();
            Assert.That(Double.Parse(point["AltitudeMeters"].InnerXml), Is.EqualTo(261).Within(1).Percent);
        }
        [Test]
        public void TrackPointCheckDistanceMeters() {
            var point = GetTrackPoint();
            Assert.That(Double.Parse(point["DistanceMeters"].InnerXml), Is.EqualTo(0).Within(1).Percent);
        }
        [Test]
        public void TrackPointCheckHeartRateBpm() {
            var point = GetTrackPoint();
            Assert.That(Int32.Parse(point["HeartRateBpm"]["Value"].InnerXml), Is.EqualTo(132).Within(1).Percent);
        }
        [Test]
        public void TrackPointCheckCadence() {
            var point = GetTrackPoint();
            Assert.That(Int32.Parse(point["Cadence"].InnerXml), Is.EqualTo(26).Within(2));
        }
        [Test]
        public void TrackPointCheckSpeed() {
            var point = GetTrackPoint();
            Assert.That(Double.Parse(point["Extensions"]["TPX"]["Speed"].InnerXml), Is.EqualTo(2.389).Within(1).Percent);
        }
    }
}
