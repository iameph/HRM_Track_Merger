using System;

namespace HRM_Track_Merger {
    public class DateTimeRange {
        public DateTimeRange(DateTime start, DateTime end){
            Start = start;
            End = end;
            Duration = end - start;
        }
        public DateTimeRange(DateTime start, TimeSpan duration) {
            Start = start;
            Duration = duration;
            End = Start + Duration;
        }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public TimeSpan Duration { get; private set; }
    }
}
