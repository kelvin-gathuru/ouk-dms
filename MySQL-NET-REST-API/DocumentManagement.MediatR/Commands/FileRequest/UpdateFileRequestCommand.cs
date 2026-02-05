using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateFileRequestCommand : IRequest<ServiceResponse<FileRequestDto>>
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? SizeInMb { get; set; }
        public int? MaxDocument { get; set; }
        public int[] FileExtension { get; set; }
        public DateTime? LinkExpiryTime { get; set; }
    }
}
