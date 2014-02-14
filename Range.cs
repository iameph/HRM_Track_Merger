using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger {
    struct Range<T> {
        public T Min;
        public T Max;
        public T Avg;
        public Range(T min, T avg, T max) {
            Min = min;
            Max = max;
            Avg = avg;
        }
    }
}
