using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Context
{
    /// <summary>
    /// 国际化信息处理服务
    /// </summary>
    public interface IInternationalizationHandleService
    {
        /// <summary>
        /// 获取国际化信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetInternationalizationInfo(HttpRequest request);
        /// <summary>
        /// 生成国际化上下文
        /// </summary>
        /// <param name="internationalizationInfo"></param>
        void GenerateContext(object internationalizationInfo);
    }
}
