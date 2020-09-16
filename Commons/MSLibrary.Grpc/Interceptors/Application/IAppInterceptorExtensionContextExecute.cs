using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSLibrary.Grpc.Interceptors.Application
{
    /// <summary>
    /// 应用层处理拦截器扩展上下文
    /// </summary>
    public interface IAppInterceptorExtensionContextExecute
    {
        Task<IInterceptorExtensionContextInit> Do(ServerCallContext callContext, string name);
    }

    public interface IInterceptorExtensionContextInit
    {
        void Execute();
    }
}
