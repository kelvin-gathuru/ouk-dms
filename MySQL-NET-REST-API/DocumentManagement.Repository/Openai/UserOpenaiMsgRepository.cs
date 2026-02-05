using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository;
public class UserOpenaiMsgRepository : GenericRepository<UserOpenaiMsg, DocumentContext>,
          IUserOpenaiMsgRepository
{
    private readonly IPropertyMappingService _propertyMappingService;
    public UserOpenaiMsgRepository(
        IUnitOfWork<DocumentContext> uow,
        IPropertyMappingService propertyMappingService) : base(uow)
    {
        _propertyMappingService = propertyMappingService;
    }

    public async Task<UserOpenaiMsgList> GetUserOpenaiMsgAsync(UserOpenaiMsgResource userOpenaiMsgResource)
    {
        var collectionBeforePaging = All;
        collectionBeforePaging =
           collectionBeforePaging.ApplySort(userOpenaiMsgResource.OrderBy,
           _propertyMappingService.GetPropertyMapping<UserOpenaiMsgDto, UserOpenaiMsg>());

        if (!string.IsNullOrWhiteSpace(userOpenaiMsgResource.Title))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.Title, $"%{userOpenaiMsgResource.Title.Trim()}%"));
        }
        if (!string.IsNullOrWhiteSpace(userOpenaiMsgResource.PromptInput))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.PromptInput, $"%{userOpenaiMsgResource.PromptInput.Trim()}%"));
        }


        var userOpenaiMsgList = new UserOpenaiMsgList();
        return await userOpenaiMsgList.Create(
            collectionBeforePaging,
            userOpenaiMsgResource.Skip,
            userOpenaiMsgResource.PageSize
            );
    }
}