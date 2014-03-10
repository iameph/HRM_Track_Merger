
namespace HRM_Track_Merger {
    class Range<T> {
        public T Min { get; set; }
        public T Max { get; set; }
        public T Avg { get; set; }
        public Range(T min, T avg, T max) {
            Min = min;
            Max = max;
            Avg = avg;
        }
        public Range(T value) {
            Min = Avg = Max = value;
        }
        public Range() {
        }
    }
}
