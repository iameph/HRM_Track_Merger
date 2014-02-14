using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.PolarHRM {
    class PolarHRMFile106 : PolarHRMFile105 {
        protected override int LapsDataCount {
            get {
                return 28;
            }
        }
        protected override void parseMode() {
            InitModeFlags();
            string modeString = Params["SMode"];
            if (modeString[0] == '1') {
                IsSpeedDataAvailable = true;
            }
            if (modeString[1] == '1') {
                IsCadenceDataAvailable = true;
            }
            if (modeString[2] == '1') {
                IsAltitudeDataAvailable = true;
            }
            if (modeString[3] == '1') {
                IsPowerDataAvailable = true;
            }
            if (modeString[4] == '1') {
                IsBalanceDataAvailable = true;
            }
            if (modeString[5] == '1') {
                IsPedallingIndexDataAvailable = true;
            }
            if (modeString[6] == '1') {
                IsCyclingDataAvailable = true;
            }
            if (modeString[7] == '1') {
                IsImperialSystemUsed = true;
            }
        }
        protected override Lap parseLap(string[] s) {
            var lap = base.parseLap(s);
            lap.Distance = Int32.Parse(s[17]) / (double)(IsImperialSystemUsed ? 1760 : 1000);
            lap.Power = Int32.Parse(s[18]);
            lap.Temperature = Int32.Parse(s[19]) / 10.0;
            lap.AutomaticLap = Int32.Parse(s[23]) == 1;
            return lap;
        }
        protected override void parseHRDataValues(List<int> values, HRDataPoint data) {
            base.parseHRDataValues(values, data);
            if (IsPowerDataAvailable) {
                data.Power = values[0];
                values.RemoveAt(0);
            }
            if (IsBalanceDataAvailable) {
                data.PowerBalance = values[0];
                values.RemoveAt(0);
            }
        }
    }
}
