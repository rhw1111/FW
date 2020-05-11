using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.AspNet.Middleware.Application
{
    /// <summary>
    /// 应用层获取不需要记录日志的Api动作路径（格式为正则表达式）
    /// </summary>
    public interface IAppGetLogExcludePaths
    {
        Task<List<string>> Do();
    }
}
