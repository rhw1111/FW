using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Context.Application
{
    /// <summary>
    /// 应用层国际化处理
    /// </summary>
    public interface IAppInternationalizationExecute
    {
        Task<IInternationalizationContextInit> Do(HttpRequest request,string name);
    }

    public interface IInternationalizationContextInit
    {
        void Execute();
    }
}
