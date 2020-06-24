using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities.DAL;

namespace FW.TestPlatform.Main.Entities
{
    [Injection(InterfaceType = typeof(IScriptTemplateRepository), Scope = InjectionScope.Singleton)]
    public class ScriptTemplateRepository : IScriptTemplateRepository
    {
        private readonly IScriptTemplateStore _scriptTemplateStore;

        public ScriptTemplateRepository(IScriptTemplateStore scriptTemplateStore)
        {
            _scriptTemplateStore = scriptTemplateStore;
        }

        public async Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _scriptTemplateStore.QueryByName(name, cancellationToken);
        }
    }
}
