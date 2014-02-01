using System;

namespace HRM_Track_Merger {
    class InvalidArgumentsException : Exception {
        public InvalidArgumentsException() : base() { }
        public InvalidArgumentsException(string message) : base(message) { }
    }
}
