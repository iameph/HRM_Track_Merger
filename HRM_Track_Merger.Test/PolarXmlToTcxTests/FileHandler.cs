using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test.PolarXmlToTcxTests {
    [SetUpFixture]
    [SetCulture("")]
    public class FileHandler {
        internal static GarminTCX.TCXFile tcxFile;
        internal static GarminTCX.TCXFile tcxWithTrackFile;
        [SetUp]
        public void Init() {
            var xmlFile = new PolarXML.PolarXMLFile(CommonData.PathToSampleFiles + "sample.xml");
            var gpxFile = new GPXFile(PolarHRMFileTest.PathToSampleFiles + "sample.gpx");
            var exercises = xmlFile.GetExercises();
            var exerciseData = new ExerciseData.CommonExerciseData(exercises[0]);
            tcxFile = exerciseData.ConvertToTCX();
            exerciseData.AddGPSData(gpxFile, TimeSpan.Zero);
            tcxWithTrackFile = exerciseData.ConvertToTCX();
            tcxFile.Save(@"sample.tcx");
            tcxWithTrackFile.Save(@"sample_track.tcx");
        }
        [TearDown]
        public void CleanUP() {
            if (System.IO.File.Exists(@"sample.tcx")) {
                System.IO.File.Delete(@"sample.tcx");
            }
            if (System.IO.File.Exists(@"sample_track.tcx")) {
                System.IO.File.Delete(@"sample_track.tcx");
            }
        }
    }
}
