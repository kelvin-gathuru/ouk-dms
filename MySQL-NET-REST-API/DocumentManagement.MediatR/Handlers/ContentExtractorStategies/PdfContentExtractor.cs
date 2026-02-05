using DocumentManagement.MediatR.Handlers.ContentExtractorStategies;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using Microsoft.AspNetCore.Http;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class PdfContentExtractor : IContentExtractor
{
    readonly string _wwwRootPath = string.Empty;
    readonly string outputImagePath = Guid.NewGuid() + "_extracted_image.png";
    public PdfContentExtractor(string wwwRootPath)
    {
        _wwwRootPath = wwwRootPath;
        outputImagePath = Path.Combine(_wwwRootPath, outputImagePath);
    }
    private static readonly object _lock = new object();

    public string ExtractContentByBytes(byte[] documentBytes, string tessdataPath, string tessLang)
    {
        StringBuilder text = new StringBuilder();
        try
        {
            using (var memoryStream = new MemoryStream(documentBytes))
            {
                using (PdfReader pdfReader = new PdfReader(memoryStream))
                using (iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader))
                {

                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        var page = pdfDocument.GetPage(i);

                        var processor = new PdfCanvasProcessor(new ImageExtractionHandler(outputImagePath));
                        processor.ProcessPageContent(page);

                        var annotations = page.GetAnnotations();

                        foreach (var annotation in annotations)
                        {
                            // Check if it's a text annotation (comment)
                            if (annotation.GetSubtype().Equals(PdfName.FreeText))
                            {
                                // Extract the comment's content
                                string content = annotation.GetContents()?.ToString() ?? "";
                                if (!string.IsNullOrEmpty(content))
                                {
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        text.Append(" " + content);
                                    }
                                }
                                // Get author and other metadata if needed
                                string author = annotation.GetPdfObject().GetAsString(PdfName.T)?.ToString() ?? "";
                                if (!string.IsNullOrEmpty(author))
                                {

                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        text.Append(" " + content);
                                    }
                                }
                            }
                        }

                        var pageText = PdfTextExtractor.GetTextFromPage(page);

                        if (!string.IsNullOrEmpty(pageText))
                        {
                            if (!string.IsNullOrEmpty(pageText))
                            {
                                text.Append(" " + pageText);
                            }

                        }
                        if (File.Exists(outputImagePath))
                        {
                            try
                            {

                                string imgText = ContentExtractorText.ExtractTextPath(tessdataPath, tessLang, outputImagePath);
                                if (!string.IsNullOrEmpty(imgText))
                                {
                                    text.Append(" " + imgText);
                                }

                            }
                            catch (Exception)
                            {
                                File.Delete(outputImagePath); // Delete the image if OCR fails
                            }
                        }

                    }
                    return text.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return text.ToString();
        }
    }


    //public string ExtractContentByBytes(byte[] documentBytes, string tessdataPath, string tessLang)
    //{
    //    string extractedText = "";
    //    try
    //    {
    //        using (var memoryStream = new MemoryStream(documentBytes))
    //        {
    //            using (var document = UglyToad.PdfPig.PdfDocument.Open(memoryStream))
    //            {
    //                extractedText = ExtractTextFromPdf(document, tessdataPath, tessLang);
    //                Console.WriteLine(extractedText);

    //                //foreach (var page in document.GetPages())
    //                //{
    //                //    extractedText += page.Text;
    //                //    foreach (var image in page.GetImages())
    //                //    {
    //                //        try
    //                //        {
    //                //            if (image.RawBytes != null && image.RawBytes.Length > 0)
    //                //            {
    //                //                byte[] pngBytes = ConvertToPng(image.RawBytes.ToArray());
    //                //                if (pngBytes != null)
    //                //                {
    //                //                    using var img = Pix.LoadFromMemory(pngBytes);
    //                //                    using var pageText = engine.Process(img);
    //                //                    extractedText += pageText.GetText() + "\n";
    //                //                }
    //                //            }
    //                //        }

    //                //        catch (Exception)
    //                //        {
    //                //            continue;
    //                //        }

    //                //    }
    //                //}
    //            }
    //        }
    //        return extractedText;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //        return extractedText;
    //    }
    //}

    public static string ExtractTextFromPdf(UglyToad.PdfPig.PdfDocument document, string tessdataPath, string tessLang)
    {

        var extractedText = new ConcurrentBag<string>(); // Thread-safe collection
        Parallel.ForEach(document.GetPages(), page =>
        {
            try
            {
                extractedText.Add(page.Text); // Add page text

                Parallel.ForEach(page.GetImages(), image =>
               {
                   try
                   {
                       if (image.RawBytes != null && image.RawBytes.Length > 0)
                       {
                           byte[] pngBytes = ConvertToPng(image.RawBytes.ToArray());
                           if (pngBytes != null)
                           {
                               string text = ContentExtractorText.ExtractTextBytes(tessdataPath, tessLang, pngBytes);
                               if (string.IsNullOrEmpty(text))
                               {
                                   extractedText.Add(text);
                               }

                               //using var engine = new TesseractEngine(tessdataPath, tessLang, EngineMode.Default);
                               //{
                               //    using var img = Pix.LoadFromMemory(pngBytes);
                               //    using var pageText = engine.Process(img);
                               //    // Add OCR text
                               //}
                           }
                       }
                   }
                   catch (Exception)
                   {

                   }
               });

            }
            catch (Exception)
            {

            }
        });
        return string.Join(" ", extractedText);
    }

    public static byte[] ConvertToPng(byte[] imageBytes)
    {
        using var bitmap = SKBitmap.Decode(imageBytes);
        if (bitmap == null)
        {
            return null;
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return data.ToArray();
    }

    public string ExtractContentByFile(IFormFile file, string tessdataPath, string tessLang)
    {
        if (file == null || file.Length == 0)
        {
            return "";
        }
        string outputImagePath = "extracted_image.png";
        var text = new StringBuilder();
        try
        {
            using (var stream = file.OpenReadStream())
            {
                // Initialize PdfReader with the file stream
                using (var pdfReader = new PdfReader(stream))
                {
                    // Load the PDF document using PdfDocument
                    using (var pdfDoc = new iText.Kernel.Pdf.PdfDocument(pdfReader))
                    {
                        // Loop through all pages and extract text
                        for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                        {
                            var pdfPage = pdfDoc.GetPage(page);

                            var processor = new PdfCanvasProcessor(new ImageExtractionHandler(outputImagePath));
                            processor.ProcessPageContent(pdfPage);
                            var annotations = pdfPage.GetAnnotations();

                            foreach (var annotation in annotations)
                            {
                                // Check if it's a text annotation (comment)
                                if (annotation.GetSubtype().Equals(PdfName.FreeText))
                                {
                                    // Extract the comment's content
                                    string content = annotation.GetContents()?.ToString() ?? "";
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        if (!string.IsNullOrEmpty(content))
                                        {
                                            text.Append(" " + content);
                                        }

                                    }
                                    // Get author and other metadata if needed
                                    string author = annotation.GetPdfObject().GetAsString(PdfName.T)?.ToString() ?? "";
                                    if (!string.IsNullOrEmpty(author))
                                    {
                                        if (!string.IsNullOrEmpty(author))
                                        {
                                            text.Append(" " + author);
                                        }
                                    }
                                }
                            }

                            string pageText = PdfTextExtractor.GetTextFromPage(pdfPage);

                            if (!string.IsNullOrEmpty(pageText))
                            {
                                if (!string.IsNullOrEmpty(pageText))
                                {
                                    text.Append(" " + pageText);
                                }

                            }
                            if (File.Exists(outputImagePath))
                            {
                                try
                                {

                                    string imgText = ContentExtractorText.ExtractTextPath(tessdataPath, tessLang, outputImagePath);
                                    if (!string.IsNullOrEmpty(imgText))
                                    {
                                        text.Append(" " + imgText);
                                    }

                                    //using (var engine = new TesseractEngine(tessdataPath, tessLang, EngineMode.Default))
                                    //{
                                    //    using (var pixImage = Pix.LoadFromFile(outputImagePath))
                                    //    {
                                    //        using (var pageOCR = engine.Process(pixImage))
                                    //        {
                                    //            var imgText = pageOCR.GetText();
                                    //            if (!string.IsNullOrEmpty(imgText))
                                    //            {
                                    //                text.Append(" " + imgText);
                                    //            }
                                    //            File.Delete(outputImagePath); // Delete the image if OCR fails
                                    //        }
                                    //    }
                                    //}
                                }
                                catch (Exception)
                                {
                                    File.Delete(outputImagePath); // Delete the image if OCR fails
                                }
                            }
                        }
                    }
                }
            }
            return text.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return text.ToString();
        }
    }


    // Check if a page contains text
    private static bool CheckPageForText(PdfPage page)
    {
        var strategy = new SimpleTextExtractionStrategy();
        string extractedText = PdfTextExtractor.GetTextFromPage(page, strategy);
        return !string.IsNullOrWhiteSpace(extractedText);  // Returns true if text is found
    }


}

class ImageExtractionHandler : IEventListener
{
    private readonly string outputImagePath;

    public ImageExtractionHandler(string outputImagePath)
    {
        this.outputImagePath = outputImagePath;
    }

    public void EventOccurred(IEventData data, EventType eventType)
    {
        if (eventType == EventType.RENDER_IMAGE)
        {
            ImageRenderInfo renderInfo = (ImageRenderInfo)data;
            PdfImageXObject imageObject = renderInfo.GetImage();
            byte[] imageBytes = imageObject.GetImageBytes();


            // 🟢 Detect Image Format using ImageSharp
            using (var imageStream = new MemoryStream(imageBytes))
            {
                //ProcessImageForOCR(imageStream)
                using (var stream = new SKManagedStream(imageStream))
                using (var bitmap = SKBitmap.Decode(stream))
                {
                    if (bitmap == null)
                    {
                        Console.WriteLine("Failed to decode image.");
                        return;
                    }

                    // Convert to grayscale
                    using (var grayBitmap = ConvertToGrayscale(bitmap))
                    using (var image = SKImage.FromBitmap(grayBitmap))
                    using (var dataNew = image.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        File.WriteAllBytes(outputImagePath, dataNew.ToArray());
                    }
                }
            }
        }
    }


    private static SKBitmap ConvertToGrayscale(SKBitmap bitmap)
    {
        SKBitmap grayBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
        using (var canvas = new SKCanvas(grayBitmap))
        {
            var paint = new SKPaint
            {
                ColorFilter = SKColorFilter.CreateColorMatrix(new float[]
                {
                0.3f, 0.59f, 0.11f, 0, 0,
                0.3f, 0.59f, 0.11f, 0, 0,
                0.3f, 0.59f, 0.11f, 0, 0,
                0,    0,    0,    1, 0
                })
            };
            canvas.DrawBitmap(bitmap, 0, 0, paint);
        }
        return grayBitmap;
    }


    public ICollection<EventType> GetSupportedEvents()
    {
        return new HashSet<EventType> { EventType.RENDER_IMAGE };
    }
}
