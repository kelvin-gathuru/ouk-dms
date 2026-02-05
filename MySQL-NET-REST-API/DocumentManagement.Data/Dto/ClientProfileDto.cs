using System;

namespace DocumentManagement.Data.Dto
{
    public class ClientProfileDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
