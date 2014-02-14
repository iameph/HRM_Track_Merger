using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.GarminTCX {
    class Activity {
        public Sport Sport = Sport.Other;
        public DateTime Id;
        public List<Lap> Laps;
        public string? Notes;
        public Creator Creator;
    }
}
