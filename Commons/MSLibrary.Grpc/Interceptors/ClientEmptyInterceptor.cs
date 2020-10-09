using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using Grpc.Core.Interceptors;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Context.Application;
using MSLibrary.ExceptionHandle;
using MSLibrary.Serializer;

namespace MSLibrary.Grpc.Interceptors
{
    /// <summary>
    /// 客户端空拦截器
    /// 无任何操作，仅用来生成CallInvoker
    /// </summary>
    public class ClientEmptyInterceptor: Interceptor
    {
    }
}
