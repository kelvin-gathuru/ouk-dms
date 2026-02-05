using Amazon;
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
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.StorageStategies;

public class AwsS3StorageService(IConfiguration Configuration,
    ILogger<AwsS3StorageService> logger) : IStorageService
{
    private IAmazonS3 _r2Client; // Cloudflare R2 is S3-compatible.
    private string _bucketName = string.Empty;

    private async Task<bool> CreateS3Client(AwsOption storageSetting, bool isTest)
    {
        try
        {
            var accessKey = storageSetting.AccessKey ?? Configuration["AWS:AccessKey"];
            var secretKey = storageSetting.SecretKey ?? Configuration["AWS:SecretKey"];
            var region = storageSetting.Region ?? Configuration["AWS:Region"];

            if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(region))
            {
                logger.LogError("AWS credentials or region are missing.");
                throw new ArgumentException("AWS credentials or region are missing.");
            }

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region)
            };

            _r2Client = new AmazonS3Client(credentials, config);
            var isBucketNameExist = await EnsureBucketExistsAsync(storageSetting.BucketName ?? Configuration["AWS:BucketName"]);
            if (string.IsNullOrEmpty(isBucketNameExist))
            {
                return false;
            }
            _bucketName = isTest ? isBucketNameExist : storageSetting.BucketName;
            return true;
        }
        catch (AmazonS3Exception s3Ex)
        {
            var error = "error while uploading file in AWS storage.";
            logger.LogError(s3Ex, error, s3Ex.Message);
            return false;
        }
        catch (ArgumentException argEx)
        {
            var error = "error while uploading file in AWS storage.";
            logger.LogError(argEx, error, argEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            var error = "error while uploading file in AWS storage.";
            logger.LogError(ex, error, ex.Message);
            throw;
        }
    }

    private async Task<string> EnsureBucketExistsAsync(string bucketName)
    {
        try
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                // Check if the bucket exists
                var listResponse = await _r2Client.ListBucketsAsync(cts.Token);
                var bucketExists = listResponse.Buckets.Any(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));

                if (!bucketExists)
                {
                    // Create the bucket
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    await _r2Client.PutBucketAsync(putBucketRequest);
                }

                return bucketName;
            }
        }
        catch (AmazonS3Exception ex)
        {
            var error = "error while uploading file in AWS storage.";
            logger.LogError(ex, error, ex.Message);
            return "";
        }
        catch (Exception ex)
        {
            var error = "error while uploading file in AWS storage.";
            logger.LogError(ex, error, ex.Message);
            return "";
        }
    }

    public async Task<UploadFileResponse> UploadFileAsync(IFormFile file, StorageSetting storageSetting, string extension)
    {
        try
        {
            var awsOptions = GetStorageSetting(storageSetting.JsonValue);
            var isBucketExist = await CreateS3Client(awsOptions, true);
            if (!isBucketExist)
            {
                logger.LogError("Bucket does not exist or Not able to create new bucket.");
                return new UploadFileResponse();
            }
            PutObjectRequest uploadRequest;
            UploadFileResponse result = new();
            if (string.IsNullOrEmpty(extension))
                extension = MimeTypeHelper.GetFileExtension(file.ContentType);
            if (!extension.StartsWith("."))
                extension = "." + extension;
            string fileName = Guid.NewGuid().ToString() + extension;
            if (storageSetting.EnableEncryption)
            {
                var keyGenerator = KeyGenerator.GenerateKeyAndIV();
                var byteArray = AesOperation.EncryptFileToBytes(file, keyGenerator.Item1, keyGenerator.Item2);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = _bucketName
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
                        logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
                    }
                    return result;
                }
            }
            else
            {
                uploadRequest = new PutObjectRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = fileName,
                    BucketName = _bucketName
                };
                await _r2Client.PutObjectAsync(uploadRequest);
                result = new UploadFileResponse { FileName = fileName };
            }
            return result;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
            return new UploadFileResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
            return new UploadFileResponse();
        }

    }


    public async Task<UploadFileResponse> UploadFileChunkAsync(IFormFile file, StorageSetting storageSetting, string extension, byte[] key, byte[] iv)
    {
        try
        {
            var awsOptions = GetStorageSetting(storageSetting.JsonValue);
            var isBucketExist = await CreateS3Client(awsOptions, true);
            if (!isBucketExist)
            {
                logger.LogError("Bucket does not exist or Not able to create new bucket.");
                return new UploadFileResponse();
            }
            PutObjectRequest uploadRequest;
            UploadFileResponse result = new();
            //if (string.IsNullOrEmpty(extension))
            //    extension = MimeTypeHelper.GetFileExtension(file.ContentType);
            //if (!extension.StartsWith("."))
            //    extension = "." + extension;
            string fileName = Guid.NewGuid().ToString();
            if (storageSetting.EnableEncryption)
            {
                var byteArray = AesOperation.EncryptFileToBytes(file, key, iv);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = _bucketName
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
                        logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
                    }
                    return result;
                }
            }
            else
            {
                uploadRequest = new PutObjectRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = fileName,
                    BucketName = _bucketName
                };
                await _r2Client.PutObjectAsync(uploadRequest);
                result = new UploadFileResponse { FileName = fileName };
            }
            return result;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
            return new UploadFileResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
            return new UploadFileResponse();
        }

    }

    public async Task<UploadFileResponse> TestUploadFileAsync(IFormFile file, StorageSetting storageSetting, string extension)
    {
        var awsOptions = GetStorageSetting(storageSetting.JsonValue);
        var isBucketExist = await CreateS3Client(awsOptions, true);
        if (!isBucketExist)
        {
            logger.LogError("Bucket does not exist or Not able to create new bucket.");
            return new UploadFileResponse();
        }
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
                    BucketName = _bucketName
                };

                try
                {
                    await _r2Client.PutObjectAsync(uploadRequest);
                    result = new UploadFileResponse
                    {
                        FileName = file.FileName,
                        Key = keyGenerator.Item1,
                        IV = keyGenerator.Item2
                    };
                    return result;
                }
                catch (AmazonS3Exception ex)
                {
                    logger.LogError(ex, $"error while uploading file in AWS storage. {ex.Message}", ex.Message);
                    return new UploadFileResponse();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"error while uploading file in AWS storage.{ex.Message}", ex.Message);
                    return new UploadFileResponse();
                }
            }
        }
        else
        {
            uploadRequest = new PutObjectRequest
            {
                InputStream = file.OpenReadStream(),
                Key = file.FileName,
                BucketName = _bucketName
            };

            try
            {
                await _r2Client.PutObjectAsync(uploadRequest);
                result.FileName = file.FileName;
                return result;

            }
            catch (AmazonS3Exception ex)
            {
                logger.LogError(ex, $"error while uploading file in AWS storage.{ex.Message}", ex.Message);
                return new UploadFileResponse();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"error while uploading file in AWS storage., {ex.Message}", ex.Message);
                return new UploadFileResponse();
            }
        }
    }

    public async Task<DownloadFileResponse> DownloadFileAsync(string fileName, string storageSetting, byte[] key, byte[] iv)
    {
        try
        {
            var awsOptions = GetStorageSetting(storageSetting);
            var isBucketExist = await CreateS3Client(awsOptions, true);
            if (!isBucketExist)
            {
                logger.LogError("Bucket does not exist or Not able to create new bucket.");
                return new DownloadFileResponse() { ErrorMessage = "Bucket does not exist or Not able to create new bucket." };
            }
            var downloadRequest = new GetObjectRequest
            {
                Key = fileName,
                BucketName = _bucketName
            };

            var response = await _r2Client.GetObjectAsync(downloadRequest);

            if (key != null && iv != null)
            {
                var bytes = AesOperation.DecryptFileFromBytes(AesOperation.ConvertStreamToByteArray(response.ResponseStream), key, iv);
                return new DownloadFileResponse
                {
                    FileBytes = bytes
                };
            }
            else
            {
                var bytes = AesOperation.ConvertStreamToByteArray(response.ResponseStream);
                return new DownloadFileResponse
                {
                    FileBytes = bytes
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while downloading file in AWS storage: {ex.Message}");
            return new DownloadFileResponse() { ErrorMessage = $"error while downloading file in AWS storage: {ex.Message}" };
        }
    }

    public async Task DeleteFileAsync(string fileName, string storageSetting)
    {
        try
        {
            var awsOptions = GetStorageSetting(storageSetting);
            var isBucketExist = await CreateS3Client(awsOptions, true);
            if (!isBucketExist)
            {
                logger.LogError("Bucket does not exist or Not able to create new bucket.");
                return;
            }
            var deleteRequest = new DeleteObjectRequest
            {
                Key = fileName,
                BucketName = _bucketName
            };
            await _r2Client.DeleteObjectAsync(deleteRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while deleting file in AWS storage: {ex.Message}");
        }
    }
    private AwsOption GetStorageSetting(string storageSetting)
    {
        return JsonConvert.DeserializeObject<AwsOption>(storageSetting);
    }

    public async Task<UploadFileResponse> UploadBytesAsync(byte[] file, StorageSetting storageSetting, string extension)
    {
        try
        {
            var awsOptions = GetStorageSetting(storageSetting.JsonValue);
            var isBucketExist = await CreateS3Client(awsOptions, true);
            if (!isBucketExist)
            {
                logger.LogError("Bucket does not exist or Not able to create new bucket.");
                return new UploadFileResponse();
            }
            if (!extension.StartsWith("."))
                extension = "." + extension;
            string fileName = Guid.NewGuid().ToString() + extension;
            PutObjectRequest uploadRequest;
            UploadFileResponse result = new();
            if (storageSetting.EnableEncryption)
            {
                var keyGenerator = KeyGenerator.GenerateKeyAndIV();
                var byteArray = AesOperation.EncryptBytesToBytes(file, keyGenerator.Item1, keyGenerator.Item2);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = _bucketName
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
                        logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
                    }
                    return result;
                }
            }
            else
            {
                using (var memoryStream = new MemoryStream(file))
                {

                    uploadRequest = new PutObjectRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = _bucketName
                    };
                    await _r2Client.PutObjectAsync(uploadRequest);
                    result = new UploadFileResponse { FileName = fileName };
                }
            }
            return result;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
            return new UploadFileResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error while uploading file in AWS storage: {ex.Message}");
            return new UploadFileResponse();
        }
    }
}
