﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class ShortRange {
        public ShortRange() { }
        public ShortRange(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            if (elem["minimum"] != null) {
                Minimum = Byte.Parse(elem["minimum"].InnerXml);
            }
            if (elem["average"] != null) {
                Average = Byte.Parse(elem["average"].InnerXml);
            }
            if (elem["maximum"] != null) {
                Maximum = Byte.Parse(elem["maximum"].InnerXml);
            }
        }
        public static ShortRange Parse(XmlElement elem) {
            return new ShortRange(elem);
        }
        public byte? Minimum;
        public byte? Maximum;
        public byte? Average;
    }
}