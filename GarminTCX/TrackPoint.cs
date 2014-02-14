using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class TrackPoint {
        public DateTime Time;
        public Position Position;
        public double? AltitudeMeters;
        public double? DistanceMeters;
        public HeartRate HeartRateBpm;
        public byte? Cadence;
        public TrackPointExtension Extension;
    }
}
