using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class Lap {
        public DateTime StartTime;
        public double TotalTimeSeconds;
        public double DistanceMeters;
        public double? MaximumSpeed;
        public uint Calories;
        public HeartRate AverageHeartRateBpm;
        public HeartRate MaximumHeartRateBpm;
        public string Intesity;
        public byte? Cadence;
        public string TriggerMethod;
        public List<TrackPoint> Track;
        public string? Notes;
        public LapExtension Extension;
    }
}
