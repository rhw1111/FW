using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Logger.LoggingBuilderProviderHandlers
{
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForCommonLogLocalFactory), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForCommonLogLocalFactory : IFactory<ILoggingBuilderProviderHandler>
    {
        private LoggingBuilderProviderHandlerForCommonLogLocal _loggingBuilderProviderHandlerForCommonLogLocal;

        public LoggingBuilderProviderHandlerForCommonLogLocalFactory(LoggingBuilderProviderHandlerForCommonLogLocal loggingBuilderProviderHandlerForCommonLogLocal)
        {
            _loggingBuilderProviderHandlerForCommonLogLocal = loggingBuilderProviderHandlerForCommonLogLocal;
        }
        public ILoggingBuilderProviderHandler Create()
        {
            return _loggingBuilderProviderHandlerForCommonLogLocal;
        }
    }
}
