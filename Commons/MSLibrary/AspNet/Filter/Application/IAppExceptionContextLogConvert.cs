using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MSLibrary.AspNet.Filter.Application
{
    /// <summary>
    /// 应用层异常上下文日志对象转换
    /// </summary>
    public interface IAppExceptionContextLogConvert
    {
        Task<object> Do(ExceptionContext context);
    }
}
