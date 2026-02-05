using DocumentManagement.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DocumentManagement.MediatR.Handlers.StorageStategies
{
    public class StorageServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public StorageServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IStorageService GetStorageService(StorageType storageType = StorageType.LOCAL_STORAGE)
        {
            return storageType switch
            {
                StorageType.LOCAL_STORAGE => _serviceProvider.GetService<LocalStorageService>(),
                StorageType.AWS_S3 => _serviceProvider.GetService<AwsS3StorageService>(),
                StorageType.CLOUDFLARER2 => _serviceProvider.GetService<CloudflareR2StorageService>(),
                _ => throw new Exception("Invalid storage type specified")
            };
        }
    }

}
