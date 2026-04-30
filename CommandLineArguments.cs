// Updated CommandLineArguments.cs

using YourNamespace.Constants; // Make sure to import the Constants class

public class CommandLineArguments {
    // Use Constants for file extensions
    private const string HrmExtension = Constants.HRM_EXTENSION;
    private const string XmlExtension = Constants.XML_EXTENSION;
    private const string GpxExtension = Constants.GPX_EXTENSION;

    // ... the rest of your class code where you use these variables ...

    public void SomeMethod() {
        // Example usage
        if (filePath.EndsWith(HrmExtension) || filePath.EndsWith(XmlExtension) || filePath.EndsWith(GpxExtension)) {
            // process the file
        }
    }
}