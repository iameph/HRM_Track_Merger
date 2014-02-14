using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.ExerciseData {
    class Summary {
        public DateTimeRange Time;
        public Range<double> Speed;
        public Range<double> HeartRate;
        public Range<double> Cadence;
        public Range<double> Altitude;
        public Range<double> Power;
        public Range<double> Temperature;
        public double Ascent;
        public double Distance;
        public double Calories;
    }
}
