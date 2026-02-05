using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Dto.Document;
using DocumentManagement.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.StorageStategies;

public class LocalStorageService : IStorageService
{
    private readonly PathHelper _pathHelper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _storagePath;
    private readonly ILogger<LocalStorageService> _logger;

    public LocalStorageService(PathHelper pathHelper,
        IWebHostEnvironment webHostEnvironment,
        ILogger<LocalStorageService> logger)
    {
        _pathHelper = pathHelper;
        _webHostEnvironment = webHostEnvironment;
        _storagePath = Path.Combine(_webHostEnvironment.ContentRootPath, _pathHelper.DocumentPath);
        _logger = logger;
    }


    public async Task<UploadFileResponse> UploadBytesAsync(byte[] file, StorageSetting storageSetting, string extension)
    {
        try
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            if (!extension.StartsWith("."))
                extension = "." + extension;
            string uri = $"{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(_storagePath, uri);
            UploadFileResponse result = new();
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                if (storageSetting.EnableEncryption)
                {
                    var keyGenerator = KeyGenerator.GenerateKeyAndIV();
                    var byteArray = AesOperation.EncryptBytesToBytes(file, keyGenerator.Item1, keyGenerator.Item2);
                    await stream.WriteAsync(byteArray, 0, byteArray.Length);
                    result = new UploadFileResponse
                    {
                        FileName = uri,
                        Key = keyGenerator.Item1,
                        IV = keyGenerator.Item2
                    };
                    return result;
                }
                else
                {
                    await stream.WriteAsync(file, 0, file.Length);
                    result = new UploadFileResponse
                    {
                        FileName = uri,
                    };
                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"error while uploading file in Local storage: {ex.Message}");
            return new();
        }
    }

    public async Task<UploadFileResponse> UploadFileAsync(IFormFile file, StorageSetting storageSetting, string extension)
    {
        try
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            if (string.IsNullOrEmpty(extension))
                extension = MimeTypeHelper.GetFileExtension(file.ContentType);
            if (!extension.StartsWith("."))
                extension = "." + extension;
            string uri = $"{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(_storagePath, uri);
            UploadFileResponse result = new();
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                if (storageSetting.EnableEncryption)
                {
                    var keyGenerator = KeyGenerator.GenerateKeyAndIV();
                    var byteArray = AesOperation.EncryptFileToBytes(file, keyGenerator.Item1, keyGenerator.Item2);
                    await stream.WriteAsync(byteArray, 0, byteArray.Length);
                    result = new UploadFileResponse
                    {
                        FileName = uri,
                        Key = keyGenerator.Item1,
                        IV = keyGenerator.Item2
                    };
                    return result;
                }
                else
                {
                    var bytesData = AesOperation.ConvertIFormFileToByteArray(file);
                    await stream.WriteAsync(bytesData, 0, bytesData.Length);
                    result = new UploadFileResponse
                    {
                        FileName = uri,
                    };
                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"error while uploading file in Local storage: {ex.Message}");
            return new();
        }
    }

    public async Task<UploadFileResponse> UploadFileChunkAsync(IFormFile file, StorageSetting storageSetting, string extension, byte[] key, byte[] iv)
    {
        try
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            string uri = $"{Guid.NewGuid()}";
            string fullPath = Path.Combine(_storagePath, uri);
            UploadFileResponse result = new();
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                if (storageSetting.EnableEncryption)
                {
                    var byteArray = AesOperation.EncryptFileToBytes(file, key, iv);
                    await stream.WriteAsync(byteArray, 0, byteArray.Length);
                    result = new UploadFileResponse
                    {
                        FileName = uri,
                        Key = key,
                        IV = iv
                    };
                    return result;
                }
                else
                {
                    var bytesData = AesOperation.ConvertIFormFileToByteArray(file);
                    await stream.WriteAsync(bytesData, 0, bytesData.Length);
                    result = new UploadFileResponse
                    {
                        FileName = uri,
                    };
                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"error while uploading file in Local storage: {ex.Message}");
            return new();
        }
    }

    public async Task<UploadFileResponse> TestUploadFileAsync(IFormFile file, StorageSetting storageSetting, string extension)
    {
        return await UploadFileAsync(file, storageSetting, extension);
    }

    public async Task<DownloadFileResponse> DownloadFileAsync(string fileName, string storageSettingId, byte[] key, byte[] iv)
    {
        try
        {
            var filePath = Path.Combine(_storagePath, fileName);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            if (key != null && iv != null)
            {
                var encryptBytes = AesOperation.ConvertStreamToByteArray(stream);
                var bytes = await Task.FromResult<byte[]>(AesOperation.DecryptFileFromBytes(encryptBytes, key, iv));
                return new DownloadFileResponse { FileBytes = bytes };
            }
            else
            {
                var bytes = await Task.FromResult<byte[]>(AesOperation.ConvertStreamToByteArray(stream));
                return new DownloadFileResponse { FileBytes = bytes };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"error while downloading file from Local storage: {ex.Message}");
            return new DownloadFileResponse() { ErrorMessage = $"error while downloading file from Local storage. {ex.Message}" };
        }
    }

    public Task DeleteFileAsync(string fileName, string storageSettingId)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }
}
