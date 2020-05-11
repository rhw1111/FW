using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Logger.Application
{
    /// <summary>
    /// 根据HttpContext创建通用日志根内容
    /// </summary>
    public interface IAppCreateCommonLogRootContentByHttpContext
    {
        Task<CommonLogRootContent> Do(HttpContext context);
    }
}
