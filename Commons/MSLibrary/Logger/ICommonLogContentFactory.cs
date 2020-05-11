using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 通用日志内容工厂
    /// </summary>
    public interface ICommonLogContentFactory
    {
        Task<CommonLogContent> CreateFromHttpContext(HttpContext context);
        Task<CommonLogContent> Create(string actionName,string message);
    }
}
