using System;

namespace DocumentManagement.Data.Dto
{
    public class OperationDto: ErrorStatusCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
