using System.Text.RegularExpressions;

namespace DocumentManagement.Helper;
public static class CleanOCRText
{
    public static string ClearText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Apply multiple cleaning steps
        text = Regex.Replace(text, @"[^\x20-\x7E]", " ");   // Remove non-printable ASCII characters
                                                            // Normalize spaces (Remove extra spaces, new lines, tabs)

        return text;
    }
}
