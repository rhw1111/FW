using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 基于通用日志的日志提供方
    /// </summary>
    [Injection(InterfaceType = typeof(CommonLogProvider), Scope = InjectionScope.Singleton)]
    public class CommonLogProvider : ILoggerProvider
    {
        private ICommonLogLoggerFactory _commonLogLoggerFactory;

        public CommonLogProvider(ICommonLogLoggerFactory commonLogLoggerFactory)
        {
            _commonLogLoggerFactory = commonLogLoggerFactory;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _commonLogLoggerFactory.Create();
        }

        public void Dispose()
        {
        }
    }

    public interface ICommonLogLoggerFactory:IFactory<CommonLogLogger>
    {

    }

    [Injection(InterfaceType = typeof(ICommonLogLoggerFactory), Scope = InjectionScope.Singleton)]
    public class CommonLogLoggerFactory : ICommonLogLoggerFactory
    {
        public CommonLogLogger _commonLogLogger;

        public CommonLogLoggerFactory(CommonLogLogger commonLogLogger)
        {
            _commonLogLogger = commonLogLogger;
        }
        public CommonLogLogger Create()
        {
            return _commonLogLogger;
        }
    }
}
