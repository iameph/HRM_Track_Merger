using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class ExerciseElement : ExerciseData.IExercise {
        public ExerciseElement() {
        }
        public ExerciseElement(XmlElement elem) {
            ParseXmlElement(elem);
        }
        private void ParseXmlElement(XmlElement elem) {
            var dtfi = System.Globalization.DateTimeFormatInfo.InvariantInfo;
            TimeCreated = DateTime.Parse(elem["created"].InnerXml, dtfi);
            if (elem["time"] != null) {
                Time = DateTime.Parse(elem["time"].InnerXml, dtfi);
            }
            if (elem["sport"] != null) {
                Sport = elem["sport"].InnerXml;
            }
            if (elem["name"] != null) {
                Name = elem["name"].InnerXml;
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



        internal void UpdateUserData(User userData, bool forceReplace) {
            if (Result != null) {
                if (Result.UserSettingsData == null) {
                    Result.UserSettingsData = new UserSettings();
                }
                if (userData.HeartRate != null && (Result.UserSettingsData.HeartRate == null || forceReplace)) {
                    Result.UserSettingsData.HeartRate = (HeartRateRange)userData.HeartRate.Clone();
                }
                if (userData.Height != null && (Result.UserSettingsData.Height == null || forceReplace)) {
                    Result.UserSettingsData.Height = userData.Height;
                }
                if (userData.VO2Max != null && (Result.UserSettingsData.VO2Max == null || forceReplace)) {
                    Result.UserSettingsData.VO2Max = userData.VO2Max;
                }
                if (userData.Weight != null && (Result.UserSettingsData.Weight == null || forceReplace)) {
                    Result.UserSettingsData.Weight = userData.Weight;
                }
            }
        }
        public ExerciseData.UserData GetUserData(){
            var result = new ExerciseData.UserData();
            if (Result != null && Result.UserSettingsData!=null) {
                result.MaxHR = Result.UserSettingsData.HeartRate.Maximum;
                result.RestHR = Result.UserSettingsData.HeartRate.Restring;
                result.VO2Max = Result.UserSettingsData.VO2Max;
                result.Weight = Result.UserSettingsData.Weight;
            }
            return result;
        }
        public List<ExerciseData.DataPoint> GetDataPoints() {
            var result = new List<ExerciseData.DataPoint>();
            if(Result==null || Result.Samples==null)
                return result;
            DateTime time = this.Time != null ? this.Time.Value : this.TimeCreated;
            int count = Result.Samples.Max(sample => sample.Values.Count);
            TimeSpan interval = this.Result.RecordingRate!=null?
                this.Result.RecordingRate.Value
                :
                TimeSpan.FromSeconds(Math.Truncate(this.Result.Duration.TotalSeconds / count));
            for (int i = 0; i < count; ++i,time = time + interval) {
                var point = new ExerciseData.DataPoint();
                foreach (Sample sample in Result.Samples) {
                    switch (sample.SampleType) {
                        case SampleType.AIR_PRESSURE:
                            point.AirPressure = sample.Values[i];
                            break;
                        case SampleType.ALTITUDE:
                            point.Altitude = sample.Values[i];
                            break;
                        case SampleType.CADENCE:
                            point.Cadence = sample.Values[i];
                            break;
                        case SampleType.DISTANCE:
                            point.Distance = sample.Values[i];
                            break;
                        case SampleType.HEARTRATE:
                            point.HeartRate = sample.Values[i];
                            break;
                        case SampleType.POWER:
                            point.Power = sample.Values[i];
                            break;
                        case SampleType.POWER_LRB:
                            point.PowerBalance = sample.Values[i];
                            break;
                        case SampleType.SPEED:
                            point.Speed = sample.Values[i];
                            break;
                        default:
                            break;
                    }
                    
                }
                point.Time = time;
                result.Add(point);
            }
            return result;
        }
        public List<ExerciseData.DataPoint> GetDataPointsWithPartialData() {
            var result = new List<ExerciseData.DataPoint>();
            if (Result == null || Result.Laps == null)
                return result;
            DateTime time = this.Time != null ? this.Time.Value : this.TimeCreated;
            foreach (ExerciseLap lap in Result.Laps) {
                time+=lap.Duration;
                if (lap.EndingValues!=null) {
                    var point = new ExerciseData.DataPoint();
                    point.Time = time;
                    if (lap.EndingValues.Cadence.HasValue) {
                        point.Cadence = lap.EndingValues.Cadence.Value;
                    }
                    if (lap.EndingValues.HeartRate.HasValue) {
                        point.HeartRate= lap.EndingValues.HeartRate.Value;
                    }
                    if (lap.EndingValues.Speed.HasValue) {
                        point.Speed = lap.EndingValues.Speed.Value;
                    }
                    result.Add(point);
                }
            }
            return result;
        }
        public List<ExerciseData.Lap> GetLaps() {
            var result = new List<ExerciseData.Lap>();
            if (Result == null || Result.Laps == null)
                return result;
            DateTime time = this.Time != null ? this.Time.Value : this.TimeCreated;
            foreach (ExerciseLap lap in Result.Laps) {
                var exLap = new ExerciseData.Lap();
                exLap.Totals = new ExerciseData.Summary();
                exLap.Totals.Time = new DateTimeRange(time, lap.Duration);
                if (lap.Ascent.HasValue) {
                    exLap.Totals.Ascent = lap.Ascent.Value;
                }
                if (lap.Cadence != null) {
                    exLap.Totals.Cadence = new Range<double>();
                    if (lap.Cadence.Minimum.HasValue) {
                        exLap.Totals.Cadence.Min = lap.Cadence.Minimum.Value;
                    }
                    if (lap.Cadence.Maximum.HasValue) {
                        exLap.Totals.Cadence.Max = lap.Cadence.Maximum.Value;
                    }
                    if (lap.Cadence.Average.HasValue) {
                        exLap.Totals.Cadence.Avg = lap.Cadence.Average.Value;
                    }
                }
                if (lap.Distance.HasValue) {
                    exLap.Totals.Distance = lap.Distance.Value;
                }
                if (lap.HeartRate != null) {
                    exLap.Totals.HeartRate = new Range<double>();
                    if (lap.HeartRate.Minimum.HasValue) {
                        exLap.Totals.HeartRate.Min = lap.HeartRate.Minimum.Value;
                    }
                    if (lap.HeartRate.Maximum.HasValue) {
                        exLap.Totals.HeartRate.Max = lap.HeartRate.Maximum.Value;
                    }
                    if (lap.HeartRate.Average.HasValue) {
                        exLap.Totals.HeartRate.Avg = lap.HeartRate.Average.Value;
                    }
                }
                if (lap.Power!=null) {
                    exLap.Totals.Power = new Range<double>();
                    if (lap.Power.Power.HasValue) {
                        exLap.Totals.Power.Avg = lap.Power.Power.Value;
                    }
                }
                if (lap.Speed!=null) {
                    exLap.Totals.Speed = new Range<double>();
                    if (lap.Speed.Minimum.HasValue) {
                        exLap.Totals.Speed.Min = lap.Speed.Minimum.Value;
                    }
                    if (lap.Speed.Maximum.HasValue) {
                        exLap.Totals.Speed.Max = lap.Speed.Maximum.Value;
                    }
                    if (lap.Speed.Average.HasValue) {
                        exLap.Totals.Speed.Avg = lap.Speed.Average.Value;
                    }
                }
                if (lap.Temperature != null) {
                    exLap.Totals.Temperature = new Range<double>();
                    if (lap.Temperature.Minimum.HasValue) {
                        exLap.Totals.Temperature.Min = lap.Temperature.Minimum.Value;
                    }
                    if (lap.Temperature.Maximum.HasValue) {
                        exLap.Totals.Temperature.Max = lap.Temperature.Maximum.Value;
                    }
                    if (lap.Temperature.Average.HasValue) {
                        exLap.Totals.Temperature.Avg = lap.Temperature.Average.Value;
                    }
                }
                result.Add(exLap);
            }
            return result;
        }
        public bool IsCadenceDataAvailable { get; private set; }
        public bool IsAltitudeDataAvailable { get; private set; }
        public bool IsCyclingDataAvailable { get; private set; }
        public bool IsImperialSystemUsed { get; private set; }
        public bool IsAirPressureDataAvailable { get; private set; }
        public bool IsSpeedDataAvailable { get; private set; }
        public bool IsPowerDataAvailable { get; private set; }
        public bool IsBalanceDataAvailable { get; private set; }
        public bool IsPedallingIndexDataAvailable { get; private set; }
    }
}
