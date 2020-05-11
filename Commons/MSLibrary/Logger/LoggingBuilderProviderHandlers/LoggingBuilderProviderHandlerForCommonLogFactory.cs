using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Logger.LoggingBuilderProviderHandlers
{
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForCommonLogFactory), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForCommonLogFactory : IFactory<ILoggingBuilderProviderHandler>
    {
        private LoggingBuilderProviderHandlerForCommonLog _loggingBuilderProviderHandlerForCommonLog;

        public LoggingBuilderProviderHandlerForCommonLogFactory(LoggingBuilderProviderHandlerForCommonLog loggingBuilderProviderHandlerForCommonLog)
        {
            _loggingBuilderProviderHandlerForCommonLog=loggingBuilderProviderHandlerForCommonLog;
        }
        public ILoggingBuilderProviderHandler Create()
        {
            return _loggingBuilderProviderHandlerForCommonLog;
        }
    }
}
