using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test {
    [TestFixture]
    class CommandLineArgumentsTest {
        
        private CommandLineArguments _args;
        [SetUp]
        public void Init() {
            _args = CommandLineArguments.Parse(new string[]{CommonData.PathToSampleFiles+"sample.gpx",CommonData.PathToSampleFiles+"sample.hrm",
                "/weight:12.5","/age:24","/output:output.tcx","/output2:\"C:\\Program Files\""});
        }
        [Test]
        public void HRMFileNameCorrect() {
            Assert.That(_args.HRMFileName, Is.StringEnding("sample.hrm"));
        }
        [Test]
        public void GPSFileNameCorrect() {
            Assert.That(_args.GPSFileName, Is.StringEnding("sample.gpx"));
        }
        [Test]
        public void WeightOptionCorrect() {
            var opts = _args.GetOptions();
            Assert.That(opts.ContainsKey("weight"),Is.True,"weight option is absent");
            Assert.That(opts["weight"],Is.EqualTo("12.5"));
        }
        [Test]
        public void OutputOptionCorrect() {
            var opts = _args.GetOptions();
            Assert.That(opts.ContainsKey("output"), Is.True, "output option is absent");
            Assert.That(opts["output"], Is.EqualTo("output.tcx"));
        }
        [Test]
        public void Output2OptionCorrect() {
            var opts = _args.GetOptions();
            Assert.That(opts.ContainsKey("output2"), Is.True, "output2 option is absent");
            Assert.That(opts["output2"], Is.EqualTo("C:\\Program Files"));
        }
    }
}
