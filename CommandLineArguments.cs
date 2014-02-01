using System;
using System.Collections.Generic;
using System.IO;

namespace HRM_Track_Merger {
    class CommandLineArguments {
        protected CommandLineArguments(string[] args) {
            if (args.Length < 2) {
                throw new InvalidArgumentsException("Wrong arguments");
            }
            if (isFileName(args[0]) && isFileName(args[1])) {
                if (Path.GetExtension(args[0]).Equals(".gpx")) {
                    GetTimeFromFile(args[1]);
                    FileName = args[0];
                }
                else {
                    GetTimeFromFile(args[0]);
                    FileName = args[1];
                }
                if (args.Length > 2) {
                    Offset = SecondsToTimeSpan(args[2]);
                }
            }
            else if (args.Length < 5) {
                throw new InvalidArgumentsException("Wrong arguments");
            }
            else {
                FileName = args[0];
                Offset = SecondsToTimeSpan(args[1]);
                StartTime = DateTime.Parse(args[2]);
                Duration = TimeSpan.Parse(args[3]);
                Step = SecondsToTimeSpan(args[4]);
            }
            Duration = Duration.Subtract(TimeSpan.FromMilliseconds(Duration.Milliseconds));
            StartTime = StartTime.Subtract(TimeSpan.FromMilliseconds(StartTime.Millisecond));
        }
        private TimeSpan SecondsToTimeSpan(string p) {
            return new TimeSpan(0, 0, Int32.Parse(p));
        }

        private void GetTimeFromFile(string fileName) {
            var Parser = HRMParser.GetParser(fileName);
            var paramsSection = Parser.GetSection("Params");
            var pairs = new Dictionary<string, string>();
            foreach (var str in paramsSection) {
                var pair = str.Split(new char[] { '=' }, 2);
                if (pair.Length > 1) {
                    pairs.Add(pair[0], pair[1]);
                }
            }
            try {
                StartTime = GetDateFromString(pairs["Date"]) + TimeSpan.Parse(pairs["StartTime"]);
                Duration = TimeSpan.Parse(pairs["Length"]);
                Step = SecondsToTimeSpan(pairs["Interval"]);
            }
            catch (Exception e) {
                throw new InvalidFileFormatException(e.Message);
            }
        }

        private bool isFileName(string name) {
            return File.Exists(name);
        }
        #region Properties
        public string FileName { get; set; }
        public TimeSpan Offset { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Step { get; set; }
        #endregion
        public void FixTimeForTCXCreator() {
            Offset = Offset.Add(Step);
            StartTime = StartTime.Add(Step);
        }
        public static CommandLineArguments Parse(string[] args) {

            return new CommandLineArguments(args);
        }
        public void SetDateForStartTime(DateTime date) {
            StartTime = new DateTime(date.Year, date.Month, date.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
        }
        private DateTime GetDateFromString(string str) {
            return new DateTime(Int32.Parse(str.Substring(0, 4)), Int32.Parse(str.Substring(4, 2)),
                                        Int32.Parse(str.Substring(6, 2)));
        }
    }
}
