using Azure;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class SendEmailRepository : GenericRepository<SendEmail, DocumentContext>,
           ISendEmailRepository
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendEmailRepository(IUnitOfWork<DocumentContext> uow,
            IWebHostEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor) : base(uow)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddSharedEmails(SendEmail sendEmail, string documentName)
        {
            var requestContext = _httpContextAccessor.HttpContext.Request;
            var url = $"{requestContext.Scheme}://{requestContext.Host}";
            var emailTemplatePath = Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplates", "SharedEmail.html");
            var emailTemplateContent = System.IO.File.ReadAllText(emailTemplatePath);
            sendEmail.Subject = $"{sendEmail.ToName} shared \"{documentName}\" with you";
            sendEmail.Message = emailTemplateContent
                .Replace("##TO_NAME##", sendEmail.ToName)
                .Replace("##FROM_NAME##", sendEmail.FromName)
                .Replace("##LINK##", url);
            Add(sendEmail);
        }

        public void AddSharedFolderEmails(SendEmail sendEmail, string folderName)
        {
            var requestContext = _httpContextAccessor.HttpContext.Request;
            var url = $"{requestContext.Scheme}://{requestContext.Host}";
            var emailTemplatePath = Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplates", "SharedFolderEmail.html");
            var emailTemplateContent = System.IO.File.ReadAllText(emailTemplatePath);
            sendEmail.Subject = $"{sendEmail.ToName} shared \"{folderName}\" with you";
            sendEmail.Message = emailTemplateContent
                .Replace("##TO_NAME##", sendEmail.ToName)
                .Replace("##FROM_NAME##", sendEmail.FromName)
                .Replace("##LINK##", url);
            Add(sendEmail);
        }

        public void AddTransitionEmails(SendEmail sendEmail, string documentName, string transitionName, string workflowName)
        {
            var emailTemplatePath = Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplates", "TransitionEmail.html");
            var emailTemplateContent = System.IO.File.ReadAllText(emailTemplatePath);
            sendEmail.Subject = $"You have been assigned the task of ${transitionName} the workflow ${workflowName} for the document named ${documentName}";
            sendEmail.Message = emailTemplateContent
                .Replace("##TO_NAME##", sendEmail.ToName)
                .Replace("##TRANSITION_NAME##", transitionName)
                .Replace("##WORKFLOW_NAME##", workflowName)
                .Replace("##DOCUMENT_NAME##", documentName);
            Add(sendEmail);
        }

        public void AddFileRequestEmails(SendEmail sendEmail,string url)
        {
            var requestContext = _httpContextAccessor.HttpContext.Request;
            var emailTemplatePath = Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplates", "FileRequestEmail.html");
            var emailTemplateContent = System.IO.File.ReadAllText(emailTemplatePath);
            sendEmail.Subject = $"{sendEmail.ToName} has requested you to upload a file.";
            sendEmail.Message = emailTemplateContent
                .Replace("##TO_NAME##", sendEmail.ToName)
                .Replace("##FROM_NAME##", sendEmail.FromName)
                .Replace("##UPLOAD_LINK##", url);
            Add(sendEmail);
        }

        public void AddFileRequestDocumentEmails(SendEmail sendEmail)
        {
            var emailTemplatePath = Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplates", "FileRequestDocumentEmail.html");
            var emailTemplateContent = System.IO.File.ReadAllText(emailTemplatePath);
            sendEmail.Subject = $"{sendEmail.ToName}  requested file has been successfully uploaded";
            sendEmail.Message = emailTemplateContent
                .Replace("##TO_NAME##", sendEmail.ToName);
            Add(sendEmail);
        }
    }
}
