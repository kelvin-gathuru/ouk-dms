
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface ISendEmailRepository : IGenericRepository<SendEmail>
    {
        void AddSharedEmails(SendEmail sendEmail, string documentName);
        void AddTransitionEmails(SendEmail sendEmail, string documentName, string transitionName, string workflowName);
        void AddFileRequestEmails(SendEmail sendEmail, string url);
        void AddFileRequestDocumentEmails(SendEmail sendEmail);
        void AddSharedFolderEmails(SendEmail sendEmail, string folderName);
    }
}
