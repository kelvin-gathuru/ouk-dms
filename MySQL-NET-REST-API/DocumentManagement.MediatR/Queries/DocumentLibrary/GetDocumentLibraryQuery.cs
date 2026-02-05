namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumentLibraryQuery : GetAllDocumentQuery
    {
        public string Email { get; set; }
    }
}
