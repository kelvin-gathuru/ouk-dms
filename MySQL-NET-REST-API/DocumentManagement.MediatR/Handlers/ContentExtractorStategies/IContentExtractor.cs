using Microsoft.AspNetCore.Http;


namespace DocumentManagement.MediatR.Handlers
{
    public interface IContentExtractor
    {
        string ExtractContentByBytes(byte[] documentBytes, string tessdataPath, string tessLang);
        string ExtractContentByFile(IFormFile file, string tessdataPath, string tessLang);
    }
}
