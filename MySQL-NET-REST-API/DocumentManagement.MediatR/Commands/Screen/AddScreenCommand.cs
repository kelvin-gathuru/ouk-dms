using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class AddScreenCommand : IRequest<ScreenDto>
    {
        public string Name { get; set; }
        public int OrderNo { get; set; }
    }
}
