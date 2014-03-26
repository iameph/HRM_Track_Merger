using System;

namespace HRM_Track_Merger {
    public class InvalidFileFormatException : Exception {
        public InvalidFileFormatException() : base() { }
        public InvalidFileFormatException(string message) : base(message) { }
    }
}
