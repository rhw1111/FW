using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.AspNet.Middleware.Application
{
    /// <summary>
    /// 应用层http上下文日志对象转换
    /// </summary>
    public interface IAppHttpContextLogConvert
    {
        Task<object> Convert(HttpContextData context);
    }
}
