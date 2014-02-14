using System;
using System.Collections.Generic;
using System.Linq;

namespace HRM_Track_Merger.PolarHRM {
    class PolarHRMFile102 : PolarHRMFile {
        protected override void parseMode() {
            base.parseMode();
            string modeString = Params["Mode"];
            switch (modeString[0]) {
                case '0':
                    IsCadenceDataAvailable = true;
                    break;
                case '1':
                    IsAltitudeDataAvailable = true;
                    break;
                default:
                    break;
            }
            if (modeString[1] == '1') {
                IsCyclingDataAvailable = true;
            }
            if (modeString[2] == '1') {
                IsImperialSystemUsed = true;
            }
        }
        protected override HRDataPoint parseHRDataString(string HRDataString) {
            var result = new HRDataPoint();
            var values = SplitStringToInts(HRDataString);
            if (values.Count > 0) {
                parseHRDataValues(values,result);
            }
            return result;
        }

        protected virtual void parseHRDataValues(List<int> values, HRDataPoint data) {
           data.HeartRate = values[0];
           values.RemoveAt(0);
        }
        protected List<int> SplitStringToInts(string str) {
            return str.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select((strVal) => Int32.Parse(strVal)).ToList();
        }
    }
}
