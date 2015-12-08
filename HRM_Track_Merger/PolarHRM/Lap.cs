using System;
using System.Collections.Generic;

namespace HRM_Track_Merger.PolarHRM {
    class Lap {
        public TimeSpan Time { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int HR { get; set; }
        public int HRavg { get; set; }
        public int HRmax { get; set; }
        public int HRmin { get; set; }
        public double Speed { get; set; }
        public double Cadence { get; set; }
        public double Altitude { get; set; }
        public double Ascend { get; set; }
        public double Distance { get; set; }
        public double Power { get; set; }
        public double Temperature { get; set; }
        public string Note { get; set; }
        public bool AutomaticLap { get; set; }
        public Lap() {
            Note = "";
        }
        public class TimeComparer : IComparer<Lap> {
            public int Compare(Lap x, Lap y) {
                return x.Time.CompareTo(y.Time);
            }
        }
    }
}
