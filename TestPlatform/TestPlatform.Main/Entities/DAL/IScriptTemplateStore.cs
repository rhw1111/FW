using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;

namespace FW.TestPlatform.Main.Entities.DAL
{
    /// <summary>
    /// 脚本模板数据操作
    /// </summary>
    public interface IScriptTemplateStore
    {
        Task Add(ScriptTemplate template, CancellationToken cancellationToken = default);
        Task Update(ScriptTemplate template, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<ScriptTemplate?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
