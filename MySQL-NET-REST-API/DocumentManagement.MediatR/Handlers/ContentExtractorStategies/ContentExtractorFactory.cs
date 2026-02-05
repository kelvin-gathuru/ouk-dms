using DocumentManagement.MediatR.Handlers.ContentExtractorStategies;

namespace DocumentManagement.MediatR.Handlers;

public class ContentExtractorFactory
{
    public static IContentExtractor GetExtractor(string fileExtension, string wwwRootPath)
    {
        return fileExtension.ToLower() switch
        {
            ".pdf" => new PdfContentExtractor(wwwRootPath),
            ".doc" or ".docx" => new WordContentExtractor(),
            ".xls" or ".xlsx" => new ExcelContentExtractor(),
            ".ppt" or ".pptx" => new PptContentExtractor(wwwRootPath),
            ".txt" => new TextContentExtractor(),
            _ => null
        };
    }
}
