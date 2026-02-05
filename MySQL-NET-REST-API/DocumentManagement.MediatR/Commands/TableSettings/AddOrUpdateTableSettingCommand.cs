using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;


namespace DocumentManagement.MediatR.Commands
{
    public class AddOrUpdateTableSettingCommand: IRequest<ServiceResponse<MatTableSettingDto>>
    {
        public string ScreenName { get; set; }
        public List<TableSetting> Settings { get; set; } = new List<TableSetting>();
    }
}
