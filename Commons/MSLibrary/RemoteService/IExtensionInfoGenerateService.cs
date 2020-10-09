using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.RemoteService
{
    /// <summary>
    /// 远程调用需要的附加信息生成服务
    /// </summary>
    public interface IExtensionInfoGenerateService
    {
        /// <summary>
        /// 生成命名调用的附加信息键值对
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IDictionary<string, string>> Generate(string name,object state);
        IDictionary<string, string> GenerateSync(string name, object state);
    }
}
