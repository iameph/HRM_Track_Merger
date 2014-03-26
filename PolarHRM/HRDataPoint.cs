using System;

namespace HRM_Track_Merger.PolarHRM {
    class HRDataPoint {
        public int HeartRate { get; set; }
        public double Speed { get; set; }
        public int Cadence { get; set; }
        public double Altitude { get; set; }
        public int Power { get; set; }
        public int PowerBalance { get; set; }
        public int AirPressure { get; set; }
        public DateTime Time { get; set; }
        public double Distance { get; set; }
    }
}
