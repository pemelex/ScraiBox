using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core.Interfaces.DTO
{
    public enum FileType { CSharp, Razor, Json, Project, Other }

    public record FileEntry(
        string Name,
        string RelativePath,
        FileType Type,
        string ProjectName
    );
}
