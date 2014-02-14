using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger {
    class Program {
        static int Main(string[] args) {
            if (args.Length == 0 || args.Contains("/?") || args.Contains("-h") || args.Contains("--h)")) {
                ShowUsageInfo();
                return 0;
            }
            return 0;
        }

        private static void ShowUsageInfo() {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            string name = "HRM_Track_Merger";
            Console.WriteLine(String.Join("\n", new string[] { 
                name + " version: " + version,
                "Software to merge heartrate monitor data with gps data to TCX XML format",
                "",
                "Usage: "+name+ ".exe <hrm file> [<gpx file>] [/output:<output_file_name>] [/offset:<offset>] [/age:<age>] [/weight:<weight>] [/vo2max:<vo2max>]",
                "where:",
                "<hrm file> - path to hrm file, currently Polar HRM(.hrm) supported",
                "<gpx file> - path to gpx file",
                "<output_file_name> - path to output file" ,
                "<offset> - offset in seconds to add to GPS time",
                "",
                "Optional data for calories calculation (Caution! It will replace data from HRM!):",
                "<age> - age in years",
                "<weight> - weight in KILOGRAMS (floating-point number is OK, use dot as separator)",
                "<vo2max> - ml/kg/min"
            }));
        }

        private static void PrintInputArguments(TimeSpan offset, DateTime startTime, TimeSpan duration, TimeSpan step) {
            Console.WriteLine("Input arguments:\n" + "Offset:{0}\n" + "Start Time:{1}\n" + "Duration:{2}\n" + "Step:{3}", offset, startTime, duration, step);
        }

        private static void PrintInputArguments(CommandLineArguments args) {
            PrintInputArguments(args.Offset, args.StartTime, args.Duration, args.Step);
        }
        private static string CreateCorrectedFilename(string fileName) {
            return CreateCorrectedFilename(fileName, "_corrected");
        }
        public static string CreateCorrectedFilename(string fileName, string add) {
            var info = new FileInfo(fileName);
            return info.DirectoryName + (info.DirectoryName.EndsWith("\\") ? "" : "\\") + Path.GetFileNameWithoutExtension(info.Name) + add + info.Extension;
        }
    }
}
