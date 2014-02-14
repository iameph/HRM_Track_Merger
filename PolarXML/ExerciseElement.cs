using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class ExerciseElement {
        public ExerciseElement(){
        }
        public ExerciseElement(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem){
            var dtfi = System.Globalization.DateTimeFormatInfo.InvariantInfo;
            TimeCreated = DateTime.Parse(elem["created"].InnerXml, dtfi);
            if (elem["time"] != null) {
                Time = DateTime.Parse(elem["time"].InnerXml, dtfi);
            }
            if (elem["sport"] != null) {
                Sport = elem["sport"].InnerXml;
            }
            if (elem["name"] != null) {
                Sport = elem["name"].InnerXml;
            }
            if (elem["result"] != null) {
                Result = Result.Parse(elem["result"]);
            }
            if (elem["note"] != null) {
                Note = elem["note"].InnerXml;
            }
        }
        public static ExerciseElement Parse(XmlElement elem) {
            return new ExerciseElement(elem);
        }
        public DateTime TimeCreated;
        public DateTime? Time;
        //xs:element name="sport" minOccurs="0">...</xs:element>
        public string Sport;
        //<xs:element name="name" minOccurs="0">...</xs:element>
        public string Name;
        //<xs:element ref="target" minOccurs="0"/>
        //<xs:element ref="result" minOccurs="0"/>
        public Result Result;
        //<xs:element name="sport-results" minOccurs="0">...</xs:element>
        //<xs:element name="note" minOccurs="0">...</xs:element>
        public string Note;
    }
}
