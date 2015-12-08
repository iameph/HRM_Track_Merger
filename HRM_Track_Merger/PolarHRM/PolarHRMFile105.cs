using System.Collections.Generic;

namespace HRM_Track_Merger.PolarHRM {
    class PolarHRMFile105 : PolarHRMFile102 {
        protected override int getAltitudeFromTrip(int ascent) {
            return ascent;
        }
        protected override double getSpeedFromTrip(int speed) {
            return speed / 10.0;
        }
        protected override void parseHRDataValues(List<int> values, HRDataPoint data) {
            base.parseHRDataValues(values, data);
            if (IsSpeedDataAvailable) {
                data.Speed = getSpeedFromTrip(values[0]);
                values.RemoveAt(0);
            }
            if (IsCadenceDataAvailable) {
                data.Cadence = values[0];
                values.RemoveAt(0);
            }
            if (IsAltitudeDataAvailable) {
                data.Altitude = values[0];
                values.RemoveAt(0);
            }
            
        }
    }
}
