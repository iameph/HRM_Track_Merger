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
            searchPoint.time = time;
            var index = TrackPoints.BinarySearch(searchPoint, new TrackPointTimeComparer());
            if (index >= 0 && index < TrackPoints.Count) {
                return TrackPoints[index];
            }
            if (~index == TrackPoints.Count) {
                var returnPoint = TrackPoints[TrackPoints.Count - 1];
                returnPoint.time = time;
                return returnPoint;
            }
            if (~index == 0) {
                var returnPoint = TrackPoints[0];
                returnPoint.time = time;
                return returnPoint;
            }
            return InterpolatePoint(~index,time);
        }
        private TrackPoint InterpolatePoint(int index,DateTime time){
            TrackPoint returnPoint;
            returnPoint.time = time;
            returnPoint.latitude = Interpolate(
                TrackPoints[index - 1].time.Ticks, TrackPoints[index].time.Ticks,
                TrackPoints[index - 1].latitude, TrackPoints[index].latitude,
                time.Ticks);
            returnPoint.longitude = Interpolate(
                TrackPoints[index - 1].time.Ticks, TrackPoints[index].time.Ticks,
                TrackPoints[index - 1].longitude, TrackPoints[index].longitude,
                time.Ticks);
            returnPoint.elevation = Interpolate(
                TrackPoints[index - 1].time.Ticks, TrackPoints[index].time.Ticks,
                TrackPoints[index - 1].elevation, TrackPoints[index].elevation,
                time.Ticks);
            return returnPoint;
        }
        private double Interpolate(double x, double y, double a, double b, double targetCoord) {
            return (targetCoord - x) * (b - a) / (y - x) + a;
        }
        class TrackPointTimeComparer : IComparer<TrackPoint> {
            public int Compare(TrackPoint x, TrackPoint y) {
                return DateTime.Compare(x.time, y.time);
            }
        }
    }
}
