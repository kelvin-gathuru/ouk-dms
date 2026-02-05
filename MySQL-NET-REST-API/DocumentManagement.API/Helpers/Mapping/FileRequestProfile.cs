using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class FileRequestProfile: Profile
    {
        public FileRequestProfile()
        {
            CreateMap<FileRequest, FileRequestDto>().ReverseMap();
            CreateMap<AddFileRequestCommand, FileRequest>();
            CreateMap<UpdateFileRequestCommand, FileRequest>();
        }
    }
}
