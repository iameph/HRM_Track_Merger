using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Xml;

namespace HRM_Track_Merger.Test {
    [TestFixture]
    class XmlDocumentTest {
        private XmlDocument _doc;
        [SetUp]
        public void Init() {
            _doc = new XmlDocument();
            _doc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><root></root>");
        }
        [Test]
        public void InsertNullChild() {
           Assert.Throws<NullReferenceException>(()=> _doc.DocumentElement.AppendChild(null));
        }
    }
}
