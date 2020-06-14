using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Entities
{
    public interface IScriptTemplateRepository
    {
        Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
