using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test.PolarXMLTests {
    using HRM_Track_Merger.PolarXML;
    [TestFixture]
    [SetCulture("")]
    class PolarXMLExportTest {
        private PolarXMLFile file;
        [SetUp]
        public void Init() {
            file = new PolarXMLFile(CommonData.PathToSampleFiles + "sample.xml");
        }
        [Test]
        public void GetUserData(){
            var data = file.Exercises[0].GetUserData();
            Assert.That(data.VO2Max,Is.EqualTo(41));
            Assert.That(data.MaxHR,Is.EqualTo(196));
            Assert.That(data.Weight,Is.EqualTo(65));
        }
        [Test]
        public void GetDataPoints() {
            var points = file.Exercises[0].GetDataPoints();
            Assert.That(points[1].Altitude, Is.EqualTo(256.9464));
            Assert.That(points[1].Cadence, Is.EqualTo(69));
            Assert.That(points[1].Distance, Is.EqualTo(0));
            Assert.That(points[1].HeartRate, Is.EqualTo(123));
            Assert.That(points[1].Speed, Is.EqualTo(12.8));
            Assert.That(points[1].Time, Is.EqualTo(new DateTime(2014,02,10,16,37,19)));
        }
        [Test]
        public void GetDataPointsWithPartialData() {
            var points = file.Exercises[0].GetDataPointsWithPartialData();
            Assert.That(points, Is.Not.Empty);
            Assert.That(points[1].Time, Is.EqualTo(new DateTime(2014, 02, 10, 16, 37, 14) + new TimeSpan(0, 39, 02)));
            Assert.That(points[1].HeartRate, Is.EqualTo(134));
            Assert.That(points[1].Speed, Is.EqualTo(6.6));
        }
        [Test]
        public void GetLaps() {
            var laps = file.Exercises[0].GetLaps();
            var lap2 = laps[2].Totals;
            Assert.That(lap2.Ascent,Is.EqualTo(50.9016));
            Assert.That(lap2.Cadence.Avg, Is.EqualTo(57));
            Assert.That(lap2.Temperature.Avg, Is.EqualTo(-23.3333));
            Assert.That(lap2.Distance, Is.EqualTo(7456.0));
            Assert.That(lap2.HeartRate.Avg, Is.EqualTo(138));
            Assert.That(lap2.HeartRate.Max, Is.EqualTo(162));
            Assert.That(lap2.Speed.Avg, Is.EqualTo(6));
        }
    }
}
