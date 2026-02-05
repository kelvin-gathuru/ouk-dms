using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class FileRequestDocumentProfile:Profile
    {
        public FileRequestDocumentProfile()
        {
            CreateMap<FileRequestDocument, FileRequestDocumentDto>().ReverseMap();
            CreateMap<AddFileRequestDocumentCommand, FileRequestDocument>();
        }
    }
}
