using System;

namespace HRM_Track_Merger.ExerciseData {
    class DataPoint : IComparable<DataPoint>, ICloneable {
        public double HeartRate { get; set; }
        public double Speed { get; set; }
        public double Cadence { get; set; }
        public double Altitude { get; set; }
        public double Power { get; set; }
        public double PowerBalance { get; set; }
        public double AirPressure { get; set; }
        public DateTime Time { get; set; }
        public double Distance { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public int CompareTo(DataPoint other) {
            return Time.CompareTo(other.Time);
        }

        public object Clone() {
            return MemberwiseClone();
        }
        public static DataPoint Interpolate(DataPoint dataPoint1, DataPoint dataPoint2, DateTime time) {
            double x1 = 0;
            double x2 = (dataPoint2.Time - dataPoint1.Time).TotalSeconds;
            double x = (time - dataPoint1.Time).TotalSeconds;
            return new DataPoint() {
                AirPressure = Interpolate(x1, x2, dataPoint1.AirPressure, dataPoint2.AirPressure, x),
                Altitude = Interpolate(x1, x2, dataPoint1.Altitude, dataPoint2.Altitude, x),
                Cadence = Interpolate(x1, x2, dataPoint1.Cadence, dataPoint2.Cadence, x),
                Distance = Interpolate(x1, x2, dataPoint1.Distance, dataPoint2.Distance, x),
                HeartRate = Interpolate(x1, x2, dataPoint1.HeartRate, dataPoint2.HeartRate, x),
                Latitude = Interpolate(x1, x2, dataPoint1.Latitude, dataPoint2.Latitude, x),
                Longitude = Interpolate(x1, x2, dataPoint1.Longitude, dataPoint2.Longitude, x),
                Power = Interpolate(x1, x2, dataPoint1.Power, dataPoint2.Power, x),
                PowerBalance = Interpolate(x1, x2, dataPoint1.PowerBalance, dataPoint2.PowerBalance, x),
                Speed = Interpolate(x1, x2, dataPoint1.Speed, dataPoint2.Speed, x),
                Time = time
            };
        }
        public static double Interpolate(double x1, double x2, double y1, double y2, double x) {
            if (x <= x1) {
                return y1;
            }
            if (x >= x2) {
                return y2;
            }
            return (x - x1) * (y2 - y1) / (x2 - x1) + y1;
        }
    }
}
