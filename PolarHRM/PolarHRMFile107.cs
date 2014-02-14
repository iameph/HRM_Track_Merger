using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.PolarHRM {
    class PolarHRMFile107 : PolarHRMFile106 {
        protected override void parseMode() {
            base.parseMode();
            if (Params["SMode"][8] == '1') {
                IsAirPressureDataAvailable = true;
            }
        }
        protected override void parseHRDataValues(List<int> values, HRDataPoint data) {
            base.parseHRDataValues(values, data);
            if (IsAirPressureDataAvailable) {
                data.AirPressure = values[0];
                values.RemoveAt(0);
            }
        }
    }
}
