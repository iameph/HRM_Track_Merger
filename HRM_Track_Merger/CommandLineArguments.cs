using System;
using System.Collections.Generic;
using System.IO;

namespace HRM_Track_Merger {
    public class CommandLineArguments {
        private List<string> hrmExtensions= new List<string>(new string[]{".hrm",".xml"});
        private List<string> gpsExtensions = new List<string>(new string[] { ".gpx" });
        private Dictionary<string, string> options = new Dictionary<string,string>();
        protected CommandLineArguments(string[] args) {
            if (args.Length == 0) {
                throw new InvalidArgumentsException("No arguments");
            }
            foreach (var arg in args) {
                if (isFileName(arg)) {
                    if (hrmExtensions.Contains(Path.GetExtension(arg))) {
                        if (HRMFileName != null)
                            throw new InvalidArgumentsException(String.Format(
                                "Two HRM files in input: {0} and {1}. Please check data.", HRMFileName, arg));
                        HRMFileName = arg;
                    }
                    else if (gpsExtensions.Contains(Path.GetExtension(arg))) {
                        if (GPSFileName != null)
                            throw new InvalidArgumentsException(String.Format(
                                "Two GPS files in input: {0} and {1}. Please check data.", GPSFileName, arg));
                        GPSFileName = arg;
                    }
                    else {
                        throw new InvalidArgumentsException(String.Format(
                                "Unknown type of file {0}. Please check data.", arg));
                    }
                }
                else if (IsOption(arg)) {
                    AddOption(arg);
                }
                else {
                    throw new InvalidArgumentsException(String.Format("Wrong argument: {0}", arg));
                }
            }
        }

        private void AddOption(string arg) {
            var m = System.Text.RegularExpressions.Regex.Match(arg, @"/([0-9a-zA-Z]+):(.+)");
            var key = m.Groups[1].Value;
            var val = m.Groups[2].Value;
            val = val.Trim(new char[] { ' ', '\t', '"' });
            if (options.ContainsKey(key)) {
                throw new InvalidArgumentsException("Two option arguments with same name: " + key + ". Please check data.");
            }
            options.Add(key, val);
        }

        internal bool IsOption(string arg) {
            return System.Text.RegularExpressions.Regex.Match(arg,@"/[0-9a-zA-Z]+:.+").Success;
        }
        private TimeSpan SecondsToTimeSpan(string p) {
            return new TimeSpan(0, 0, Int32.Parse(p));
        }
        private bool isFileName(string name) {
            return File.Exists(name);
        }
        #region Properties
        
        public string HRMFileName { get; set; }
        public string GPSFileName { get; set; }
        #endregion
        
        public static CommandLineArguments Parse(string[] args) {
            return new CommandLineArguments(args);
        }
        public Dictionary<string, string> GetOptions() {
            return new Dictionary<string, string>(options);
        }

        public Dictionary<string, string> Options
        {
            get
            {
                return new Dictionary<string, string>(options);
            }
        }
    }
}
