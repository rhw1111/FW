using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.RemoteService.AuthInfoGeneratorServices
{
    /// <summary>
    /// 远程服务验证信息生成服务接口
    /// </summary>
    public interface IAuthInfoGeneratorService
    {
        /// <summary>
        /// 生成验证信息键值对
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, string>> Generate();
    }
}
