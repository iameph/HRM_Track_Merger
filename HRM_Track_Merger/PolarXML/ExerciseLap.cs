﻿using System;
using System.Collections.Generic;
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
        public int Index { get; set; }
        //xs:element name="duration" minOccurs="0">
        public TimeSpan Duration { get; set; }
        //<xs:element ref="heart-rate" minOccurs="0"/>
        public HeartRateRange HeartRate { get; set; }
        //<xs:element name="speed" type="float-range" minOccurs="0"/>
        public FloatRange Speed { get; set; }
        //<xs:element name="cadence" type="short-range" minOccurs="0"/>
        public ShortRange Cadence { get; set; }
        //<xs:element ref="power" minOccurs="0"/>
        public PowerData Power { get; set; }
        //<xs:element name="temperature" type="float-range" minOccurs="0"/>
        public FloatRange Temperature { get; set; }
        //<xs:element name="altitude" type="xs:float" minOccurs="0"/>
        public double? Altitude { get; set; }
        //<xs:element name="ascent" type="xs:float" minOccurs="0"/>
        public double? Ascent { get; set; }
        //<xs:element name="descent" type="xs:float" minOccurs="0"/>
        public double? Descent { get; set; }
        //<xs:element name="distance" minOccurs="0">...</xs:element>
        public double? Distance { get; set; }
        //<xs:element ref="ending-values" minOccurs="0"/>
        public EndingValues EndingValues { get; set; }

        public class IndexComparer : IComparer<ExerciseLap> {
            public int Compare(ExerciseLap x, ExerciseLap y) {
                return x.Index.CompareTo(y.Index);
            }
        }
    }
}
