using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test {
    using HRM_Track_Merger.PolarHRM;
    [TestFixture]
    class PolarHRMFileTest {
        public static readonly string PathToSampleFiles = @"..\..\..\HRM_Track_Merger\Samples\";
        private PolarHRMFile file;
        [SetUp]
        public void Init() {
            file = PolarHRMFile.Parse(PathToSampleFiles + "sample.hrm");
        }
        [Test]
        public void PolarHRMSampleFileReturns106() {
            Assert.That(file.Version, Is.EqualTo(106));
        }
        [Test]
        public void PolarHRMSampleFileStartTimeCorrect() {
            Assert.That(file.StartTime, Is.EqualTo(new DateTime(2014, 01, 17, 12, 16, 32)));
        }
        [Test]
        public void PolarHRMSampleFileDurationCorrect() {
            Assert.That(file.Duration, Is.EqualTo(new TimeSpan(3, 28, 55)));
        }
        [Test]
        public void PolarHRMSampleFileIntervalCorrect() {
            Assert.That(file.Interval, Is.EqualTo(new TimeSpan(0, 0, 5)));
        }
        [Test]
        public void PolarHRMSampleFileAltitudeFlagCorrect() {
            Assert.That(file, Is.InstanceOf<PolarHRMFile106>());
            Assert.That((file as PolarHRMFile106).IsAltitudeDataAvailable, Is.True);
        }
        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void WrongParamNameThrowsException() {
            file.GetParam("qqq");
        }
        [Test]
        public void PolarHRMSampleFileNoteCorrect() {
            Assert.That(file.Note, Is.StringContaining("ляляля"));
        }
        [Test]
        public void TimeSpanSecondsMore60() {
            Assert.That(new TimeSpan(0, 0, 238).TotalSeconds, Is.EqualTo(238));
        }
        [Test]
        public void CorrectWeightFromSample() {
            Assert.That(file.UserSettings.Weight, Is.EqualTo(65));
        }
        [Test]
        public void CorrectLapTimeFromSample() {
            Assert.That(file.Laps[0].Time, Is.EqualTo(new TimeSpan(0, 0, 10, 43, 900)));
        }
        [Test]
        public void CorrectLapTemperatureFromSample() {
            Assert.That(file.Laps[6].Temperature, Is.EqualTo(-8));
        }
        [Test]
        public void CorrectLapStartTimeCalculation() {
            Assert.That(file.Laps[6].StartTime, Is.EqualTo(new DateTime(2014, 01, 17, 14, 54, 34).AddMilliseconds(300)));
        }
        [Test]
        public void LastLapTimeEqualsExerciseduration() {
            Assert.That(file.Laps.Last<Lap>().Time, Is.EqualTo(file.Duration));
        }
        [Test]
        public void CorrectLapNote() {
            Assert.That(file.Laps[0].Note, Is.EqualTo("ололо"));
        }
        [Test]
        public void TripSecondsEqualsDuration() {
            Assert.That(file.Trip.TotalTime, Is.EqualTo(file.Duration).Within(TimeSpan.FromSeconds(5)));
        }
        [Test]
        [TestCase(0, 8.6)]
        [TestCase(2, 11.0)]
        public void DataPointSpeed(int idx, double speed) {
            Assert.That(file.HRData[idx].Speed, Is.EqualTo(speed));
        }
        [Test]
        public void DataPointsCount() {
            Assert.That(file.HRData.Count, Is.EqualTo(file.Duration.TotalSeconds / file.Interval.TotalSeconds + 1));
        }
        [Test]
        public void AltitudeData() {
            Assert.That(file.IsAltitudeDataAvailable, Is.True);
        }
        [Test]
        public void VO2Max() {
            Assert.That(file.UserSettings.VO2Max, Is.EqualTo(55));
        }
    }
}
