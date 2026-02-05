using DocumentManagement.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;

namespace DocumentManagement.MediatR.Handlers;

public class TextContentExtractor : IContentExtractor
{
    public string ExtractContentByBytes(byte[] documentBytes, string tessdataPath, string tessLang)
    {
        // Convert the byte array to a string using the specified encoding
        try
        {


            string fileContent = Encoding.UTF8.GetString(documentBytes);
            var cleanText = UnWantKeywordRemovalHelper.CleanExtractedText(fileContent.ToString());
            if (!string.IsNullOrEmpty(cleanText))
            {
                return cleanText;
            }
            return "";
        }
        catch (Exception)
        {
            return "";
        }
    }

    public string ExtractContentByFile(IFormFile file, string tessdataPath, string tessLang)
    {
        if (file == null || file.Length == 0)
        {
            return "No file uploaded or the file is empty.";
        }

        var stringBuilder = new StringBuilder();
        try
        {

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                // Read the content of the file line by line
                while (reader.ReadLine() is string line)
                {
                    var cleanText = UnWantKeywordRemovalHelper.CleanExtractedText(line);
                    if (!string.IsNullOrEmpty(cleanText))
                    {
                        stringBuilder.AppendLine(cleanText);
                    }
                }
            }

            return stringBuilder.ToString();
        }
        catch (Exception)
        {
            return "";
        }
    }


}
