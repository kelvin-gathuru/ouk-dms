using System;

namespace DocumentManagement.Data.Entities;
public class AIPromptTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PromptInput { get; set; }
    public bool IsActive { get; set; }
    public DateTime ModifiedDate { get; set; }
}
