using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class ClientSignupCommandHandler : IRequestHandler<ClientSignupCommand, ServiceResponse<Guid>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        private readonly EmailHelper _emailHelper;
        private readonly IConfiguration _configuration;

        public ClientSignupCommandHandler(
            IClientRepository clientRepository,
            IUnitOfWork<DocumentContext> uow,
            IMapper mapper,
            EmailHelper emailHelper,
            IConfiguration configuration)
        {
            _clientRepository = clientRepository;
            _uow = uow;
            _mapper = mapper;
            _emailHelper = emailHelper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<Guid>> Handle(ClientSignupCommand request, CancellationToken cancellationToken)
        {
            var existingClient = await _clientRepository.FindBy(c => c.Email == request.Email).FirstOrDefaultAsync();
            if (existingClient != null)
            {
                return ServiceResponse<Guid>.ReturnFailed(409, "Email already exists.");
            }

            var client = new Client
            {
                Id = Guid.NewGuid(),
                CompanyName = request.CompanyName,
                ContactPerson = request.ContactPerson,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                IsActivated = false,
                ActivationCode = new Random().Next(100000, 999999).ToString(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreatedBy = Guid.Empty, // Self-registered
                ModifiedBy = Guid.Empty
            };

            var passwordHasher = new PasswordHasher<object>();
            client.Password = passwordHasher.HashPassword(null, request.Password);

            _clientRepository.Add(client);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<Guid>.Return500();
            }

            // Send Activation Email
            try 
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var sendEmailSpecification = new SendEmailSpecification
                {
                    FromAddress = smtpSettings["FromEmail"],
                    FromName = smtpSettings["FromName"],
                    ToAddress = client.Email,
                    ToName = client.ContactPerson,
                    Subject = "Activate your Parliament System Account",
                    Body = $@"
                        <h3>Welcome to Parliament System</h3>
                        <p>Dear {client.ContactPerson},</p>
                        <p>Thank you for registering. Please use the following code to activate your account:</p>
                        <h2>{client.ActivationCode}</h2>
                        <p>If you did not request this, please ignore this email.</p>",
                    Host = smtpSettings["Host"],
                    Port = int.Parse(smtpSettings["Port"]),
                    UserName = smtpSettings["UserName"],
                    Password = smtpSettings["Password"],
                    EncryptionType = smtpSettings["EncryptionType"]
                };

                await _emailHelper.SendEmail(sendEmailSpecification);
            }
            catch (Exception)
            {
                // Log error but don't fail the request, user can request resend later if needed
                // In a real app, we might want to return a warning
            }
            
            return ServiceResponse<Guid>.ReturnResultWith201(client.Id, "Signup successful! Please check your email for activation code.");
        }
    }
}
