using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Entities
{
    public interface IScriptTemplateRepository
    {
        Task Add(ScriptTemplate template, CancellationToken cancellationToken = default);
        Task Update(ScriptTemplate template, CancellationToken cancellationToken = default);
        Task Delete(ScriptTemplate template, CancellationToken cancellationToken = default);
        Task<ScriptTemplate?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
