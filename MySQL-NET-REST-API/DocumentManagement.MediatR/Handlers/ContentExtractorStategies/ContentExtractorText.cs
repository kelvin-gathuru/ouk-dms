using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using Tesseract;


namespace DocumentManagement.MediatR.Handlers.ContentExtractorStategies;
public static class ContentExtractorText
{
    public static string ExtractTextPath(string tessdataPath, string tessLang, string imagePath, ILogger<Object> logger = null)
    {
        try
        {
            var text = new StringBuilder();

            using (var engine = new TesseractEngine(tessdataPath, tessLang, Tesseract.EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        text.Append(page.GetText()); // Get recognized text

                    }
                }
            }

            // Delete image if OCR succeeded (your original logic deleted on fail)
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            return text.ToString();
        }
        catch (Exception ex)
        {
            if (logger != null)
            {
                logger.LogError(ex, ex.Message);
            }
            return "";
        }
    }
    public static string ExtractTextBytes(string tessdataPath, string tessLang, byte[] imagePath, ILogger<Object> logger = null)
    {
        try
        {
            var text = new StringBuilder();

            using (var engine = new TesseractEngine(tessdataPath, tessLang, Tesseract.EngineMode.Default))
            {
                using (var img = Pix.LoadFromMemory(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        text.Append(page.GetText()); // Get recognized text

                    }
                }
            }

            return text.ToString();
        }
        catch (Exception ex)
        {
            if (logger != null)
            {
                logger.LogError(ex, ex.Message);
            }
            return "";
        }
    }
}
