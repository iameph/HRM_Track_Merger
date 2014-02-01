using System.Collections.Generic;
using System.IO;

namespace HRM_Track_Merger {
    class HRMParser {
        private Dictionary<string, List<string>> sections;
        public static HRMParser GetParser(string fileName){
            if (Path.GetExtension(fileName).Equals(".hrm")) {
                return new HRMParser(fileName);
            }
            else {
                throw new UnknownFileTypeException();
            }
        }
        protected HRMParser(string fileName) {
            using (var file = File.OpenText(fileName)) {
                sections = new Dictionary<string, List<string>>();
                var sectionName = "";
                while (!file.EndOfStream) {
                    var line = file.ReadLine().Replace(System.Environment.NewLine,"").Trim();
                    if (line.Length>2 && line[0] == '[' && line[line.Length - 1] == ']') {
                        sectionName = line.Substring(1, line.Length - 2);
                    }
                    else if (sectionName != "") {
                        if (!sections.ContainsKey(sectionName)) {
                            sections.Add(sectionName,new List<string>());
                        }
                        sections[sectionName].Add(line);
                    }
                }
            }
        }
        public string[] GetSection(string sectionName) {
            return sections[sectionName].ToArray();
        }
        
    }
}
