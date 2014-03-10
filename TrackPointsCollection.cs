using System;
using System.Collections.Generic;

namespace HRM_Track_Merger {
    class TrackPointsCollection {
        
        public List<TrackPoint> TrackPoints { get; set; }
        public TrackPointsCollection(List<TrackPoint> trackPoints) {
            TrackPoints = new List<TrackPoint>(trackPoints);
            TrackPoints.Sort();
        }
        public TrackPoint GetTrackPointAtTime(DateTime time){
            var searchPoint = new TrackPoint();
            searchPoint.Time = time;
            var index = TrackPoints.BinarySearch(searchPoint, new TrackPointTimeComparer());
            if (index >= 0 && index < TrackPoints.Count) {
                return (TrackPoint)TrackPoints[index].Clone();
            }
            if (~index == TrackPoints.Count) {
                var returnPoint = TrackPoints[TrackPoints.Count - 1];
                returnPoint.Time = time;
                return (TrackPoint)returnPoint.Clone();
            }
            if (~index == 0) {
                var returnPoint = TrackPoints[0];
                returnPoint.Time = time;
                return (TrackPoint)returnPoint.Clone();
            }
            return InterpolatePoint(~index,time);
        }
        private TrackPoint InterpolatePoint(int index,DateTime time){
            TrackPoint returnPoint = new TrackPoint();
            returnPoint.Time = time;
            returnPoint.Latitude = Interpolate(
                TrackPoints[index - 1].Time.Ticks, TrackPoints[index].Time.Ticks,
                TrackPoints[index - 1].Latitude, TrackPoints[index].Latitude,
                time.Ticks);
            returnPoint.Longitude = Interpolate(
                TrackPoints[index - 1].Time.Ticks, TrackPoints[index].Time.Ticks,
                TrackPoints[index - 1].Longitude, TrackPoints[index].Longitude,
                time.Ticks);
            returnPoint.Elevation = Interpolate(
                TrackPoints[index - 1].Time.Ticks, TrackPoints[index].Time.Ticks,
                TrackPoints[index - 1].Elevation, TrackPoints[index].Elevation,
                time.Ticks);
            return returnPoint;
        }
        private double Interpolate(double x, double y, double a, double b, double targetCoord) {
            return (targetCoord - x) * (b - a) / (y - x) + a;
        }
        class TrackPointTimeComparer : IComparer<TrackPoint> {
            public int Compare(TrackPoint x, TrackPoint y) {
                return DateTime.Compare(x.Time, y.Time);
            }
        }
    }
}
