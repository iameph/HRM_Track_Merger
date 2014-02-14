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
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            try {
                CommandLineArguments cmdArgs = null;
                try {
                    cmdArgs = CommandLineArguments.Parse(args);
                }
                catch (InvalidArgumentsException e) {
                    throw new Exception("Incorrect arguments! Execute program with /? key for help\n" + e.Message);
                }
                catch (Exception) {
                    throw new Exception("Something wrong with arguments, please contact authors");
                }
                if (cmdArgs == null)
                    return 1;

                if (cmdArgs.HRMFileName == null) {
                    throw new Exception("No HRM file in arguments or file doesn't exist");
                }
                var hrmFile = PolarHRM.PolarHRMFile.Parse(cmdArgs.HRMFileName);
                var exercise = new ExerciseData.CommonExerciseData(hrmFile);
                ExerciseData.UserData user = ParseUserOptions(cmdArgs.GetOptions());
                exercise.UpdateUserData(user, false);
                if (cmdArgs.GPSFileName != null) {
                    TimeSpan offset = TimeSpan.Zero;
                    if (cmdArgs.GetOptions().ContainsKey("offset")) {
                        offset = new TimeSpan(0, 0, Int32.Parse(cmdArgs.GetOptions()["offset"]));
                    }
                    exercise.AddGPSData(new GPXFile(cmdArgs.GPSFileName),offset);
                }
                exercise.UpdateCaloriesData();
                var tcxFile = exercise.ConvertToTCX();
                if (cmdArgs.GetOptions().ContainsKey("sport")) {
                    GarminTCX.Sport sport;
                    switch (cmdArgs.GetOptions()["sport"].ToLower()) {
                        case "biking" : 
                            sport = GarminTCX.Sport.Biking;
                            break;
                        case "running":
                            sport = GarminTCX.Sport.Running;
                            break;
                        case "other":
                            sport = GarminTCX.Sport.Other;
                            break;
                        default:
                            sport = GarminTCX.Sport.Other;
                            Console.WriteLine("Incorrect sport name, use Other instead");
                            break;
                    }
                    tcxFile.SetSport(0, sport);
                }
                string outputFileName;
                if (cmdArgs.GetOptions().ContainsKey("output")) {
                    outputFileName = cmdArgs.GetOptions()["output"];
                }
                else {
                    outputFileName = GenerateOutputFileName(cmdArgs.HRMFileName,cmdArgs.GPSFileName);
                }
                tcxFile.Save(outputFileName, new System.Xml.XmlWriterSettings() { Indent=true, IndentChars="\t"});
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return 1;
            }
            return 0;
        }

        private static string GenerateOutputFileName(string hrmFileName, string gpsFileName) {
            StringBuilder name = new StringBuilder();
            name.Append(Path.GetFileNameWithoutExtension(hrmFileName));
            if(gpsFileName!=null){
                name.Append("_Merge");
            }
            name.Append("_");
            var now = DateTime.Now;
            name.Append(String.Format("{0:D4}-{1:D2}-{2:D2}_{3:D2}-{4:D2}-{5:D2}",
                now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second));
            name.Append(".tcx");
            return name.ToString();
        }

        private static ExerciseData.UserData ParseUserOptions(Dictionary<string, string> dictionary) {
            var result = new ExerciseData.UserData();
            if (dictionary.ContainsKey("weight")) {
                try {
                    result.Weight = Double.Parse(dictionary["weight"]);
                }
                catch (Exception) {
                    throw new InvalidArgumentsException("Can't parse weight");
                }
            }
            if (dictionary.ContainsKey("age")) {
                try {
                    result.Age = Double.Parse(dictionary["age"]);
                }
                catch (Exception) {
                    throw new InvalidArgumentsException("Can't parse age");
                }
            }
            if (dictionary.ContainsKey("vo2max")) {
                try {
                    result.VO2Max = Int32.Parse(dictionary["vo2max"]);
                }
                catch (Exception) {
                    throw new InvalidArgumentsException("Can't parse vo2max");
                }
            }
            if (dictionary.ContainsKey("sex")) {
                switch (dictionary["sex"].ToLower()) {
                    case "male":
                        result.Sex = ExerciseData.Sex.Male;
                        break;
                    case "female":
                        result.Sex = ExerciseData.Sex.Female;
                        break;
                    default:
                        Console.WriteLine("Incorrect sex, use male instead");
                        result.Sex = ExerciseData.Sex.Male;
                        break;
                }
            }
            return result;
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
                "Usage: "+name+ ".exe <hrm file> [<gpx file>] [/output:<output_file_name>] [/offset:<offset>] [/sport:<sport>] [/sex:<sex>] [/age:<age>] [/weight:<weight>] [/vo2max:<vo2max>]",
                "where:",
                "<hrm file> - path to hrm file, currently Polar HRM(.hrm) supported",
                "<gpx file> - path to gpx file",
                "<output_file_name> - path to output file" ,
                "<offset> - offset in seconds to add to GPS time",
                "<sport> - Biking, Running or Other(default)",
                "",
                "Optional data for calories calculation (Caution! It will replace data from HRM!):",
                "<sex> - male or female",
                "<age> - age in years (floating-point number is OK, use dot as separator)",
                "<weight> - weight in KILOGRAMS (floating-point number is OK, use dot as separator)",
                "<vo2max> - ml/kg/min"
            }));
        }

        private static void PrintInputArguments(TimeSpan offset, DateTime startTime, TimeSpan duration, TimeSpan step) {
            Console.WriteLine("Input arguments:\n" + "Offset:{0}\n" + "Start Time:{1}\n" + "Duration:{2}\n" + "Step:{3}", offset, startTime, duration, step);
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
