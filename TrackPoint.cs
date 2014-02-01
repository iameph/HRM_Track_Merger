using System;

namespace HRM_Track_Merger {
    struct TrackPoint : IComparable<TrackPoint> {
        public DateTime time;
        public double longitude, latitude, elevation;

        public int CompareTo(TrackPoint other) {

            return this.time.CompareTo(other.time);
        }
        
        public TrackPoint(DateTime time, double longitude, double latitude, double elevation) {
            this.time = time;
            this.longitude = longitude;
            this.latitude = latitude;
            this.elevation = elevation;
        }
        public override string ToString() {
            return "TrackPoint time: " + time.ToString() + " longitude: " + longitude + " latitude: " + latitude + " elevation: " + elevation;
        }
    }
}
