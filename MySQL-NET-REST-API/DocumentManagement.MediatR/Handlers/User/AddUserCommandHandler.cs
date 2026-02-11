using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    readonly IUnitOfWork<DocumentContext> _uow;
    private readonly IMapper _mapper;
    private readonly EmailHelper _emailHelper;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AddUserCommandHandler> _logger;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public AddUserCommandHandler(
        IMapper mapper,
        UserManager<User> userManager,
        IUserRepository userRepository,
        IUnitOfWork<DocumentContext> uow,
        EmailHelper emailHelper,
        IConfiguration configuration,
        ILogger<AddUserCommandHandler> logger,
        IWebHostEnvironment hostingEnvironment)
    {
        _mapper = mapper;
        _userManager = userManager;
        _userRepository = userRepository;
        _uow = uow;
        _emailHelper = emailHelper;
        _configuration = configuration;
        _logger = logger;
        _hostingEnvironment = hostingEnvironment;
    }

    private string GeneratePassword()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
        var random = new Random();
        return new string(Enumerable.Range(0, 6)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());
    }

    public async Task<UserDto> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var allUser = await _userRepository.All.IgnoreQueryFilters().ToListAsync();
        var appUser = await _userRepository.All.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Email.ToLower() == request.Email.ToLower());
        if (appUser != null && appUser.IsDeleted)
        {
            var currentDateTime = $"{DateTime.UtcNow.Year}{DateTime.UtcNow.Month}{DateTime.UtcNow.Day}{DateTime.UtcNow.Hour}{DateTime.UtcNow.Minute}{DateTime.UtcNow.Second}{DateTime.UtcNow.Millisecond}";
            appUser.UserName = $"{appUser.UserName}_IsDeleted_{currentDateTime}";
            var email = appUser.Email.Split('@');

            appUser.Email = $"{email[0]}_IsDeleted_{currentDateTime}{email[1]}";
            appUser.NormalizedUserName = appUser.UserName.ToUpper();
            appUser.NormalizedEmail = appUser.Email.ToUpper();
            _userRepository.Update(appUser);
        }
        if (appUser != null && !appUser.IsDeleted)
        {
            var errorDto = new UserDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Email already exist for another user." }
            };
            return errorDto;
        }

        // Generate random password
        var generatedPassword = GeneratePassword();

        var entity = _mapper.Map<User>(request);
        entity.Id = Guid.NewGuid();
        entity.ClientId = Guid.NewGuid().ToString();
        entity.ClientSecretHash = ClientSecretGenerator.GenerateClientSecret();

        IdentityResult result = await _userManager.CreateAsync(entity);
        if (await _uow.SaveAsync() <= -1 && !result.Succeeded)
        {
            var errorDto = new UserDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }

        // Set the generated password
        string code = await _userManager.GeneratePasswordResetTokenAsync(entity);
        IdentityResult passwordResult = await _userManager.ResetPasswordAsync(entity, code, generatedPassword);
        if (!passwordResult.Succeeded)
        {
            var errorDto = new UserDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }

        // Send welcome email with credentials
        try
        {
            var smtpHost = _configuration["SmtpSettings:Host"];
            var smtpPort = _configuration.GetValue<int>("SmtpSettings:Port");
            var smtpUserName = _configuration["SmtpSettings:UserName"];
            var smtpPassword = _configuration["SmtpSettings:Password"];
            var smtpEncryptionType = _configuration["SmtpSettings:EncryptionType"];
            var smtpFromEmail = _configuration["SmtpSettings:FromEmail"];
            var smtpFromName = _configuration["SmtpSettings:FromName"];
            var loginUrl = _configuration["JwtSettings:issuer"];

            if (!string.IsNullOrEmpty(smtpHost) && !string.IsNullOrEmpty(entity.Email))
            {
                var emailTemplatePath = Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplates", "WelcomeEmail.html");
                var emailTemplateContent = System.IO.File.ReadAllText(emailTemplatePath);

                var emailBody = emailTemplateContent
                    .Replace("##USER_NAME##", $"{entity.FirstName} {entity.LastName}")
                    .Replace("##EMAIL##", entity.Email)
                    .Replace("##PASSWORD##", generatedPassword)
                    .Replace("##LOGIN_URL##", loginUrl);

                var emailSpec = new SendEmailSpecification
                {
                    UserName = smtpUserName,
                    Password = smtpPassword,
                    FromAddress = smtpFromEmail,
                    ToAddress = entity.Email,
                    Body = emailBody,
                    Host = smtpHost,
                    Port = smtpPort,
                    Subject = "Welcome to OUK DMS - Your Account Credentials",
                    CCAddress = "",
                    EncryptionType = smtpEncryptionType,
                    FromName = smtpFromName
                };

                await _emailHelper.SendEmail(emailSpec);
                _logger.LogInformation($"Welcome email sent successfully to {entity.Email} with password: {generatedPassword}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send welcome email to {entity.Email}. Generated password: {generatedPassword}");
            // Don't fail user creation if email fails - password is logged
        }

        return _mapper.Map<UserDto>(entity);
    }
}
