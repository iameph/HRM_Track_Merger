using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.ExerciseData {
    class UserData {
        public double? Weight { get; set; }
        public int? MaxHR { get; set; }
        public int? RestHR { get; set; }
        public double? VO2Max { get; set; }
        public int? Age { get; set; }
        public Gender? Gender;
    }
}
