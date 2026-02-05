using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetTableSettingsQueryHandler(IMatTableSettingReposistory matTableSettingReposistory, IMapper mapper, UserInfoToken userInfoToken) : IRequestHandler<GetTableSettingsQuery, MatTableSettingDto>
    {
        public async Task<MatTableSettingDto> Handle(GetTableSettingsQuery request, CancellationToken cancellationToken)
        {
            var matTableSetting = await matTableSettingReposistory.All.Where(c=>c.ScreenName == request.ScreenName && c.UserId == userInfoToken.Id).FirstOrDefaultAsync();

            if(matTableSetting == null)
            {
               return new MatTableSettingDto();
            }
            else
            {
                return mapper.Map<MatTableSettingDto>(matTableSetting);
            }
        }
    }
}
