using DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class MatTableSettingDto
    {
        public Guid Id { get; set; }
        public string ScreenName { get; set; }
        public List<TableSetting> Settings { get; set; } = new List<TableSetting>();
    }
}
