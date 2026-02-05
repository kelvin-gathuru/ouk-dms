using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Entities;
public class PageAction
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public Guid PageId { get; set; }
    [ForeignKey("PageId")]
    public Screen Page { get; set; }
    public string Code { get; set; }
}
