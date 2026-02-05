using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository;
public class UserOpenaiMsgList : List<UserOpenaiMsgDto>
{
    public int Skip { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public UserOpenaiMsgList(List<UserOpenaiMsgDto> items, int count, int skip, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        Skip = skip;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public UserOpenaiMsgList()
    {
    }

    public async Task<UserOpenaiMsgList> Create(IQueryable<UserOpenaiMsg> source, int skip, int pageSize)
    {
        var count = await GetCount(source);
        var dtoList = await GetDtos(source, skip, pageSize);
        var dtoPageList = new UserOpenaiMsgList(dtoList, count, skip, pageSize);
        return dtoPageList;
    }

    public async Task<int> GetCount(IQueryable<UserOpenaiMsg> source)
    {
        return await source.AsNoTracking().CountAsync();
    }

    public async Task<List<UserOpenaiMsgDto>> GetDtos(IQueryable<UserOpenaiMsg> source, int skip, int pageSize)
    {
        var entities = await source
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .Select(c => new UserOpenaiMsgDto
            {
                Id = c.Id,
                Title = c.Title,
                PromptInput = c.PromptInput,
                Language = c.Language,
                MaximumLength = c.MaximumLength,
                Creativity = c.Creativity,
                ToneOfVoice = c.ToneOfVoice,
                SelectedModel = c.SelectedModel,
                CreatedDate = c.CreatedDate
            }).ToListAsync();

        return entities;
    }
}