
using DocumentManagement.MediatR.Handlers.ContentExtractorStategies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class TessDataContextExtractor
{
    public string ExtractContentByBytes(string tessDataPath, byte[] documentBytes, string tessLang, ILogger<Object> logger)
    {
        try
        {
            string text = string.Empty;
            if (documentBytes == null || documentBytes.Length == 0)
            {
                return text;
            }

            //using (var engine = new TesseractEngine(tessDataPath, tessLang, Tesseract.EngineMode.Default))
            //{
            //    using (var img = Pix.LoadFromMemory(documentBytes))
            //    {
            //        using (var page = engine.Process(img))
            //        {
            //            text = page.GetText(); // Get recognized text

            //        }
            //    }
            //}
            text = ContentExtractorText.ExtractTextBytes(tessDataPath, tessLang, documentBytes, logger);

            return text;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return "";
        }
    }
    public async Task<string> ExtractContentByFile(string tessDataPath, IFormFile file, string tessLang, ILogger<Object> logger)
    {
        if (file == null || file.Length == 0)
        {
            return "";
        }

        try
        {

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                //// Step 2: Use Tesseract to perform OCR on the file in memory
                //using (var engine = new TesseractEngine(tessDataPath, tessLang, Tesseract.EngineMode.Default))
                //{
                //    using (var img = Pix.LoadFromMemory(fileBytes))
                //    {
                //        using (var page = engine.Process(img))
                //        {
                //            return page.GetText();
                //        }
                //    }
                //}

                return ContentExtractorText.ExtractTextBytes(tessDataPath, tessLang, fileBytes, logger);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return "";
        }

    }
}
