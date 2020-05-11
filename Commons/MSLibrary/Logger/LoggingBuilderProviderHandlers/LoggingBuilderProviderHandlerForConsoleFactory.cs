using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Logger.LoggingBuilderProviderHandlers
{
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForConsoleFactory), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForConsoleFactory : IFactory<ILoggingBuilderProviderHandler>
    {
        private LoggingBuilderProviderHandlerForConsole _loggingBuilderProviderHandlerForConsole;

        public LoggingBuilderProviderHandlerForConsoleFactory(LoggingBuilderProviderHandlerForConsole loggingBuilderProviderHandlerForConsole)
        {
            _loggingBuilderProviderHandlerForConsole = loggingBuilderProviderHandlerForConsole;
        }

        public ILoggingBuilderProviderHandler Create()
        {
            return _loggingBuilderProviderHandlerForConsole;
        }
    }
}
