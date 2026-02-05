using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Dto.Document;
using DocumentManagement.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.StorageStategies;

public class CloudflareR2StorageService(IConfiguration _configuration,
    ILogger<CloudflareR2StorageService> logger) : IStorageService
{
    private AmazonS3Client _r2Client;
    private string _bucketName = string.Empty;

    private async Task CreateS3Client(CloudflareOption storageSetting, bool isTest)
    {
        try
        {
            var accessKeyId = storageSetting.AccessKey ?? _configuration["Cloudflare:R2:AccessKey"];
            var secretAccessKey = storageSetting.SecretKey ?? _configuration["Cloudflare:R2:SecretKey"];
            _bucketName = storageSetting.BucketName ?? _configuration["Cloudflare:R2:BucketName"];
            var r2Endpoint = storageSetting.R2Endpoint ?? _configuration["Cloudflare:R2:Endpoint"];
            var accountId = storageSetting.AccountId ?? _configuration["Cloudflare:R2:AccountId"];

            r2Endpoint = $"https://{accountId}.r2.cloudflarestorage.com";
            if (string.IsNullOrWhiteSpace(accessKeyId) || string.IsNullOrWhiteSpace(secretAccessKey) || string.IsNullOrWhiteSpace(_bucketName) || string.IsNullOrWhiteSpace(r2Endpoint))
            {
                throw new InvalidOperationException("Invalid R2 configuration settings.");
            }

            var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
            var config = new AmazonS3Config
            {
                ServiceURL = r2Endpoint,
                ForcePathStyle = true,
                Timeout = TimeSpan.FromMinutes(10),

            };

            _r2Client = new AmazonS3Client(credentials, config);

            _bucketName = isTest ? await EnsureBucketExistsAsync(storageSetting.BucketName ?? _configuration["Cloudflare:R2:BucketName"]) : storageSetting.BucketName;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while creating R2 bucket: {ex.Message}");
        }
    }

    private async Task<string> EnsureBucketExistsAsync(string bucketName)
    {
        var listResponse = await _r2Client.ListBucketsAsync();
        var bucketExists = listResponse.Buckets.Any(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));

        if (!bucketExists)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };
            await _r2Client.PutBucketAsync(putBucketRequest);
        }

        return bucketName;
    }

    public async Task<UploadFileResponse> UploadFileAsync(IFormFile file, StorageSetting storageSetting, string extension)
    {
        try
        {
            var cloudFlareOptions = GetStorageSetting(storageSetting.JsonValue);
            await CreateS3Client(cloudFlareOptions, false);
            _bucketName = await EnsureBucketExistsAsync(_bucketName);

            if (string.IsNullOrEmpty(extension))
                extension = MimeTypeHelper.GetFileExtension(file.ContentType);
            if (!extension.StartsWith("."))
                extension = "." + extension;
            string fileName = Guid.NewGuid().ToString() + extension;
            UploadFileResponse result = new();
            if (storageSetting.EnableEncryption)
            {
                var keyGenerator = KeyGenerator.GenerateKeyAndIV();
                var byteArray = AesOperation.EncryptFileToBytes(file, keyGenerator.Item1, keyGenerator.Item2);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    var uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = _bucketName,
                        DisablePayloadSigning = true
                    };

                    try
                    {
                        await _r2Client.PutObjectAsync(uploadRequest);
                        result = new UploadFileResponse
                        {
                            FileName = fileName,
                            Key = keyGenerator.Item1,
                            IV = keyGenerator.Item2
                        };
                        return result;

                    }
                    catch (AmazonS3Exception ex)
                    {
                        logger.LogError(ex, $"Error while uploading file in AWS storage: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error while uploading file in AWS storage: {ex.Message}");
                    }
                    return result;
                }
            }
            else
            {
                var uploadRequest = new PutObjectRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = fileName,
                    BucketName = _bucketName,
                    DisablePayloadSigning = true
                };

                await UploadToR2(uploadRequest);
                result = new UploadFileResponse { FileName = fileName };
            }

            return result;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in Cloudflare R2 storage: {ex.Message}");
            return new UploadFileResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in Cloudflare R2 storage: {ex.Message}");
            return new UploadFileResponse();
        }
    }


    public async Task<UploadFileResponse> UploadFileChunkAsync(IFormFile file, StorageSetting storageSetting, string extension, byte[] key, byte[] iv)
    {
        try
        {
            var cloudFlareOptions = GetStorageSetting(storageSetting.JsonValue);
            await CreateS3Client(cloudFlareOptions, false);
            _bucketName = await EnsureBucketExistsAsync(_bucketName);

            //if (!extension.StartsWith("."))
            //    extension = "." + extension;
            string fileName = Guid.NewGuid().ToString();
            UploadFileResponse result = new();
            if (storageSetting.EnableEncryption)
            {
                var keyGenerator = KeyGenerator.GenerateKeyAndIV();
                var byteArray = AesOperation.EncryptFileToBytes(file, key, iv);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    var uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = _bucketName,
                        DisablePayloadSigning = true
                    };

                    try
                    {
                        await _r2Client.PutObjectAsync(uploadRequest);
                        result = new UploadFileResponse
                        {
                            FileName = fileName,
                            Key = key,
                            IV = iv
                        };
                        return result;

                    }
                    catch (AmazonS3Exception ex)
                    {
                        logger.LogError(ex, $"Error while uploading file in AWS storage: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error while uploading file in AWS storage: {ex.Message}");
                    }
                    return result;
                }
            }
            else
            {
                var uploadRequest = new PutObjectRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = fileName,
                    BucketName = _bucketName,
                    DisablePayloadSigning = true
                };

                await _r2Client.PutObjectAsync(uploadRequest);
                result = new UploadFileResponse { FileName = fileName };
            }

            return result;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in Cloudflare R2 storage: {ex.Message}");
            return new UploadFileResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in Cloudflare R2 storage: {ex.Message}");
            return new UploadFileResponse();
        }
    }

    private async Task UploadToR2(PutObjectRequest uploadRequest)
    {
        await _r2Client.PutObjectAsync(uploadRequest);
    }

    public async Task<UploadFileResponse> TestUploadFileAsync(IFormFile file, StorageSetting storageSetting, string extension)
    {
        try
        {
            var cloudFlareOptions = GetStorageSetting(storageSetting.JsonValue);
            await CreateS3Client(cloudFlareOptions, true);
            PutObjectRequest uploadRequest;
            UploadFileResponse result = new();
            if (storageSetting.EnableEncryption)
            {
                var keyGenerator = KeyGenerator.GenerateKeyAndIV();
                var byteArray = AesOperation.EncryptFileToBytes(file, keyGenerator.Item1, keyGenerator.Item2);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = file.FileName,
                        BucketName = _bucketName,
                        DisablePayloadSigning = true
                    };

                    await _r2Client.PutObjectAsync(uploadRequest);
                    result = new UploadFileResponse
                    {
                        FileName = file.FileName,
                        Key = keyGenerator.Item1,
                        IV = keyGenerator.Item2
                    };
                    return result;
                }
            }
            else
            {
                uploadRequest = new PutObjectRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = file.FileName,
                    BucketName = _bucketName,
                    DisablePayloadSigning = true
                };

                await _r2Client.PutObjectAsync(uploadRequest);
                result = new UploadFileResponse
                {
                    FileName = file.FileName
                };
            }
            return result;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, $"error while uploading test file in Cloudflare R2 storage: {ex.Message}");
            return new UploadFileResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while uploading test file in Cloudflare R2 storage: {ex.Message}");
            return new UploadFileResponse();
        }
    }

    public async Task<DownloadFileResponse> DownloadFileAsync(string fileName, string storageSetting, byte[] key, byte[] iv)
    {
        try
        {
            var cloudFlareOptions = GetStorageSetting(storageSetting);
            await CreateS3Client(cloudFlareOptions, false);
            var downloadRequest = new GetObjectRequest
            {
                Key = fileName,
                BucketName = _bucketName
            };
            var response = await _r2Client.GetObjectAsync(downloadRequest);
            if (key != null && iv != null)
            {
                var bytes = AesOperation.DecryptFileFromBytes(AesOperation.ConvertStreamToByteArray(response.ResponseStream), key, iv);
                return new DownloadFileResponse { FileBytes = bytes };
            }
            else
            {
                var bytes = AesOperation.ConvertStreamToByteArray(response.ResponseStream);
                return new DownloadFileResponse { FileBytes = bytes };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while downloading file in Cloudflare R2 storage: {ex.Message}");
            return new DownloadFileResponse
            {
                ErrorMessage = $"error while downloading file in Cloudflare R2 storage: {ex.Message}"
            };
        }
    }

    public async Task DeleteFileAsync(string fileName, string storageSetting)
    {
        try
        {
            var cloudFlareOptions = GetStorageSetting(storageSetting);
            await CreateS3Client(cloudFlareOptions, false);
            var deleteRequest = new DeleteObjectRequest
            {
                Key = fileName,
                BucketName = _bucketName
            };
            await _r2Client.DeleteObjectAsync(deleteRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while deleting file in Cloudflare R2 storage: {ex.Message}");
        }
    }

    private CloudflareOption GetStorageSetting(string storageSetting)
    {
        return JsonConvert.DeserializeObject<CloudflareOption>(storageSetting);
    }

    public async Task<UploadFileResponse> UploadBytesAsync(byte[] file, StorageSetting storageSetting, string extension)
    {
        try
        {
            var cloudFlareOptions = GetStorageSetting(storageSetting.JsonValue);
            await CreateS3Client(cloudFlareOptions, false);
            _bucketName = await EnsureBucketExistsAsync(_bucketName);
            if (!extension.StartsWith("."))
                extension = "." + extension;
            string fileName = Guid.NewGuid().ToString() + extension;

            UploadFileResponse result = new();
            if (storageSetting.EnableEncryption)
            {
                var keyGenerator = KeyGenerator.GenerateKeyAndIV();
                var byteArray = AesOperation.EncryptBytesToBytes(file, keyGenerator.Item1, keyGenerator.Item2);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    var uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = _bucketName,
                        DisablePayloadSigning = true
                    };

                    try
                    {
                        await _r2Client.PutObjectAsync(uploadRequest);
                        result = new UploadFileResponse
                        {
                            FileName = fileName,
                            Key = keyGenerator.Item1,
                            IV = keyGenerator.Item2
                        };
                        return result;

                    }
                    catch (AmazonS3Exception ex)
                    {
                        logger.LogError(ex, $"Error while uploading file in AWS storage: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error while uploading file in AWS storage: {ex.Message}");
                    }
                    return result;
                }
            }
            else
            {
                using (var memoryStream = new MemoryStream(file))
                {
                    var uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = _bucketName,
                        DisablePayloadSigning = true
                    };

                    await _r2Client.PutObjectAsync(uploadRequest);
                    result = new UploadFileResponse { FileName = fileName };
                }

            }

            return result;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in Cloudflare R2 storage: {ex.Message}");
            return new UploadFileResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in Cloudflare R2 storage: {ex.Message}");
            return new UploadFileResponse();
        }
    }
}
