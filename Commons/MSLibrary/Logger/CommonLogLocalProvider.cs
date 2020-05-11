using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 基于本地通用日志的日志提供方
    /// </summary>
    [Injection(InterfaceType = typeof(CommonLogLocalProvider), Scope = InjectionScope.Singleton)]
    public class CommonLogLocalProvider : ILoggerProvider
    {
        private ICommonLogLocalProviderFactory _commonLogLocalProviderFactory;

        public CommonLogLocalProvider(ICommonLogLocalProviderFactory commonLogLocalProviderFactory)
        {
            _commonLogLocalProviderFactory = commonLogLocalProviderFactory; ;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _commonLogLocalProviderFactory.Create();
        }

        public void Dispose()
        {

        }
    }


    public interface ICommonLogLocalProviderFactory : IFactory<CommonLogLocalLogger>
    {

    }

    [Injection(InterfaceType = typeof(ICommonLogLocalProviderFactory), Scope = InjectionScope.Singleton)]
    public class CommonLogLocalProviderFactory : ICommonLogLocalProviderFactory
    {
        public CommonLogLocalLogger _commonLogLocalLogger;

        public CommonLogLocalProviderFactory(CommonLogLocalLogger commonLogLocalLogger)
        {
            _commonLogLocalLogger = commonLogLocalLogger;
        }
        public CommonLogLocalLogger Create()
        {
            return _commonLogLocalLogger;
        }
    }

}
