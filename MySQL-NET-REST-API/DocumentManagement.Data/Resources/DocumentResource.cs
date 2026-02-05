using System;

namespace DocumentManagement.Data.Resources;

public class DocumentResource : ResourceParameter
{
    public DocumentResource() : base("Name")
    {
    }
    public string Name { get; set; }
    public string Id { get; set; }
    public string CategoryId { get; set; }
    public string Operation { get; set; }
    public string DocumentStatusId { get; set; }
    public string StorageSettingId { get; set; }
    public string ClientId { get; set; }
    public DateTime? CreateDate { get; set; }
    public string CreateDateString { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string CreatedBy { get; set; }
    public string MetaTags { get; set; }
    public bool IsArchive { get; set; }
    public string DocumentNumber { get; set; }
    public string MetaTagsTypeId { get; set; }
}
