using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace MSLibrary.AspNet.Middleware.Application
{
    /// <summary>
    /// 应用层异常转换日志
    /// </summary>
    public interface IAppExceptionHttpContextLogConvert
    {
        Task<object> Convert(HttpContext context);
    }
}
