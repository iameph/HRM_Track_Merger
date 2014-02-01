using System;

namespace HRM_Track_Merger {
    class InvalidFileFormatException : Exception {
        public InvalidFileFormatException() : base() { }
        public InvalidFileFormatException(string message) : base(message) { }
    }
}
