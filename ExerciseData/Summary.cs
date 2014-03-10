
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
        public string Note;
        public Summary() {
            Time = new DateTimeRange(System.DateTime.Today, System.DateTime.Today);
            Speed = new Range<double>();
            HeartRate = new Range<double>();
            Cadence = new Range<double>();
            Altitude = new Range<double>();
            Power = new Range<double>();
            Temperature = new Range<double>();
        }
    }
}
