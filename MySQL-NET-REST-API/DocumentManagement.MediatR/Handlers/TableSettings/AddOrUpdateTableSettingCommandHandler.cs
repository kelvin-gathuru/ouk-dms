using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddOrUpdateTableSettingCommandHandler(
        IMatTableSettingReposistory matTableSettingReposistory, 
        IMapper mapper, 
        UserInfoToken userInfoToken, 
        IUnitOfWork<DocumentContext> _uow) : IRequestHandler<AddOrUpdateTableSettingCommand, ServiceResponse<MatTableSettingDto>>
    {
        public async Task<ServiceResponse<MatTableSettingDto>> Handle(AddOrUpdateTableSettingCommand request, CancellationToken cancellationToken)
        {
            var matTableSetting = await matTableSettingReposistory.All.Where(c => c.ScreenName == request.ScreenName && c.UserId == userInfoToken.Id).FirstOrDefaultAsync();
            if (matTableSetting !=null)
            {
                matTableSetting.Settings = request.Settings;
                matTableSettingReposistory.Update(matTableSetting);
            }
            else
            {
                matTableSetting = new MatTableSetting
                {
                    ScreenName = request.ScreenName,
                    UserId = userInfoToken.Id,
                    Settings = request.Settings
                };
                matTableSettingReposistory.Add(matTableSetting);
            }
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<MatTableSettingDto>.Return500();
            }
           var matTableSettingDto=   mapper.Map<MatTableSettingDto>(matTableSetting);
            return ServiceResponse<MatTableSettingDto>.ReturnResultWith200(matTableSettingDto);
        }
    }
}
