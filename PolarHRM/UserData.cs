using System;

namespace HRM_Track_Merger.PolarHRM {
    class UserData : ICloneable {
        public double? Weight { get; set; }
        public int? MaxHR { get; set; }
        public int? RestHR { get; set; }
        public double? VO2Max { get; set; }
        public int? Age { get; set; }

        public object Clone() {
            return new UserData() {
                Weight = this.Weight,
                MaxHR = this.MaxHR,
                RestHR = this.RestHR,
                VO2Max = this.VO2Max,
                Age = this.Age
            };
        }
    }
}
