using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test.PolarXMLTests {
    using HRM_Track_Merger.PolarXML;
    [TestFixture]
    [SetCulture("")]
    class PolarXMLFileTest {
        private PolarXMLFile file;
        [SetUp]
        public void Init(){
            file = new PolarXMLFile(CommonData.PathToSampleFiles + "sample.xml");
        }
        [Test]
        public void GetExerciseFromFile() {
            Assert.That(file.Exercises, Is.Not.Empty);
        }
        [Test]
        public void GetExerciseTime() {
            Assert.That(file.Exercises[0].TimeCreated, Is.EqualTo(new DateTime(2014, 02, 11, 14, 54, 47)),"Created time incorrect");
            Assert.That(file.Exercises[0].Time, Is.EqualTo(new DateTime(2014, 02, 10, 16, 37, 14)), "Time incorrect");
            Assert.That(file.Exercises[0].Name, Is.EqualTo("Free"), "Name incorrect");
            Assert.That(file.Exercises[0].Sport, Is.EqualTo("MTB"), "Sport incorrect");
        }
        [Test]
        public void GetResult() {
            var res = file.Exercises[0].Result;
            Assert.That(res.Distance, Is.EqualTo(37500.0), "Distance incorrect");
            Assert.That(res.Calories, Is.EqualTo(1570), "Calories incorrect");
            Assert.That(res.Duration, Is.EqualTo(new TimeSpan(02,32,59)), "Duration incorrect");
            Assert.That(res.HeartRate.Average, Is.EqualTo(142), "Heartrate avg incorrect");
            Assert.That(res.HeartRate.Maximum, Is.EqualTo(171), "Heartrate max incorrect");
            Assert.That(res.RecordingRate.Value.TotalSeconds, Is.EqualTo(5), "Recording rate incorrect");
            Assert.That(res.Laps, Is.Not.Empty,"Laps empty");
        }
        [Test]
        public void GetResultUserSettings() {
            var userSettings = file.Exercises[0].Result.UserSettingsData;
            Assert.That(userSettings.HeartRate.Restring, Is.EqualTo(60),"Resting HR incorrect");
            Assert.That(userSettings.HeartRate.Maximum, Is.EqualTo(196), "Maximum HR incorrect");
            Assert.That(userSettings.VO2Max, Is.EqualTo(41), "VO2Max incorrect");
            Assert.That(userSettings.Weight, Is.EqualTo(65), "Weight incorrect");
        }
        [Test]
        public void GetLap() {
            var lap = file.Exercises[0].Result.Laps[1];
            Assert.That(lap.Duration, Is.EqualTo(new TimeSpan(0,28,27)), "Duration incorrect");
            Assert.That(lap.HeartRate.Average, Is.EqualTo(147), "HR avg incorrect");
            Assert.That(lap.HeartRate.Maximum, Is.EqualTo(171), "HR max incorrect");
            Assert.That(lap.Speed.Average, Is.EqualTo(11), "Speed avg incorrect");
            Assert.That(lap.Cadence.Average, Is.EqualTo(61), "Cadence avg incorrect");
            Assert.That(lap.Temperature.Average, Is.EqualTo(-22.78).Within(0.1),"Temp avg incorrect");
            Assert.That(lap.Altitude, Is.EqualTo(72.23).Within(0.1), "Altitude incorrect");
            Assert.That(lap.Ascent, Is.EqualTo(51.51).Within(0.1), "Ascent incorrect");
            Assert.That(lap.Descent, Is.EqualTo(54.864).Within(0.1), "Descent incorrect");
            Assert.That(lap.Distance, Is.EqualTo(7457).Within(0.1), "Distance incorrect");
            Assert.That(lap.EndingValues.HeartRate, Is.EqualTo(134).Within(0.1), "HR end incorrect");
            Assert.That(lap.EndingValues.Speed, Is.EqualTo(6.6).Within(0.1), "Speed incorrect");
            var sum = file.Exercises[0].Result.Laps.Aggregate(0.0, (acc, Lap) => (acc + Lap.Duration.TotalSeconds));
            Assert.That(sum, Is.EqualTo(file.Exercises[0].Result.Duration.TotalSeconds));
        }
        [Test]
        public void GetResultSpeed() {
            var speed = file.Exercises[0].Result.Speed;
            Assert.That(speed.SpeedType, Is.EqualTo(SpeedType.CYCLING), "Speedtype incorrect");
            Assert.That(speed.Speed.Average, Is.EqualTo(15), "Speed avg incorrect");
            Assert.That(speed.Speed.Maximum, Is.EqualTo(33.6449), "Speed max incorrect");
            Assert.That(speed.Cadence.Average, Is.EqualTo(68), "Cadence avg incorrect");
            Assert.That(speed.Cadence.Maximum, Is.EqualTo(91), "Cadence max incorrect");
        }
        [Test]
        public void GetResultAltitude() {
            var alt = file.Exercises[0].Result.Altitude;
            Assert.That(alt.Minimum, Is.EqualTo(71.628), "Altitude min incorrect");
            Assert.That(alt.Average, Is.EqualTo(74.9808), "Altitude avg incorrect");
            Assert.That(alt.Maximum, Is.EqualTo(84.4296), "Altitude max incorrect");
            var altInfo = file.Exercises[0].Result.AltitudeInfo;
            Assert.That(altInfo.Ascent, Is.EqualTo(264.871), "Ascent incorrect");
            Assert.That(altInfo.Descent, Is.EqualTo(343.814), "Descent incorrect");
        }
        [Test]
        public void GetSamples() {
            var samples = file.Exercises[0].Result.Samples;
            Assert.That(samples.Count, Is.EqualTo(4));
            Assert.That(samples[0].SampleType, Is.EqualTo(SampleType.HEARTRATE),"Sample 0 type");
            Assert.That(samples[1].SampleType, Is.EqualTo(SampleType.SPEED),"Sample 1 type");
            Assert.That(samples[2].SampleType, Is.EqualTo(SampleType.CADENCE),"Sample 2 type");
            Assert.That(samples[3].SampleType, Is.EqualTo(SampleType.ALTITUDE),"Sample 3 type");
            Assert.That(samples[2].Values.Count, Is.EqualTo(samples[3].Values.Count));
            var rate = file.Exercises[0].Result.RecordingRate.Value.TotalSeconds;
            Assert.That((samples[0].Values.Count - 1) * rate, Is.EqualTo(file.Exercises[0].Result.Duration.TotalSeconds).Within(rate));
        }
    }
}
