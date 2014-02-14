﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRM_Track_Merger.PolarXML {
    class PolarXMLFile {
        private List<ExerciseElement> _exercises;
        public List<ExerciseElement> Exercises {
            get {
                return _exercises;
            }
            set {
                _exercises = value;
            }
        }
        public User User;
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
        }

    }
}
