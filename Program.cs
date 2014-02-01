using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("HRM_Track_Merger.Test")]

namespace HRM_Track_Merger {
    class Program {
        static int Main(string[] args) {
            CommandLineArguments Arguments;
            try {
                Arguments = CommandLineArguments.Parse(args);
            }
            catch (InvalidArgumentsException e) {
                Console.WriteLine(e.Message);
                ShowUsageInfo();
                return 0;
            }
            Arguments.FixTimeForTCXCreator();
            PrintInputArguments(Arguments);

            var gpxFile = new GPXFile(Arguments.FileName);
            var trackPoints = new TrackPointsCollection(gpxFile.GetTrackPoints(Arguments.Offset));
            Arguments.SetDateForStartTime(trackPoints.TrackPoints[0].time);

            var newGPXFile = new GPXFile();
            newGPXFile.CreateNewSegment();

            for (DateTime curTime = Arguments.StartTime; curTime <= Arguments.StartTime + Arguments.Duration; curTime = curTime.Add(Arguments.Step)) {
                newGPXFile.AddTrackPoint(trackPoints.GetTrackPointAtTime(curTime));
            }

            newGPXFile.File.Save(CreateCorrectedFilename(Arguments.FileName));
            return 0;
        }

        private static void ShowUsageInfo() {
            Console.WriteLine(String.Join("\n", new string[] { 
                "Usage: HRM_Track_Merger.exe <gpx file> <offset> <starttime> <duration> <step>",
                "\tor: HRM_Track_Merger.exe <gpx file> <hrm file> [<offset>]",
                "where:",
                "<gpx file> - path to gpx file",
                "<offset> - time in seconds that gpx offseted from hrm",
                "<starttime> - time in format \"HH:mm:ss\"",
                "<duration> - time in format \"HH:mm:ss\"",
                "<step> - interval between measurements in hrm file",
                "<hrm file> - path to hrm file for collecting time settings"
            }));
        }

        private static void PrintInputArguments(TimeSpan offset, DateTime startTime, TimeSpan duration, TimeSpan step) {
            Console.WriteLine("Input arguments:\n" + "Offset:{0}\n" + "Start Time:{1}\n" + "Duration:{2}\n" + "Step:{3}", offset, startTime, duration, step);
        }

        private static void PrintInputArguments(CommandLineArguments args) {
            PrintInputArguments(args.Offset, args.StartTime, args.Duration, args.Step);
        }

        private static string CreateCorrectedFilename(string fileName) {
            return Path.GetDirectoryName(fileName) + Path.GetFileNameWithoutExtension(fileName) + "_corrected" + Path.GetExtension(fileName);
        }
    }
}
