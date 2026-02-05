using System;

namespace DocumentManagement.Data.Dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
    }
}
