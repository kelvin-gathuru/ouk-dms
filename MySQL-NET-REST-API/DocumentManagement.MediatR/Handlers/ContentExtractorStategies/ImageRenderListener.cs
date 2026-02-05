
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using Image = SixLabors.ImageSharp.Image;

namespace DocumentManagement.MediatR.Handlers.ContentExtractorStategies;

public class ImageRenderListener : IEventListener
{
    private readonly List<Image<Rgba32>> _images;

    public ImageRenderListener(List<Image<Rgba32>> images)
    {
        _images = images;
    }

    public void EventOccurred(IEventData data, EventType type)
    {
        if (type == EventType.RENDER_IMAGE)
        {
            var renderInfo = (ImageRenderInfo)data;
            var pdfImage = renderInfo.GetImage();

            if (pdfImage != null)
            {
                // Convert PDF image to ImageSharp image
                var image = ConvertPdfImageToImageSharp(pdfImage);
                if (image != null)
                {
                    _images.Add(image);
                }
            }
        }
    }

    public ICollection<EventType> GetSupportedEvents()
    {
        return null; // We're interested in all events
    }

    private Image<Rgba32> ConvertPdfImageToImageSharp(PdfImageXObject pdfImage)
    {
        try
        {
            // Get the raw bytes of the image
            var imageBytes = pdfImage.GetImageBytes();
            using (var ms = new MemoryStream(imageBytes))
            {
                return Image.Load<Rgba32>(ms); // Convert bytes to ImageSharp Image
            }
        }
        catch (Exception)
        {
            return null;
        }
    }
}
