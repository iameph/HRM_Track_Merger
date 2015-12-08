using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test {
    [TestFixture]
    public class GPXFileTest {
        private GPXFile file;
        [SetUp]
        public void Init() {
            file = new GPXFile(CommonData.PathToSampleFiles + "sample.gpx");
        }
        [Test]
        public void GetTrackPointsActuallyReturnsSmt() {
            Assert.IsNotEmpty(file.GetTrackPoints());
        }
        [Test]
        public void RemoveTrackPointsRemovesIt() {
            file.RemoveTrackPoints();
            Assert.IsEmpty(file.GetTrackPoints());
        }
        [Test]
        public void CreateNewSegmentAddsNewSegment() {
            var oldCount = file.File.GetElementsByTagName("trkseg").Count;
            file.CreateNewSegment();
            Assert.Greater(file.File.GetElementsByTagName("trkseg").Count,oldCount);
        }
        [Test]
        public void RecognizePolarGPX() {
            file = new GPXFile(CommonData.PathToSampleFiles + "sample_polar.gpx");
            Assert.That(file.IsPolarGPX, Is.True);
        }
        [Test]
        public void FixPolarOffset() {
            file = new GPXFile(CommonData.PathToSampleFiles + "sample_polar.gpx");
            List<TrackPoint> points = file.GetTrackPoints();
            Assert.That(points[0].Time, Is.EqualTo(new DateTime(2014, 07, 02, 17, 33, 47)));
        }
    }
}

