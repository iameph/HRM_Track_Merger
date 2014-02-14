using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.ExerciseData {
    class DataPoint : IComparable<DataPoint>, ICloneable{
        public double HeartRate;
        public double Speed;
        public double Cadence;
        public double Altitude;
        public double Power;
        public double PowerBalance;
        public double AirPressure;
        public DateTime Time;
        public double Distance;
        public double Longitude;
        public double Latitude;

        public int CompareTo(DataPoint other) {
            return Time.CompareTo(other.Time);
        }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}
