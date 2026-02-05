using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class ScreenDto : ErrorStatusCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; } = 0;
        public List<PageActionDto> Actions { get; set; }

    }
}
