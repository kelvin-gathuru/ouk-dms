using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetAllDocumentSignatureQueryHandler(IDocumentSignatureRepository _documentSignatureRepository, PathHelper _pathHelper, IWebHostEnvironment _webHostEnvironment) : IRequestHandler<GetAllDocumentSignatureQuery, ServiceResponse<List<DocumentSignatureDataDto>>>
{
    private string StoragePath => Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SignaturePath);

    private byte[] GetFileBytes(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return AesOperation.ConvertStreamToByteArray(stream);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to read file: {ex.Message}", ex);
        }
    }


    public async Task<ServiceResponse<List<DocumentSignatureDataDto>>> Handle(GetAllDocumentSignatureQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _documentSignatureRepository.All
                .Where(c => c.DocumentId == request.DocumentId)
                .Include(a => a.SignatureUser)
                .OrderByDescending(c => c.SignatureDate)
                .ToListAsync(cancellationToken);

            var dtos = entities.Select(c =>
            {
                string filePath = Path.Combine(StoragePath, c.SignatureUrl);
                byte[] fileBytes = GetFileBytes(filePath);
                return new DocumentSignatureDataDto
                {
                    SignatureDate = c.SignatureDate,
                    SignatureUrl = c.SignatureUrl,
                    Data = fileBytes,
                    SignatureUser = c.SignatureUser != null
                        ? $"{c.SignatureUser.FirstName} {c.SignatureUser.LastName}"
                        : "",
                };
            }).ToList();
            return ServiceResponse<List<DocumentSignatureDataDto>>.ReturnResultWith200(dtos);
        }
        catch (Exception ex)
        {
            return ServiceResponse<List<DocumentSignatureDataDto>>.Return409($"Failed to get signature: {ex.Message}");
        }
    }
}
