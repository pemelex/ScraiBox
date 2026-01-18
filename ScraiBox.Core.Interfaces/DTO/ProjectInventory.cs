using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core.Interfaces.DTO
{
    public class ProjectInventory
    {
        public DateTime LastUpdated { get; set; }
        public List<FileEntry> Files { get; set; } = new();
    }
}
