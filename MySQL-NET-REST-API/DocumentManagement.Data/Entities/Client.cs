using System;

namespace DocumentManagement.Data.Entities
{
    public class Client : BaseEntity
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public bool IsActivated { get; set; }
        public string? ActivationCode { get; set; }
    }
}
