using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Logger.LoggingBuilderProviderHandlers
{
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForExceptionLessFactory), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForExceptionLessFactory : IFactory<ILoggingBuilderProviderHandler>
    {
        private LoggingBuilderProviderHandlerForExceptionLess _loggingBuilderProviderHandlerForExceptionLess;

        public LoggingBuilderProviderHandlerForExceptionLessFactory(LoggingBuilderProviderHandlerForExceptionLess loggingBuilderProviderHandlerForExceptionLess)
        {
            _loggingBuilderProviderHandlerForExceptionLess = loggingBuilderProviderHandlerForExceptionLess;
        }
        public ILoggingBuilderProviderHandler Create()
        {
            return _loggingBuilderProviderHandlerForExceptionLess;
        }
    }
}
