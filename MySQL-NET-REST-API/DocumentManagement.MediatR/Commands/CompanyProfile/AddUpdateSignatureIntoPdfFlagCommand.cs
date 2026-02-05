using System;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class AddUpdateSignatureIntoPdfFlagCommand : IRequest<ServiceResponse<bool>>
{
    public Guid Id { get; set; }
    public bool AllowSignatureIntoPdf { get; set; }
}