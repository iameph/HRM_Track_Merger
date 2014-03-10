using System;

namespace HRM_Track_Merger {
    class TrackPoint : IComparable<TrackPoint>,ICloneable {
        public DateTime Time { get; set; }
        public double Elevation { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int CompareTo(TrackPoint other) {

            return this.Time.CompareTo(other.Time);
        }
        public TrackPoint() { }
        public TrackPoint(DateTime time, double longitude, double latitude, double elevation) {
            Time = time;
            Longitude = longitude;
            Latitude = latitude;
            Elevation = elevation;
        }
        public override string ToString() {
            return "TrackPoint time: " + Time.ToString() + " longitude: " + Longitude + " latitude: " + Latitude + " elevation: " + Elevation;
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
        public override bool Equals(object obj) {
            if (!(obj is TrackPoint)) {
                return false;
            }
            TrackPoint rhs = (TrackPoint)obj;
            return Time == rhs.Time && Elevation == rhs.Elevation && Latitude == rhs.Latitude && Longitude == rhs.Longitude;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
