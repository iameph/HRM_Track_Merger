using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test.HRMGPXMergeTests {
    [SetUpFixture]
    public class FileHandler {
        internal static GarminTCX.TCXFile tcxFile;
        [SetUp]
        public void Init() {
            var hrmFile = PolarHRM.PolarHRMFile.Parse(PolarHRMFileTest.PathToSampleFiles + "sample.hrm");
            var gpxFile = new GPXFile(PolarHRMFileTest.PathToSampleFiles + "sample.gpx");
            var exerciseData = new ExerciseData.CommonExerciseData(hrmFile);
            exerciseData.AddGPSData(gpxFile, TimeSpan.Zero);
            tcxFile = exerciseData.ConvertToTCX();
            tcxFile.Save(@"sample.tcx");
        }
        [TearDown]
        public void CleanUP() {
            if (System.IO.File.Exists(@"sample.tcx")) {
                System.IO.File.Delete(@"sample.tcx");
            }
        }
    }
}
