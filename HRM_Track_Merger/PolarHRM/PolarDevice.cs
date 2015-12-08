using System.Collections.Generic;

namespace HRM_Track_Merger.PolarHRM {
    class PolarDevice {
        private static readonly IDictionary<int,string> Devices;
        static PolarDevice(){
            Devices = new Dictionary<int,string>();
            Devices.Add(1, "Polar Sport Tester / Vantage XL");
            Devices.Add(2, "Polar Vantage NV (VNV)");
            Devices.Add(3, "Polar Accurex Plus");
            Devices.Add(4, "Polar XTrainer Plus");
            Devices.Add(6, "Polar S520");
            Devices.Add(7, "Polar Coach");
            Devices.Add(8, "Polar S210");
            Devices.Add(9, "Polar S410");
            Devices.Add(10, "Polar S510");
            Devices.Add(11, "Polar S610 / S610i");
            Devices.Add(12, "Polar S710 / S710i / S720i");
            Devices.Add(13, "Polar S810 / S810i");
            Devices.Add(15, "Polar E600");
            Devices.Add(20, "Polar AXN500");
            Devices.Add(21, "Polar AXN700");
            Devices.Add(22, "Polar S625X / S725X");
            Devices.Add(23, "Polar S725");
            Devices.Add(33, "Polar CS400");
            Devices.Add(34, "Polar CS600X");
            Devices.Add(35, "Polar CS600");
            Devices.Add(36, "Polar RS400");
            Devices.Add(37, "Polar RS800");
            Devices.Add(38, "Polar RS800X");
        }
        public string GetDevice(int id) {
            if (Devices.ContainsKey(id)) {
                return Devices[id];
            }
            return "Unknown";
        }

    }
}