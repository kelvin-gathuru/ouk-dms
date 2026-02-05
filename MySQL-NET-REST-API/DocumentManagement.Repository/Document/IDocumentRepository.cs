using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;

namespace DocumentManagement.Repository;

public interface IDocumentRepository : IGenericRepository<Document>
{
    Task<DocumentList> GetDocuments(DocumentResource documentResource);
    Task<DocumentList> GetArchiveDocuments(DocumentResource documentResource);
    Task<DocumentList> GetDocumentsLibrary(string email, DocumentResource documentResource);
    Task<DocumentDto> GetDocumentById(Guid Id);
    Task<DocumentDto> GetDocumentSharedUsersRolesById(Guid Id);
    Task<string> GenerateDocumentNumberAsync();
    Task<string> GetDocumentName(string name, Guid categoryId, int index = 0);
    Task<int> UpdateIsSharedByCategoryIdAsync(List<Guid> affectedCategoryIds, bool isShared);
    Task UpdateDocumentSharingFlagAsync(List<Guid> affectedCategoryIds);
    Task<bool> HasCategorySharedPermissionsAsync(Guid categoryId);
}
