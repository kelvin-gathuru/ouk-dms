using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


namespace DocumentManagement.Data.Entities
{
    public class MatTableSetting : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string ScreenName { get; set; }
        public Guid? UserId { get; set; }

        [NotMapped]
        public List<TableSetting> Settings { get; set; } = new List<TableSetting>();

        // Backing field to store JSON in the database
        public string SettingsJson
        {
            get => Settings == null ? null : JsonSerializer.Serialize(Settings);
            set => Settings = string.IsNullOrEmpty(value) ? new List<TableSetting>() : JsonSerializer.Deserialize<List<TableSetting>>(value);
        }
    }
}
