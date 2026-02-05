using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddStorageSettingCommandHandler : IRequestHandler<AddStorageSettingCommand, ServiceResponse<StorageSettingDto<string>>>
    {
        private readonly IStorageSettingRepository _storageSettingRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly StorageServiceFactory _storeageServiceFactory;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddStorageSettingCommandHandler(IStorageSettingRepository storageSettingRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow,
            StorageServiceFactory storeageServiceFactory,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment)
        {
            _storageSettingRepository = storageSettingRepository;
            _mapper = mapper;
            _uow = uow;
            _storeageServiceFactory = storeageServiceFactory;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;

        }

        public async Task<ServiceResponse<StorageSettingDto<string>>> Handle(AddStorageSettingCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _storageSettingRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<StorageSettingDto<string>>.Return409("Storage Setting Name is already exists.");
            }

            string storagePath = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, _pathHelper.DocumentPath, _pathHelper.TestFile);
            IsDummyFileCreatedIfNotExist(storagePath);
            IFormFile file = DocumentManagement.Helper.FileHelper.ConvertToIFormFile(storagePath);
            var storageService = _storeageServiceFactory.GetStorageService(request.StorageType);
            var storageSetting = new StorageSetting
            {
                Name = request.Name,
                StorageType = request.StorageType,
                IsDefault = request.IsDefault,
                JsonValue = request.JsonValue,
                EnableEncryption = request.EnableEncryption

            };
            var keyValutFile = await storageService.TestUploadFileAsync(file, storageSetting,"");
            if (string.IsNullOrEmpty(keyValutFile.FileName))
            {
                return ServiceResponse<StorageSettingDto<string>>.Return422("Storage Setting is not properly setup.");
            }
            if (request.IsDefault)
            {
                var isDefaultEntity = _storageSettingRepository.All.Where(c => c.IsDefault == true).FirstOrDefault();
                if(isDefaultEntity != null)
                {
                    isDefaultEntity.IsDefault = false;
                    _storageSettingRepository.Update(isDefaultEntity);
                }
            }
            var entity = _mapper.Map<StorageSetting>(request);
            entity.Id = Guid.NewGuid();
            _storageSettingRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<StorageSettingDto<string>>.Return500();
            }
            return ServiceResponse<StorageSettingDto<string>>.ReturnResultWith201(_mapper.Map<StorageSettingDto<string>>(entity));
        }

        private bool IsDummyFileCreatedIfNotExist(string path)
        {
            if (File.Exists(path))
            {
               return true;
            }
            else
            {
                string dummyText = "This is a dummy text file.\n" +
                           "It contains some sample text for demonstration purposes.\n" +
                           "Feel free to modify this content as needed.\n\n" +
                           "Here are some bullet points:\n" +
                           "- Point 1: Dummy text example.\n" +
                           "- Point 2: More dummy text.\n" +
                           "- Point 3: Even more dummy text.";

                // Create the text file and write the dummy text into it
                try
                {
                    File.WriteAllText(path, dummyText);
                    return true;
                }
                catch (Exception ex)
                {
                   throw ex;
                }
            }
        }
    }
}
