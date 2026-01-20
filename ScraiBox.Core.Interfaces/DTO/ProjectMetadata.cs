using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core.Interfaces.DTO
{
    public class ProjectMetadata
    {
        public string TargetFramework { get; set; } = "";
        public List<string> References { get; set; } = new List<string>();
        public bool IsLegacy { get; set; }
    }
}
