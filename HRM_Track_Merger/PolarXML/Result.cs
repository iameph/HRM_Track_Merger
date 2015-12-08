using System;
using System.Collections.Generic;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class Result : Exercise{
        public Result(){
        }
        public Result(XmlElement node) : base(node){
            ParseXmlElement(node);
        }
        private void ParseXmlElement(XmlElement elem){
            
            foreach (XmlElement el in elem.ChildNodes) {
                switch (el.LocalName) {
                    case "heart-rate":
                        HeartRate = HeartRateRange.Parse(el);
                        break;
                    case "recording-rate":
                        RecordingRate = new TimeSpan(0,0,Byte.Parse(el.InnerXml));
                        break;
                    case "laps":
                        if (Laps == null) {
                            Laps = new List<ExerciseLap>();
                        }
                        foreach (XmlElement lap in el) {
                            if (lap.LocalName == "lap") {
                                Laps.Add(ExerciseLap.Parse(lap));
                            }
                        }
                        break;
                    case "power":
                        Power = PowerData.Parse(el);
                        break;
                    case "speed":
                        Speed = SpeedData.Parse(el);
                        break;
                    case "altitude":
                        Altitude = FloatRange.Parse(el);
                        break;
                    case "temperature":
                        Temperature = FloatRange.Parse(el);
                        break;
                    case "altitude-info":
                        AltitudeInfo = AltitudeInfo.Parse(el);
                        break;
                    case "samples":
                        Samples = new List<Sample>();
                        foreach (XmlElement sampleElement in el) {
                            Samples.Add(Sample.Parse(sampleElement));
                        }
                        break;
                    default:
                        break;
                }
            }
            Laps.Sort(new ExerciseLap.IndexComparer());
        }
        public static new Result Parse(XmlElement node) {
            return new Result(node);
        }
        //<xs:element ref="heart-rate" minOccurs="0"/>
        public HeartRateRange HeartRate { get; set; }
        //<xs:element name="recording-rate" type="xs:short" minOccurs="0"/>
        public TimeSpan? RecordingRate { get; set; }
        //<xs:element name="laps" minOccurs="0">
        public List<ExerciseLap> Laps { get; set; }
        //<xs:element ref="power" minOccurs="0"/>
        public PowerData Power { get; set; }
        //<xs:element ref="speed" minOccurs="0"/>
        public SpeedData Speed { get; set; }
        //<xs:element name="altitude" type="float-range" minOccurs="0"/>
        public FloatRange Altitude { get; set; }
        //<xs:element name="temperature" type="float-range" minOccurs="0"/>
        public FloatRange Temperature { get; set; }
        //<xs:element ref="altitude-info" minOccurs="0"/>
        public AltitudeInfo AltitudeInfo { get; set; }
        //-<xs:element name="summary-zone" type="zone" minOccurs="0"/>

        //-<xs:element name="limits" minOccurs="0">

        //<xs:element name="samples" minOccurs="0">
        public List<Sample> Samples { get; set; }
        //-<xs:element ref="activity-info" minOccurs="0"/>

    }
}
