using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.PolarHRM {
    //TODO:
    //1. Create last lap automatically
    //2. Create new track points from lap data
    class PolarHRMFile {
        protected IDictionary<string, string> Params;
        protected virtual int LapsDataCount { get { return 16; } }
        protected IDictionary<string, List<string>> sections;
        protected PolarHRMFile() {
        }
        public static PolarHRMFile Parse(string fileName) {
            var sections = GetSections(fileName);
            PolarHRMFile hrmFile;
            if (!sections.ContainsKey("Params")) {
                throw new InvalidFileFormatException("Wrong Polar HRM file format: no [Params] section");
            }
            var Params = GetParams(sections["Params"]);
            try {
                int version = Int32.Parse(Params["Version"]);
                switch (version) {
                    case 102:
                        hrmFile = new PolarHRMFile102();
                        break;
                    case 105:
                        hrmFile = new PolarHRMFile105();
                        break;
                    case 106:
                        hrmFile = new PolarHRMFile106();
                        break;
                    case 107:
                        hrmFile = new PolarHRMFile107();
                        break;
                    default:
                        hrmFile = new PolarHRMFile();
                        break;
                }
                hrmFile.Params = Params;
                hrmFile.sections = sections;
                hrmFile.Version = version;
                hrmFile.parseParams();
            }
            catch (KeyNotFoundException) {
                throw new InvalidFileFormatException("Wrong Polar HRM file format: [Params] section has incorrect or missing data");
            }
            try {
                hrmFile.parseSections();
            }
            catch (Exception e) {
                throw new InvalidFileFormatException("Can't read data from file: wrong format of file or program error. " + e.Message);
            }
            return hrmFile;
        }
        protected virtual void parseSections() {
            parseNote();
            parseLaps();
            parseLapNotes();
            parseTrip();
            parseHRData();
        }

        protected virtual void parseHRData() {
            HRData = new List<HRDataPoint>();
            var HRDataStrings = sections["HRData"];
            foreach (var HRDataString in HRDataStrings) {
                HRData.Add(parseHRDataString(HRDataString));
            }
        }

        protected virtual HRDataPoint parseHRDataString(string HRDataString) {
            return new HRDataPoint();
        }

        private void parseTrip() {
            if (sections.ContainsKey("Trip")) {
                var tripData = new List<int>();
                foreach (var data in sections["Trip"]) {
                    int val;
                    if (!Int32.TryParse(data, out val)) {
                        val = 0;
                    }
                    tripData.Add(val);
                }
                if (tripData.Count > 7) {
                    Trip = new TripData();
                    Trip.Distance = tripData[0] / 10.0;
                    Trip.Ascent = getAltitudeFromTrip(tripData[1]);
                    Trip.TotalTime = TimeSpan.FromSeconds(tripData[2]);
                    Trip.AvgAltitude = getAltitudeFromTrip(tripData[3]);
                    Trip.MaxAltitude = getAltitudeFromTrip(tripData[4]);
                    Trip.AvgSpeed = getSpeedFromTrip(tripData[5]);
                    Trip.MaxSpeed = getSpeedFromTrip(tripData[6]);
                    Trip.Odometer = tripData[7];
                }
            }
        }

        protected virtual int getAltitudeFromTrip(int ascent) {
            return ascent * 10;
        }
        protected virtual double getSpeedFromTrip(int speed) {
            return speed / 128.0;
        }
        private void parseNote() {
            if (sections.ContainsKey("Note")) {
                var str = new StringBuilder();
                foreach (var s in sections["Note"]) {
                    str.AppendLine(s);
                }
                Note = str.ToString();
            }
        }
        protected virtual void parseLapNotes() {
            if (sections.ContainsKey("IntNotes")) {
                var lapNotes = sections["IntNotes"];
                foreach (var lapNote in lapNotes) {
                    var pair = lapNote.Split(new char[] { '\t', ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (pair.Length > 1) {
                        var idx = Int32.Parse(pair[0]);
                        if (idx > 0 && idx <= Laps.Count) {
                            Laps[idx - 1].Note = pair[1];
                        }
                    }
                }
            }
        }

        private void parseLaps() {
            Laps = new List<Lap>();
            string[][] lps = lapsDataAsStringArray(sections["IntTimes"]);

            foreach (string[] s in lps) {
                Laps.Add(parseLap(s));
            }
            Laps.Sort(new Lap.TimeComparer());
            var time = StartTime;
            var prevTime = TimeSpan.Zero;
            foreach (var lap in Laps) {
                lap.StartTime = time;
                time = StartTime + lap.Time;
                lap.Duration = lap.Time - prevTime;
                prevTime = lap.Time;
            }
            if (Laps.Last<Lap>().Time < this.Duration) {
                var lap = new Lap();
                lap.Time = this.Duration;
                lap.StartTime = Laps.Last<Lap>().StartTime + Laps.Last<Lap>().Duration;
                lap.Duration = this.StartTime + this.Duration - lap.StartTime;
                Laps.Add(lap);
            }
        }

        protected virtual Lap parseLap(string[] s) {
            var lap = new Lap();
            lap.Time = TimeSpan.Parse(s[0]);
            lap.HR = Int32.Parse(s[1]);
            lap.HRmin = Int32.Parse(s[2]);
            lap.HRavg = Int32.Parse(s[3]);
            lap.HRmax = Int32.Parse(s[4]);
            lap.Speed = Int32.Parse(s[8]) / 10.0;
            lap.Cadence = Int32.Parse(s[9]);
            lap.Altitude = Int32.Parse(s[10]);
            lap.Ascend = Int32.Parse(s[14]);
            lap.Distance = Int32.Parse(s[15]);
            return lap;
        }

        protected virtual void parseParams() {
            StartTime = parseDate(Params["Date"]) + TimeSpan.Parse(Params["StartTime"]);
            Duration = TimeSpan.Parse(Params["Length"]);
            Interval = new TimeSpan(0, 0, Int32.Parse(Params["Interval"]));
            if (Interval.TotalSeconds == 238)
                throw new InvalidFileFormatException("Polar HRM file contains R-R data, not HR");
            parseMode();
            parseUserSettings();

        }

        private string[][] lapsDataAsStringArray(List<string> list) {
            var value = new List<string>();
            foreach (var s in list) {
                value.AddRange(s.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries));
            }
            var retValue = new string[value.Count / LapsDataCount][];
            for (int i = 0, k = 0; k < value.Count; ++i, k += LapsDataCount) {
                retValue[i] = value.GetRange(k, LapsDataCount).ToArray();
            }
            return retValue;
        }

        private void parseUserSettings() {
            UserSettings = new UserData();
            string val;
            if (Params.TryGetValue("Weight", out val)) {
                UserSettings.Weight = Double.Parse(val);
            }
            if (Params.TryGetValue("MaxHR", out val)) {
                UserSettings.MaxHR = Int32.Parse(val);
            }
            if (Params.TryGetValue("RestHR", out val)) {
                UserSettings.RestHR = Int32.Parse(val);
            }
            if (Params.TryGetValue("VO2max", out val)) {
                UserSettings.VO2Max = Double.Parse(val);
            }
            if (Params.TryGetValue("Age", out val)) {
                UserSettings.Age = Int32.Parse(val);
            }
        }

        protected virtual void parseMode() {
            InitModeFlags();
        }
        protected virtual void InitModeFlags() {
            IsCadenceDataAvailable = false;
            IsAltitudeDataAvailable = false;
            IsCyclingDataAvailable = false;
            IsSpeedDataAvailable = false;
            IsPowerDataAvailable = false;
            IsBalanceDataAvailable = false;
            IsPedallingIndexDataAvailable = false;
            IsAirPressureDataAvailable = false;
        }
        public DateTime StartTime { get; set; }
        private static DateTime parseDate(string p) {
            return new DateTime(Int32.Parse(p.Substring(0, 4)),
                Int32.Parse(p.Substring(4, 2)), Int32.Parse(p.Substring(6, 2)));
        }

        public int Version { get; private set; }
        public string GetParam(string s) {
            return Params[s];
        }
        private static IDictionary<string, List<string>> GetSections(string fileName) {
            var sections = new Dictionary<string, List<string>>();
            using (var file = new StreamReader(fileName, Encoding.GetEncoding(0))) {
                var sectionName = "";
                while (!file.EndOfStream) {
                    var line = file.ReadLine().Replace(System.Environment.NewLine, "").Trim();
                    if (line.Length > 2 && line[0] == '[' && line[line.Length - 1] == ']') {
                        sectionName = line.Substring(1, line.Length - 2);
                    }
                    else if (sectionName != "") {
                        if (!sections.ContainsKey(sectionName)) {
                            sections.Add(sectionName, new List<string>());
                        }
                        sections[sectionName].Add(line);
                    }
                }
            }
            return sections;
        }
        private static IDictionary<string, string> GetParams(IEnumerable<string> paramsSection) {
            var pairs = new Dictionary<string, string>();
            foreach (var str in paramsSection) {
                var pair = str.Split(new char[] { '=' }, 2);
                if (pair.Length > 1) {
                    pairs.Add(pair[0], pair[1]);
                }
            }
            return pairs;
        }

        public TimeSpan Duration { get; set; }

        public TimeSpan Interval { get; set; }

        public bool IsCadenceDataAvailable { get; set; }

        public bool IsAltitudeDataAvailable { get; set; }

        public bool IsCyclingDataAvailable { get; set; }

        public bool IsImperialSystemUsed { get; set; }
        public bool IsAirPressureDataAvailable { get; set; }
        public bool IsSpeedDataAvailable { get; set; }

        public bool IsPowerDataAvailable { get; set; }

        public bool IsBalanceDataAvailable { get; set; }

        public bool IsPedallingIndexDataAvailable { get; set; }

        public string Note { get; set; }
        public UserData UserSettings { get; set; }
        public List<Lap> Laps { get; protected set; }

        public TripData Trip { get; set; }

        public class TripData {
            public double Distance { get; set; }

            public double Ascent { get; set; }

            public TimeSpan TotalTime { get; set; }

            public double AvgAltitude { get; set; }

            public double MaxAltitude { get; set; }

            public double AvgSpeed { get; set; }

            public double MaxSpeed { get; set; }

            public double Odometer { get; set; }
        }

        public List<HRDataPoint> HRData { get; set; }
        
        public List<ExerciseData.DataPoint> GetDataPointsInMetricSystem() {
            var DataPoints = new List<ExerciseData.DataPoint>();
            var time = StartTime;
            foreach (var point in HRData) {
                DataPoints.Add(new ExerciseData.DataPoint() { 
                    Time = time,
                    HeartRate = point.HeartRate,
                    Speed = getKilometers(point.Speed),
                    Cadence = point.Cadence,
                    Altitude = getMeters(point.Altitude),
                    Power = point.Power,
                    PowerBalance = point.PowerBalance,
                    AirPressure = point.AirPressure,
                    Distance = getKilometers(point.Distance)
                });
                time += Interval;
            }
            return DataPoints;
        }
        public double getKilometers(double p) {
            if (IsImperialSystemUsed) {
                return Utility.MilesToKilometers(p);
            }
            return p;
        }
        public double getMeters(double p) {
            if (IsImperialSystemUsed) {
                return Utility.FeetsToMeters(p);
            }
            return p;
        }

        public TripData GetTripDataInMetricSystem() {
            return new TripData() {
                Distance = getKilometers(Trip.Distance),
                Ascent = getMeters(Trip.Ascent),
                TotalTime = Trip.TotalTime,
                AvgAltitude = getMeters(Trip.AvgAltitude),
                AvgSpeed = getKilometers(Trip.AvgSpeed),
                MaxSpeed = getKilometers(Trip.MaxSpeed),
                Odometer = getKilometers(Trip.Odometer)
            };
        }

        public List<Lap> GetLapsInMetricSystem() {
            var laps = new List<Lap>();
            foreach (var lap in Laps) {
                laps.Add(new Lap() { 
                    Time = lap.Time,
                    StartTime = lap.StartTime,
                    Duration = lap.Duration,
                    HR = lap.HR,
                    HRavg = lap.HRavg,
                    HRmax = lap.HRmax,
                    HRmin = lap.HRmin,
                    Speed = getKilometers(lap.Speed),
                    Cadence = lap.Cadence,
                    Altitude = getMeters(lap.Altitude),
                    Ascend = getMeters(lap.Ascend),
                    Distance = getKilometers(lap.Distance),
                    Power = lap.Power,
                    Temperature = getCelsius(lap.Temperature),
                    Note = lap.Note,
                    AutomaticLap = lap.AutomaticLap
                });
            }
            return laps;
        }

        private double getCelsius(double p) {
            if (IsImperialSystemUsed) {
                return Utility.FahrenheitToCelsius(p);
            }
            return p;
        }
    }
}
