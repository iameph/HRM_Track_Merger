
namespace HRM_Track_Merger.ExerciseData {
    class Summary {
        public DateTimeRange Time { get; set; }
        public Range<double> Speed { get; set; }
        public Range<double> HeartRate { get; set; }
        public Range<double> Cadence { get; set; }
        public Range<double> Altitude { get; set; }
        public Range<double> Power { get; set; }
        public Range<double> Temperature { get; set; }
        public double Ascent { get; set; }
        public double Distance { get; set; }
        public double Calories { get; set; }
        public string Note { get; set; }
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
