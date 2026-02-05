using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class AddFileRequestCommand : IRequest<ServiceResponse<FileRequestDto>>
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? SizeInMb { get; set; }
        public int? MaxDocument { get; set; }
        public int[] FileExtension { get; set; }
        public DateTime? LinkExpiryTime { get; set; }
        public string BaseUrl { get; set; }
    }
}
