using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Logger.LoggingBuilderProviderHandlers
{
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForApplicationInsightsFactory), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForApplicationInsightsFactory : IFactory<ILoggingBuilderProviderHandler>
    {
        private LoggingBuilderProviderHandlerForApplicationInsights _loggingBuilderProviderHandlerForApplicationInsights;
        
        public LoggingBuilderProviderHandlerForApplicationInsightsFactory(LoggingBuilderProviderHandlerForApplicationInsights loggingBuilderProviderHandlerForApplicationInsights)
        {
            _loggingBuilderProviderHandlerForApplicationInsights = loggingBuilderProviderHandlerForApplicationInsights;
        }
        public ILoggingBuilderProviderHandler Create()
        {
            return _loggingBuilderProviderHandlerForApplicationInsights;
        }
    }
}
