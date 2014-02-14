using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.PolarHRM {
    class UserData : ICloneable {
        public double? Weight;
        public int? MaxHR;
        public int? RestHR;
        public double? VO2Max;
        public int? Age;
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
