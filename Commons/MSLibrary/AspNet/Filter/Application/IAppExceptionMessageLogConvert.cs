using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MSLibrary.AspNet.Filter.Application
{
    /// <summary>
    /// 应用层异常消息对象转换
    /// </summary>
    public interface IAppExceptionMessageLogConvert
    {
        Task<object> Do(ActionContext context, string exceptionMessage);
    }
}
