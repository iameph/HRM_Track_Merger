using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HRM_Track_Merger.Test {
    [TestFixture]
    class SettingsTest {
        private Settings settings;
        [SetUp]
        public void Init() {
            settings = new Settings() {
                Birthday=new DateTime(1989,07,03)
            };
        }
        [Test]
        public void CalculatesAgeFromBithday() {
            Assert.That(settings.GetUserData(new DateTime(2014, 02, 10)).Age, Is.EqualTo(24.6).Within(1).Percent);
        }
    }
}
