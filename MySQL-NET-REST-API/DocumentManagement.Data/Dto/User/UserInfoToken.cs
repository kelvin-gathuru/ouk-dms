using System;

namespace DocumentManagement.Data.Dto;

public class UserInfoToken
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string ConnectionId { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsClient { get; set; }
}
