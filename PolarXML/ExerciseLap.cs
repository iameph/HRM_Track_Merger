using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class ExerciseLap {
        public ExerciseLap() { }
        public ExerciseLap(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            Index = Int32.Parse(elem.GetAttribute("index"));
            if (elem["duration"] != null) {
                Duration = TimeSpan.Parse(elem["duration"].InnerXml);
            }
            if (elem["heart-rate"]!=null) {
                HeartRate = HeartRateRange.Parse(elem["heart-rate"]);
            }
            if (elem["speed"] != null) {
                Speed = FloatRange.Parse(elem["speed"]);
            }
            if (elem["cadence"] != null) {
                Cadence = ShortRange.Parse(elem["cadence"]);
            }
            if (elem["power"] != null) {
                Power = PowerData.Parse(elem["power"]);
            }
            if (elem["temperature"] != null) {
                Temperature = FloatRange.Parse(elem["temperature"]);
            }
            if (elem["altitude"] != null) {
                Altitude = Double.Parse(elem["altitude"].InnerXml);
            }
            if (elem["ascent"] != null) {
                Ascent = Double.Parse(elem["ascent"].InnerXml);
            }
            if (elem["descent"] != null) {
                Descent = Double.Parse(elem["descent"].InnerXml);
            }
            if (elem["distance"] != null) {
                Distance = Double.Parse(elem["distance"].InnerXml);
            }
            if (elem["ending-values"] != null) {
                EndingValues = EndingValues.Parse(elem["ending-values"]);
            }
        }
        public static ExerciseLap Parse(XmlElement elem) {
            return new ExerciseLap(elem);
        }
        public int Index;
        //xs:element name="duration" minOccurs="0">
        public TimeSpan Duration;
        //<xs:element ref="heart-rate" minOccurs="0"/>
        public HeartRateRange HeartRate;
        //<xs:element name="speed" type="float-range" minOccurs="0"/>
        public FloatRange Speed;
        //<xs:element name="cadence" type="short-range" minOccurs="0"/>
        public ShortRange Cadence;
        //<xs:element ref="power" minOccurs="0"/>
        public PowerData Power;
        //<xs:element name="temperature" type="float-range" minOccurs="0"/>
        public FloatRange Temperature;
        //<xs:element name="altitude" type="xs:float" minOccurs="0"/>
        public double? Altitude;
        //<xs:element name="ascent" type="xs:float" minOccurs="0"/>
        public double? Ascent;
        //<xs:element name="descent" type="xs:float" minOccurs="0"/>
        public double? Descent;
        //<xs:element name="distance" minOccurs="0">...</xs:element>
        public double? Distance;
        //<xs:element ref="ending-values" minOccurs="0"/>
        public EndingValues EndingValues;

        public class IndexComparer : IComparer<ExerciseLap> {
            public int Compare(ExerciseLap x, ExerciseLap y) {
                return x.Index.CompareTo(y.Index);
            }
        }
    }
}
