using System;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddFileRequestCommandHandler (UserInfoToken _userInfoToken, IUnitOfWork<DocumentContext> _uow, IMapper _mapper, IFileRequestsRepository _fileRequestsRepository, IUserRepository _userRepository, ISendEmailRepository _sendEmailRepository, IAllowFileExtensionRepository _allowFileExtensionRepository) : IRequestHandler<AddFileRequestCommand, ServiceResponse<FileRequestDto>>
    {
        public async Task<ServiceResponse<FileRequestDto>> Handle(AddFileRequestCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _fileRequestsRepository.FindBy(c => c.Subject == request.Subject).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<FileRequestDto>.Return409("File request with subject same already exists.");
            }
            string fileExtensions = "";
            foreach (var item in request.FileExtension)
            {
                var fileExtension = _allowFileExtensionRepository.FindBy(c => c.FileType == (FileType)item).FirstOrDefault();
                if (fileExtension != null)
                {
                    fileExtensions += fileExtension.FileType + ",";
                }
            }
            if (!string.IsNullOrEmpty(fileExtensions))
            {
                fileExtensions = fileExtensions.TrimEnd(',');
            }
            var entity = _mapper.Map<FileRequest>(request);
            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(entity.Password);
                entity.Password = Convert.ToBase64String(plainTextBytes);
            }
            entity.Id = Guid.NewGuid();
            entity.SizeInMb = request.SizeInMb;
            entity.AllowExtension = fileExtensions;
            entity.CreatedById = _userInfoToken.Id;
            entity.CreatedDate = DateTime.UtcNow;
            _fileRequestsRepository.Add(entity);
            var currentUserInfo = _userRepository.Find(_userInfoToken.Id);
            if (!string.IsNullOrEmpty(entity.Email))
            {
                var url = request.BaseUrl + entity.Id;
                _sendEmailRepository.AddFileRequestEmails(new SendEmail
                {
                    Email = entity.Email,
                    FromEmail = currentUserInfo.Email,
                    FromName = currentUserInfo.FirstName + ' ' + currentUserInfo.LastName,
                    CreatedBy = currentUserInfo.Id,
                    CreatedDate = DateTime.UtcNow,
                }, url);
            }
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<FileRequestDto>.Return500();
            }
            var entityDto = _mapper.Map<FileRequestDto>(entity);
            if (!string.IsNullOrWhiteSpace(entityDto.Password))
            {
                var base64EncodedBytes = Convert.FromBase64String(entityDto.Password);
                entityDto.Password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            return ServiceResponse<FileRequestDto>.ReturnResultWith201(entityDto);
        }
    }
}
