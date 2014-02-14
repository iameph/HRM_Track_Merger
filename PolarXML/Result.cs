using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class Result : Exercise{
        public Result(){
        }
        public Result(XmlElement node) : base(node){
            ParseXmlElement(node);
        }
        private void ParseXmlElement(XmlElement node){

        }
        public static new Result Parse(XmlElement node) {
            return new Result(node);
        }
        //<xs:element ref="heart-rate" minOccurs="0"/>
        public HeartRateRange HeartRate;
        //<xs:element name="recording-rate" type="xs:short" minOccurs="0"/>
        public byte? RecordingRate;
        //<xs:element name="laps" minOccurs="0">
        public List<ExerciseLap> Laps;
        //<xs:element ref="power" minOccurs="0"/>
        public PowerData Power;
        //<xs:element ref="speed" minOccurs="0"/>
        public SpeedData Speed;
        //<xs:element name="altitude" type="float-range" minOccurs="0"/>
        public Range<double> Altitude;
        //<xs:element name="temperature" type="float-range" minOccurs="0"/>
        public Range<double> Temperature;
        //<xs:element ref="altitude-info" minOccurs="0"/>
        public AltitudeInfo AltitudeInfo;
        //-<xs:element name="summary-zone" type="zone" minOccurs="0"/>

        //-<xs:element name="limits" minOccurs="0">

        //<xs:element name="samples" minOccurs="0">
        public List<Sample> Samples;
        //-<xs:element ref="activity-info" minOccurs="0"/>

    }
}
