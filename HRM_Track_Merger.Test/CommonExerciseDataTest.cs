using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test {
    [TestFixture]
    class CommonExerciseDataTest {
        private ExerciseData.CommonExerciseData data;
        [SetUp]
        public void Init() {
            ExerciseData.UserData userData= new ExerciseData.UserData() {
                Age = 24,
                Sex = ExerciseData.Sex.Male
            };
            data = new ExerciseData.CommonExerciseData(PolarHRM.PolarHRMFile.Parse(CommonData.PathToSampleFiles + "sample.hrm"));
            data.UpdateUserData(userData,true);
            data.UpdateCaloriesData();
        }
        [Test]
        public void CalculateOverallDistanceFromDataPoints() {

        }
        [Test]
        [TestCase(0, 1, 2, 3, 0.5, 2.5)]
        [TestCase(0, 1, 2, 3, 5, 3)]
        [TestCase(0, 0, 2, 3, 0, 2)]
        public void InterpolationWorks(double x1, double x2, double y1, double y2, double x, double y) {
            Assert.That(y, Is.EqualTo(ExerciseData.DataPoint.Interpolate(x1, x2, y1, y2, x)));
        }
        [Test]
        public void InsertLapDataAsDataPoint() {
            Assert.That(data.DataPointExists(new DateTime(2014, 01, 17, 12, 27, 15).AddMilliseconds(900)), Is.True);
        }
        [Test]
        public void CalculateRangeData() {
            var sum = data.Laps[0].Totals;
            Assert.That(sum.Distance, Is.EqualTo(2.892).Within(1).Percent);
            Assert.That(sum.HeartRate.Min, Is.EqualTo(119).Within(1).Percent);
            Assert.That(sum.HeartRate.Avg, Is.EqualTo(144).Within(1).Percent);
            Assert.That(sum.HeartRate.Max, Is.EqualTo(159).Within(1).Percent);
            Assert.That(sum.Speed.Avg, Is.EqualTo(16.6).Within(1).Percent);
            Assert.That(sum.Cadence.Avg, Is.EqualTo(64).Within(1).Percent);
            Assert.That(sum.Ascent, Is.EqualTo(0).Within(5));
        }
        [Test]
        public void CalculateTotals() {
            Assert.That(data.Totals.Distance, Is.EqualTo(51.7).Within(1).Percent);
            Assert.That(data.Totals.HeartRate.Avg, Is.EqualTo(148).Within(1).Percent);
            Assert.That(data.Totals.HeartRate.Max, Is.EqualTo(176).Within(1).Percent);
            Assert.That(data.Totals.Speed.Avg, Is.EqualTo(15.3).Within(1).Percent);
        }
        [Test]
        public void GPSDataAdded() {
            data.AddGPSData(new GPXFile(CommonData.PathToSampleFiles + "sample.gpx"), TimeSpan.Zero);
            Assert.That(data.DataPoints[0].Latitude, Is.EqualTo(56.8315688867916));
            Assert.That(data.DataPoints.Last().Latitude, Is.EqualTo(56.831371417728235));
        }
        [Test]
        public void SampleGPXAddsNoPoints() {
            var before = data.DataPoints.Count;
            data.AddGPSData(new GPXFile(CommonData.PathToSampleFiles + "sample.gpx"), TimeSpan.Zero);
            Assert.That(data.DataPoints.Count, Is.EqualTo(before));
        }
        [Test]
        public void Sample2GPXAddsPoints() {
            var before = data.DataPoints.Count;
            data.AddGPSData(new GPXFile(CommonData.PathToSampleFiles + "sample2.gpx"), TimeSpan.Zero);
            Assert.That(data.DataPoints.Count, Is.GreaterThan(before));
        }
        [Test]
        public void Sample2GPXCorrectDataAdded() {
            data.AddGPSData(new GPXFile(CommonData.PathToSampleFiles + "sample2.gpx"), TimeSpan.Zero);
            Assert.That(data.DataPoints[0].Latitude, Is.EqualTo(56.8315688867916).Within(0.001));
            Assert.That(data.DataPoints.Last().Latitude, Is.EqualTo(56.831371417728235).Within(0.001));
        }
        [Test]
        public void DataPointsSortedAfterAddGPS() {
            data.AddGPSData(new GPXFile(CommonData.PathToSampleFiles + "sample2.gpx"), TimeSpan.Zero);
            for (int i = 1; i < data.DataPoints.Count; ++i) {
                if (data.DataPoints[i].Time <= data.DataPoints[i - 1].Time) {
                    Assert.Fail("Data not sorted, first occurence at index {0}", i);
                }
            }
            Assert.Pass();
        }
        [Test]
        public void AltitudeData() {
            Assert.That(data.IsAltitudeDataAvailable, Is.True);
        }
        [Test]
        public void CaloriesCalculationLap() {
            Assert.That(data.Laps[0].Totals.Calories, Is.EqualTo(128).Within(10).Percent);
        }
        [Test]
        public void CaloriesCalculation() {
            Assert.That(data.Totals.Calories, Is.EqualTo(2299.0).Within(20).Percent);
        }
        [Test]
        public void CaloriesCalculationMethod() {
            Assert.That(data.calculateCalories(150, new TimeSpan(1, 0, 0)), Is.EqualTo(770).Within(1).Percent);
        }
        [Test]
        public void UserData() {
            Assert.That(data.UserData.Age, Is.EqualTo(24),"Age incorrect");
            Assert.That(data.UserData.Sex, Is.EqualTo(ExerciseData.Sex.Male), "Sex incorrect");
            Assert.That(data.UserData.VO2Max, Is.EqualTo(55),"VO2Max incorrect");
            Assert.That(data.UserData.Weight, Is.EqualTo(65), "Weight incorrect");
        }
        [Test]
        public void TrackpointDistanceSet() {
            Assert.That(data.DataPoints.Last().Distance, Is.GreaterThan(0));
        }
    }
}
