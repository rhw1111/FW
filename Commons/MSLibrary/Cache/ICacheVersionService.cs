using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 缓存版本号服务
    /// 负责控制指定名称的版本号的更新、检查
    /// </summary>
    public interface ICacheVersionService
    {
        /// <summary>
        /// 执行版本号检查
        /// 当版本号发生变动时，将调用changedAction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="versionQueryAction"></param>
        /// <param name="changedAction"></param>
        /// <returns></returns>
        Task Execute(string name,Func<string,Task<string>> versionQueryAction,Func<Task> changedAction);
        /// <summary>
        /// 清除版本号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task Clear(string name);
    }
}
