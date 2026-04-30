using System;

namespace HRM_Track_Merger
{
    /// <summary>
    /// Contains application-wide constant values for command-line options, file extensions, and settings.
    /// </summary>
    public static class Constants
    {
        // Command-line help flags
        public const string HelpFlag1 = "/?";
        public const string HelpFlag2 = "-h";
        public const string HelpFlag3 = "--help";

        // File extension constants
        public const string HrmExtension = ".hrm";
        public const string XmlExtension = ".xml";
        public const string GpxExtension = ".gpx";
        public const string TcxExtension = ".tcx";

        // Option names
        public const string NoSpeedOption = "nospeed";
        public const string OffsetOption = "offset";
        public const string OutputOption = "output";
        public const string SportOption = "sport";
        public const string WeightOption = "weight";
        public const string AgeOption = "age";
        public const string VO2MaxOption = "vo2max";
        public const string SexOption = "sex";

        // Settings file
        public const string SettingsFileName = "settings.cfg";

        // Default values
        public const string DefaultSport = "Other";

        // Settings file keys
        public const string SettingsKeyAge = "age";
        public const string SettingsKeyBirthday = "birthday";
        public const string SettingsKeyWeight = "weight";
        public const string SettingsKeySex = "sex";
        public const string SettingsKeySport = "sport";
        public const string SettingsKeyVO2Max = "vo2max";
        public const string SettingsKeyDeviceName = "devicename";
        public const string SettingsKeyDeviceProductId = "deviceproductid";
        public const string SettingsKeyDeviceUnitId = "deviceunitid";
        public const string SettingsKeyDeviceVersion = "deviceversion";
        public const string SettingsKeyAuthorName = "authorname";
        public const string SettingsKeyAuthorVersion = "authorversion";
        public const string SettingsKeyAuthorLangId = "authorlangid";
        public const string SettingsKeyAuthorPartNumber = "authorpartnumber";

        // Sex values
        public const string SexMale = "male";
        public const string SexFemale = "female";

        // Output filename format
        public const string MergeInfix = "_Merge";
    }
}