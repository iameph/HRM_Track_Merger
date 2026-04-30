// Refactored Settings.cs

using System;
using System.Collections.Generic;
using System.Linq;

namespace HRM_Track
{
    public class Settings
    {
        // Other properties and methods...

        private string sex;

        public string Sex
        {
            get { return sex; }
            set
            {
                // Improved error handling for invalid values
                if (value != "male" && value != "female")
                {
                    throw new ArgumentException("Invalid sex value. Use 'male' or 'female'.");
                }
                sex = value;
            }
        }

        // Other methods...
    }
}