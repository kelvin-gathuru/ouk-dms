using System.Text.RegularExpressions;

namespace DocumentManagement.Helper;

public class UnWantKeywordRemovalHelper
{

    public static string CleanExtractedText(string text)
    {
        //// Remove unnecessary whitespace and unwanted content
        //text = Regex.Replace(text, @"\s+", " "); // Replace multiple spaces with a single space
        //text = text.Trim(); // Trim leading and trailing spaces

        // Filter out stop words or specific patterns
        //string[] unwantedWords = {
        //    "i", "me", "my", "myself", "we", "our", "ours", "ourselves", "you", "your",
        //    "yours", "yourself", "yourselves", "he", "him", "his", "himself", "she",
        //    "her", "hers", "herself", "it", "its", "itself", "they", "them", "their",
        //    "theirs", "themselves", "what", "which", "who", "whom", "this", "that",
        //    "these", "those", "am", "is", "are", "was", "were", "be", "been", "being",
        //    "have", "has", "had", "having", "do", "does", "did", "doing", "a", "an",
        //    "the", "and", "but", "if", "or", "because", "as", "until", "while", "of",
        //    "at", "by", "for", "with", "about", "against", "between", "into", "through",
        //    "during", "before", "after", "above", "below", "to", "from", "up", "down",
        //    "in", "out", "on", "off", "over", "under", "again", "further", "then",
        //    "once", "here", "there", "when", "where", "why", "how", "all", "any",
        //    "both", "each", "few", "more", "most", "other", "some", "such", "no",
        //    "nor", "not", "only", "own", "same", "so", "than", "too", "very", "s",
        //    "t", "can", "will", "just", "don", "should", "now"
        //};

        //var filteredText = string.Join(" ", text.Split(' ').Where(word => !unwantedWords.Contains(word.ToLower())));

        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Apply multiple cleaning steps
        text = Regex.Replace(text, @"[^\x20-\x7E]", " ");   // Remove non-printable ASCII characters
        text = Regex.Replace(text, @"\s+", " ").Trim();    // Normalize spaces (Remove extra spaces, new lines, tabs)

        return text;


    }

}
