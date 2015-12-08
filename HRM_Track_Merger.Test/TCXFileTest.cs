using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test {
    using System.Xml;
    using HRM_Track_Merger.GarminTCX;
    [TestFixture]
    class TCXFileTest {
        [SetUp]
        public void Init() {
        }
        [TearDown]
        public void CleanUp() {
            if (File.Exists(@"sample.tcx")) {
                File.Delete(@"sample.tcx");
            }
        }
        [Test]
        public void SavesTCXFile() {
            var tcxFile = new TCXFile();
            tcxFile.Save(@"sample.tcx");
            Assert.That(File.Exists(@"sample.tcx"),Is.True);
        }
        [Test]
        public void TCXFileNotEmpty() {
            var tcxFile = new TCXFile();
            tcxFile.Save(@"sample.tcx");
            var fileInfo = new FileInfo(@"sample.tcx");
            Assert.That(fileInfo.Length, Is.GreaterThan(0));
        }
        [Test]
        public void SavesExerciseInfo() {
            var doc = GenerateTCXFile();
            Assert.That(doc.DocumentElement.LocalName, Is.EqualTo("TrainingCenterDatabase"));
            Assert.That(doc.DocumentElement.HasChildNodes, Is.True);
            Assert.That(doc.DocumentElement["Activities"], Is.Not.Null);
            Assert.That(doc.DocumentElement["Activities"].HasChildNodes, Is.True, "Activities element has childs");
            Assert.That(doc.DocumentElement["Activities"]["Activity"], Is.Not.Null);

        }
        [Test]
        public void ActivityHasSportAttribute() {
            var doc = GenerateTCXFile();
            Assert.That(doc.DocumentElement["Activities"]["Activity"].GetAttribute("Sport"), Is.EqualTo("Biking"));
        }
        [Test]
        public void ActivityHasIdChild() {
            var doc = GenerateTCXFile();
            Assert.That(doc.DocumentElement["Activities"]["Activity"]["Id"], Is.Not.Null);
            Assert.That(doc.DocumentElement["Activities"]["Activity"]["Id"].InnerXml, Is.StringContaining("2014-01-01T06:00:00.300Z"));
        }
        [Test]
        public void LapCheck() {
            var doc = GenerateTCXFile();

            var lap = doc.DocumentElement["Activities"]["Activity"]["Lap"];

            Assert.That(lap.GetAttribute("StartTime"), 
                Is.StringContaining("2014-01-01T06:00:00.300Z"));

            Assert.That(lap["TotalTimeSeconds"].InnerXml, Is.EqualTo("10"));

            Assert.That(lap["DistanceMeters"].InnerXml, Is.EqualTo("210"));

            Assert.That(lap["MaximumSpeed"].InnerXml, Is.EqualTo("20"));

            Assert.That(lap["Calories"].InnerXml, Is.EqualTo("22"));

            Assert.That(lap["AverageHeartRateBpm"]["Value"].InnerXml, Is.EqualTo("153"));

            Assert.That(lap["Intensity"].InnerXml, Is.EqualTo("Active"));

            Assert.That(lap["Cadence"].InnerXml, Is.EqualTo("56"));

            Assert.That(lap["TriggerMethod"].InnerXml, Is.EqualTo("Manual"));

            Assert.That(lap["Track"].HasChildNodes, Is.True);

            Assert.That(lap["Extensions"]["LX"]["MaxBikeCadence"].InnerXml, Is.EqualTo("57"));
            Assert.That(lap["Extensions"]["LX"]["AvgSpeed"].InnerXml, Is.EqualTo("10.5"));
        }
        [Test]
        public void TrackPointCheck() {
            var doc = GenerateTCXFile();
            var point = doc.DocumentElement["Activities"]["Activity"]["Lap"]["Track"]["Trackpoint"];

            Assert.That(point["Time"].InnerXml, Is.EqualTo("2014-01-01T06:00:00.300Z"));
            Assert.That(point["Position"]["LatitudeDegrees"].InnerXml, Is.EqualTo("10"));
            Assert.That(point["Position"]["LongitudeDegrees"].InnerXml, Is.EqualTo("20"));
            Assert.That(point["AltitudeMeters"].InnerXml, Is.EqualTo("100"));
            Assert.That(point["DistanceMeters"].InnerXml, Is.EqualTo("200"));
            Assert.That(point["HeartRateBpm"]["Value"].InnerXml, Is.EqualTo("150"));
            Assert.That(point["Cadence"].InnerXml, Is.EqualTo("55"));
            Assert.That(point["Extensions"]["TPX"]["Speed"].InnerXml, Is.EqualTo("10"));
        }
        public XmlDocument GenerateTCXFile() {
            
            var track = new List<TrackPoint>();
            track.Add(new TrackPoint() {
                AltitudeMeters = 100,
                Cadence = 55,
                DistanceMeters = 200,
                Extension = new TrackPointExtension() {
                    Speed = 10,
                },
                HeartRateBpm = new HeartRate(150),
                Position = new Position(10, 20),
                Time = new DateTime(2014, 01, 01, 12, 00, 00).AddMilliseconds(300)
            });
            track.Add(new TrackPoint() {
                AltitudeMeters = 101,
                Cadence = 57,
                DistanceMeters = 210,
                Extension = new TrackPointExtension() {
                    Speed = 11,
                },
                HeartRateBpm = new HeartRate(155),
                Position = new Position(10.1, 20.1),
                Time = new DateTime(2014, 01, 01, 12, 00, 05)
            });
            var lap = new Lap() {
                AverageHeartRateBpm = new HeartRate(153),
                Cadence = 56,
                Calories = 22,
                DistanceMeters = 210,
                Extension = new LapExtension() {
                    AvgSpeed = 10.5,
                    MaxBikeCadence = 57,
                },
                Intensity = "Active",
                MaximumHeartRateBpm = new HeartRate(180),
                MaximumSpeed = 20,
                Notes = "qqq",
                StartTime = new DateTime(2014, 01, 01, 12, 00, 00).AddMilliseconds(300),
                TotalTimeSeconds = 10,
                Track = track,
                TriggerMethod = "Manual"
            };
            var exercise = new Activity() {
                Creator = new Creator() {
                    Name = "creator",
                    ProductID = 1,
                    UnitID = 2,
                    Version = new uint[] { 3, 4, 5, 6 },
                },
                Id = lap.StartTime,
                Laps = new List<Lap>(),
                Notes = "www",
                Sport = Sport.Biking
            };
            exercise.Laps.Add(lap);
            var tcxFile = new TCXFile();
            tcxFile.Activities.Add(exercise);
            tcxFile.Save(@"sample.tcx");
            var doc = new XmlDocument();
            doc.Load(@"sample.tcx");
            return doc;
        }
    }
}
