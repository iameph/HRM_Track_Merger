using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test {
    [TestFixture]
    class TrackPointsCollectionTest {
        private TrackPointsCollection _points;
        [SetUp]
        public void Init() {
            var points = new List<TrackPoint>(new TrackPoint[]{
            new TrackPoint(new DateTime(2000,06,06,12,00,00),60,60,200),
            new TrackPoint(new DateTime(2000,06,06,12,01,00),70,70,300),
            new TrackPoint(new DateTime(2000,06,06,12,02,00),80,50,250),
            new TrackPoint(new DateTime(2000,06,06,12,03,00),100,10,100)
        });
            _points = new TrackPointsCollection(points);
        }
        [Test]
        public void GetTrackPointAt_12_00_00_ReturnsFirstTrackPoint() {
            Assert.AreEqual(_points.TrackPoints[0],_points.GetTrackPointAtTime(new DateTime(2000,06,06,12,00,00)));
        }
        [Test]
        public void GetTrackPointAt_12_03_00_ReturnsLastTrackPoint() {
            Assert.AreEqual(_points.TrackPoints[_points.TrackPoints.Count-1], _points.GetTrackPointAtTime(new DateTime(2000, 06, 06, 12, 03, 00)));
        }
        [Test]
        public void GetTrackPointAt_11_55_00_ReturnsFirstTrackPointWithNewTime() {
            var time = new DateTime(2000, 06, 06, 11, 55, 00);
            var newPoint = _points.GetTrackPointAtTime(time);
            var expectedPoint = _points.TrackPoints[0];
            expectedPoint.Time = time;
            Assert.AreEqual(expectedPoint,newPoint);
        }
        [Test]
        public void GetTrackPointAt_13_00_00_ReturnsLastTrackPointWithNewTime() {
            var time = new DateTime(2000, 06, 06, 13, 00, 00);
            var expectedPoint = _points.GetTrackPointAtTime(time);
            var checkPoint = _points.TrackPoints[_points.TrackPoints.Count-1];
            checkPoint.Time = time;
            Assert.AreEqual(checkPoint, expectedPoint);
        }
        [Test]
        public void GetTrackPointReturnsCopyOfPoint() {
            var point = _points.GetTrackPointAtTime(new DateTime(2000, 06, 06, 12, 00, 00));
            point.Time = new DateTime(1999, 01, 01, 01, 01, 01);
            Assert.AreEqual(new DateTime(2000, 06, 06, 12, 00, 00), _points.TrackPoints[0].Time);
        }
        [Test]
        public void GetTrackPointCorrectlyInterpolates() {
            var time = new DateTime(2000, 06, 06, 12, 00, 30);
            var expectedPoint = new TrackPoint(time, 65, 65, 250);
            Assert.AreEqual(expectedPoint, _points.GetTrackPointAtTime(time));
            time = new DateTime(2000, 06, 06, 12, 01, 30);
            Assert.AreEqual(new TrackPoint(time, 75, 60, 275),_points.GetTrackPointAtTime(time));
            time = new DateTime(2000, 06, 06, 12, 02, 20);
            var newPoint = _points.GetTrackPointAtTime(time);
            Assert.AreEqual(86.67, newPoint.Longitude, 1);
            Assert.AreEqual(36.67, newPoint.Latitude, 1);
            Assert.AreEqual(200, newPoint.Elevation, 1);
        }
    }
}
