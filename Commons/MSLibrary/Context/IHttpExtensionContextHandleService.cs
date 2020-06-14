using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于Http请求的扩展上下文处理服务
    /// </summary>
    public interface IHttpExtensionContextHandleService
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetInfo(HttpRequest request);
        /// <summary>
        /// 生成上下文
        /// </summary>
        /// <param name="info"></param>
        void GenerateContext(object info);
    }
}
