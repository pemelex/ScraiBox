using ScraiBox.Core.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core.Interfaces
{
    public interface IUseCasePlugin
    {
        string Name { get; }
        string Description { get; }
        Task<ScraiBoxResult> ExecuteAsync(ScraiBoxContext context);
    }
}
