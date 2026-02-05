using DocumentFormat.OpenXml.Packaging;
using DocumentManagement.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DocumentManagement.MediatR.Handlers.ContentExtractorStategies;

public class PptContentExtractor : IContentExtractor
{
    readonly string _wwwRootPath = string.Empty;
    readonly string outputImagePath = Guid.NewGuid() + "_extracted_image.png";
    public PptContentExtractor(string wwwRootPath)
    {
        _wwwRootPath = wwwRootPath;
        outputImagePath = Path.Combine(_wwwRootPath, outputImagePath);
    }
    public string ExtractContentByBytes(byte[] documentBytes, string tessdataPath, string tessLang)
    {
        try
        {
            using (MemoryStream pptStream = new MemoryStream(documentBytes))
            {
                using (PresentationDocument presentationDocument = PresentationDocument.Open(pptStream, false))
                {
                    // Access the presentation part
                    PresentationPart presentationPart = presentationDocument.PresentationPart;
                    StringBuilder str = new StringBuilder();
                    if (presentationPart != null && presentationPart.SlideParts != null)
                    {
                        foreach (var slidePart in presentationPart.SlideParts)
                        {
                            // Extract text from the slide
                            var slideText = string.Join(" ", slidePart.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>().Select(t => t.Text));
                            if (!string.IsNullOrWhiteSpace(slideText))
                            {
                                var cleanText = UnWantKeywordRemovalHelper.CleanExtractedText(slideText);
                                str.Append(" " + cleanText);
                            }

                            foreach (var imagePart in slidePart.ImageParts)
                            {
                                // Save the image to disk or process it further
                                using (Stream imageStream = imagePart.GetStream())
                                {
                                    using (FileStream fileStream = new FileStream(outputImagePath, FileMode.Create, FileAccess.Write))
                                    {
                                        imageStream.CopyTo(fileStream);

                                    }

                                    string imgText = ContentExtractorText.ExtractTextPath(tessdataPath, tessLang, outputImagePath);
                                    if (!string.IsNullOrEmpty(imgText))
                                    {
                                        var cleanText = UnWantKeywordRemovalHelper.CleanExtractedText(imgText);
                                        str.Append(" " + cleanText);
                                    }

                                    if (File.Exists(outputImagePath))
                                    {
                                        File.Delete(outputImagePath);
                                    }
                                }

                            }
                        }

                    }
                    return str.ToString();
                }
            }
        }
        catch (Exception)
        {
            return "";
        }
    }

    static byte[] StreamToBytes(Stream inputStream)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Copy the input stream to the memory stream
            inputStream.CopyTo(memoryStream);
            return memoryStream.ToArray(); // Return the byte array
        }
    }


    public string ExtractContentByFile(IFormFile file, string tessdataPath, string tessLang)
    {
        if (file == null || file.Length == 0)
        {
            return "No file uploaded or file is empty.";
        }
        try
        {
            using (var stream = file.OpenReadStream())
            {
                using (PresentationDocument presentationDocument = PresentationDocument.Open(stream, false))
                {
                    // Access the presentation part
                    PresentationPart presentationPart = presentationDocument.PresentationPart;
                    StringBuilder str = new StringBuilder();
                    if (presentationPart != null && presentationPart.SlideParts != null)
                    {
                        foreach (var slidePart in presentationPart.SlideParts)
                        {
                            // Extract text from the slide
                            var slideText = string.Join(" ", slidePart.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>().Select(t => t.Text));
                            if (!string.IsNullOrWhiteSpace(slideText))
                            {
                                str.AppendLine(slideText);
                            }

                            foreach (var imagePart in slidePart.ImageParts)
                            {
                                // Save the image to disk or process it further
                                using (Stream imageStream = imagePart.GetStream())
                                {
                                    using (FileStream fileStream = new FileStream(outputImagePath, FileMode.Create, FileAccess.Write))
                                    {
                                        imageStream.CopyTo(fileStream);

                                    }

                                    string imgText = ContentExtractorText.ExtractTextPath(tessdataPath, tessLang, outputImagePath);
                                    if (!string.IsNullOrEmpty(imgText))
                                    {
                                        var cleanText = UnWantKeywordRemovalHelper.CleanExtractedText(imgText);
                                        str.Append(" " + cleanText);
                                    }

                                    if (File.Exists(outputImagePath))
                                    {
                                        File.Delete(outputImagePath);
                                    }
                                }
                            }

                        }
                    }
                    return str.ToString();
                }
            }

        }
        catch (Exception)
        {
            return "";
        }
    }
}
