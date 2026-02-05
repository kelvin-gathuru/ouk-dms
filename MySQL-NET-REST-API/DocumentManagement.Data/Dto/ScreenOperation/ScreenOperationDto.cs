using System;


namespace DocumentManagement.Data.Dto
{
    public class ScreenOperationDto: ErrorStatusCode
    {
        public Guid Id { get; set; }
        public Guid OperationId { get; set; }
        public Guid ScreenId { get; set; }
        public bool Flag { get; set; } = true;
    }
}
