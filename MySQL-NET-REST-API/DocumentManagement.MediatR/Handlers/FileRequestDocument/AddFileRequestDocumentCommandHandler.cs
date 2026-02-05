using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddFileRequestDocumentCommandHandler(IUserNotificationRepository _userNotificationRepository,UserInfoToken _userInfo,IUserRepository _userRepository,ISendEmailRepository _sendEmailRepository,IFileRequestsRepository _fileRequestsRepository,IFileRequestDocumentRepository _fileRequestDocumentRepository, IUnitOfWork<DocumentContext> _uow, IMapper _mapper, PathHelper _pathHelper, IWebHostEnvironment _webHostEnvironment) : IRequestHandler<AddFileRequestDocumentCommand, ServiceResponse<List<FileRequestDocumentDto>>>
    {
        public async Task<ServiceResponse<List<FileRequestDocumentDto>>> Handle(AddFileRequestDocumentCommand request, CancellationToken cancellationToken)
        {
            if (request.Files == null)
            {
                return ServiceResponse<List<FileRequestDocumentDto>>.ReturnFailed(409, "Please select the file.");
            }
            if (!FileSignatureHelper.IsFileSignatureValid(request.Files[0]))
            {
                return ServiceResponse<List<FileRequestDocumentDto>>.ReturnFailed(409, "Invalid file signature.");
            }
            try
            {
                var fileRequest =await _fileRequestsRepository.FindBy(c => c.Id == request.FileRequestId).FirstOrDefaultAsync();
                if(fileRequest == null)
                {
                    return ServiceResponse<List<FileRequestDocumentDto>>.Return409("File request does not exists.");
                }
                string storagePath = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.FileRequestPath);
                if (!Directory.Exists(storagePath))
                {
                    Directory.CreateDirectory(storagePath);
                }
                var fileRequestDocuments = new List<FileRequestDocument>();
                var fileRequestDocumentsUserNotification = new List<Data.UserNotification>();
                foreach (var item in request.Files)
                {
                    int index = request.Files.IndexOf(item);
                    var currentName = string.Empty;
                    if (index < request.Names.Count)
                    {
                        currentName = request.Names[index];
                        var entityExist = await _fileRequestDocumentRepository.FindBy(c => c.Name == currentName).FirstOrDefaultAsync();
                        if (entityExist != null)
                        {
                            return ServiceResponse<List<FileRequestDocumentDto>>.ReturnFailed(409, "Document already exist.");
                        }
                    }
                    else
                    {
                        return ServiceResponse<List<FileRequestDocumentDto>>.ReturnFailed(400, "Invalid request.Names collection.");
                    }
         
                    var fileExtension = item.FileName.Split(".")[1];
                    fileExtension = "." + fileExtension; 
                    string uri = $"{Guid.NewGuid()}{fileExtension}";
                    string fullPath = Path.Combine(storagePath, uri);
                    UploadFileResponse result = new();
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        var bytesData = AesOperation.ConvertIFormFileToByteArray(item);
                        await stream.WriteAsync(bytesData, 0, bytesData.Length);
                    }
                    var entity = _mapper.Map<FileRequestDocument>(request);
                    entity.Id = Guid.NewGuid();
                    entity.Name = currentName;
                    entity.Url = uri;
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.FileRequestId = request.FileRequestId;
                    entity.FileRequestDocumentStatus = FileRequestDocumentStatus.PENDING;
                    fileRequestDocuments.Add(entity);
                }
                _fileRequestDocumentRepository.AddRange(fileRequestDocuments);
                fileRequest.FileRequestStatus = FileRequestStatus.UPLOADED;
                _fileRequestsRepository.Update(fileRequest);
                foreach (var item in fileRequestDocuments)
                {
                    _userNotificationRepository.CreateUserNotificationFileRequestDocument(fileRequest.CreatedById, item.Id,fileRequest.Subject,item.Name);
                }
                var requestUserInfo = _userRepository.Find(fileRequest.CreatedById);
                _sendEmailRepository.AddFileRequestDocumentEmails(new SendEmail
                {
                    Email = requestUserInfo.Email,
                    FromEmail = requestUserInfo.Email,
                    FromName = requestUserInfo.FirstName + ' ' + requestUserInfo.LastName,
                    ToName = requestUserInfo.FirstName + ' ' + requestUserInfo.LastName,
                    CreatedBy = _userInfo.Id,
                    CreatedDate = DateTime.UtcNow,
                });
                if (await _uow.SaveAsync() <= 0)
                {
                    return ServiceResponse<List<FileRequestDocumentDto>>.Return500();
                }
                var entityDto = _mapper.Map<List<FileRequestDocumentDto>>(fileRequestDocuments);
                return ServiceResponse<List<FileRequestDocumentDto>>.ReturnResultWith200(entityDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to write file: {ex.Message}", ex);
            }
        }
    }
}
