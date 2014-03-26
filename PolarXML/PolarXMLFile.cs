using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class PolarXMLFile : ExerciseData.IExerciseCollection{
        public List<ExerciseElement> Exercises {  get;  set; }
        public User User { get; set; }
        public PolarXMLFile() {
        }
        public PolarXMLFile(string filename) {
            LoadFile(filename);
        }

        private void LoadFile(string filename) {
            var doc = new XmlDocument();
            doc.Load(filename);
            Exercises = new List<ExerciseElement>();
            ParseFile(doc);
            if (User != null) {
                foreach (ExerciseElement exercise in Exercises) {
                    exercise.UpdateUserData(User, false);
                }
            }
        }

        private void ParseFile(XmlDocument doc) {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            var data = doc["polar-exercise-data"];
            if (data == null) {
                throw new InvalidFileFormatException();
            }
            if (data["user"] != null) {
                User = User.Parse(data["user"]);
            }
            foreach (var child in data["calendar-items"].ChildNodes) {
                if (child is XmlElement) {
                    var exercise = (XmlElement)child;
                    if (exercise.LocalName == "exercise") {
                        Exercises.Add(ExerciseElement.Parse(exercise));
                    }
                }
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        }
        public List<ExerciseData.IExercise> GetExercises() {
            return Exercises.Cast<ExerciseData.IExercise>().ToList();
        }

    }
}
