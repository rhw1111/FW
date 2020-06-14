using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Context.Application
{
    /// <summary>
    /// 应用层处理基于Http请求的扩展上下文
    /// </summary>
    public interface IAppHttpExtensionContextExecute
    {
        Task<IHttpExtensionContextInit> Do(HttpRequest request, string name);
    }

 

    public interface IHttpExtensionContextInit
    {
        void Execute();
    }
}
