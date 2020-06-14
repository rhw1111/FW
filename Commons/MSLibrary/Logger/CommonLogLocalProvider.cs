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

        private Dictionary<string, CommonLogLocalLogger> _localLoggers = new Dictionary<string, CommonLogLocalLogger>();

        public CommonLogLocalProvider(ICommonLogLocalProviderFactory commonLogLocalProviderFactory)
        {
            _commonLogLocalProviderFactory = commonLogLocalProviderFactory; ;
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (!_localLoggers.TryGetValue(categoryName,out CommonLogLocalLogger logger))
            {
                lock(_localLoggers)
                {
                    if (!_localLoggers.TryGetValue(categoryName, out logger))
                    {
                        logger = _commonLogLocalProviderFactory.Create();
                        logger.CategoryName = categoryName;
                        _localLoggers[categoryName] = logger;
                    }
                }
            }
            return logger;
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

        public CommonLogLocalLogger Create()
        {
            CommonLogLocalLogger logger;
            var di = ContextContainer.GetValue<IDIContainer>("DI");
            if (di == null)
            {
                logger = DIContainerContainer.Get<CommonLogLocalLogger>();
            }
            else
            {
                logger = di.Get<CommonLogLocalLogger>();
            }

            return logger;
        }
    }

}
