using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HRM_Track_Merger {
    class Settings {
        public double? Age;
        public DateTime? Birthday;
        public double? Weight;
        public ExerciseData.Sex? Sex;
        public double? VO2max;

        public string Sport;

        public GarminTCX.Creator Device;
        public GarminTCX.Author Author;
        public Settings() {
        }
        public Settings(string filename) {
            LoadFromFile(filename);
        }
        public void LoadFromFile(string filename) {
            var dic = new Dictionary<string, string>();
            using (var file = File.OpenText(filename)) {
                while (!file.EndOfStream) {
                    var str = file.ReadLine().Trim();
                    if (str.StartsWith("#") || str.StartsWith("//") || str=="") {
                        continue;
                    }
                    var splits = str.Split(new char[] { '=' }, 2);
                    if (splits.Length == 2) {
                        var key = splits[0].Trim().ToLower();
                        var val = splits[1].Trim();
                        if (dic.ContainsKey(key)) {
                            dic[key] = val;
                        }
                        else {
                            dic.Add(key, val);
                        }
                    }
                }
            }
            var device = new GarminTCX.Creator();
            var author = new GarminTCX.Author();

            foreach (var key in dic.Keys) {
                switch (key) {
                    case "age":
                        Age = Double.Parse(dic[key]);
                        break;
                    case "birthday":
                        Birthday = DateTime.Parse(dic[key]);
                        break;
                    case "weight":
                        Weight = Double.Parse(dic[key]);
                        break;
                    case "sex":
                        switch (dic[key].ToLower()) {
                            case "male":
                                Sex = ExerciseData.Sex.Male;
                                break;
                            case "female":
                                Sex = ExerciseData.Sex.Male;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "sport":
                        Sport = dic[key].Trim();
                        break;
                    case "vo2max":
                        VO2max = Double.Parse(dic[key]);
                        break;
                    case "devicename":
                        device.Name = dic[key];
                        break;
                    case "deviceproductid":
                        device.ProductID = uint.Parse(dic[key]);
                        break;
                    case "deviceunitid":
                        device.UnitID = uint.Parse(dic[key]);
                        break;
                    case "deviceversion":
                        device.Version = new List<string>(dic[key].Split('.')).Select(el => uint.Parse(el)).ToArray();
                        break;
                    case "authorname":
                        author.Name = dic[key];
                        break;
                    case "authorversion":
                        author.Version = new List<string>(dic[key].Split('.')).Select(el => uint.Parse(el)).ToArray();
                        break;
                    case "authorlangid":
                        author.LangID = dic[key];
                        break;
                    case "authorpartnumber":
                        author.PartNumber = dic[key];
                        break;
                    default:
                        break;
                }
            }
            if (device.Name != null) {
                Device = device;
            }
            if (author.Name != null) {
                Author = author;
            }
        }
        public static readonly Settings Default = new Settings() {
            Author = new GarminTCX.Author() {
                Name = "HRM_Track_Merger"
            },
            Device = new GarminTCX.Creator() {
                Name = "Unknown Device"
            },
            Sport = "Other"
        };
        public ExerciseData.UserData GetUserData(DateTime exerciseTime) {
            var data = new ExerciseData.UserData() {
                Age = Age,
                Sex = Sex,
                VO2Max = VO2max,
                Weight = Weight
            };
            if (Birthday != null) {
                int years = exerciseTime.Year - Birthday.Value.Year;
                if (Birthday.Value > exerciseTime.AddYears(-years)) {
                    --years;
                }
                var FromLastBirthDay = (exerciseTime - Birthday.Value.AddYears(years));
                var ToNextBirthday = (Birthday.Value.AddYears(years + 1) - exerciseTime);
                data.Age = years + FromLastBirthDay.TotalHours / (FromLastBirthDay.TotalHours + ToNextBirthday.TotalHours);
            }
            return data;
        }
    }
}
