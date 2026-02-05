using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;


namespace DocumentManagement.MediatR.Commands
{
    public class GetNLogsQuery : IRequest<NLogList>
    {
        public NLogResource NLogResource { get; set; }
    }
}
