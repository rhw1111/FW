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
        Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
