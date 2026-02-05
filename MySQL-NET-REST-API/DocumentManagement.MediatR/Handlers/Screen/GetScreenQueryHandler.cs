using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetScreenQueryHandler(IScreenRepository _screenRepository, IMapper _mapper) : IRequestHandler<GetScreenQuery, ServiceResponse<ScreenDto>>
    {
        public async Task<ServiceResponse<ScreenDto>> Handle(GetScreenQuery request, CancellationToken cancellationToken)
        {
            var entity = await _screenRepository.FindAsync(request.Id);
            if (entity != null)
                return ServiceResponse<ScreenDto>.ReturnResultWith200(_mapper.Map<ScreenDto>(entity));
            else
            {
                return ServiceResponse<ScreenDto>.Return404();
            }
        }
    }
}
