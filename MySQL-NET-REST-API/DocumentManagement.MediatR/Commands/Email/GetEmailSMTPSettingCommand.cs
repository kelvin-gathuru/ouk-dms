using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GetEmailSMTPSettingCommand: IRequest<ServiceResponse<bool>>
    {
    }
}
