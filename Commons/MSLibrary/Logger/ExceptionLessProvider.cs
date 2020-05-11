using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 基于ExceptionLess的日志提供方
    /// </summary>
    [Injection(InterfaceType = typeof(ExceptionLessProvider), Scope = InjectionScope.Singleton)]
    public class ExceptionLessProvider : ILoggerProvider
    {

        public string Key { get; set; }
        public string ServiceUri { get; set; }

        private ExceptionLessLogger _exceptionLessLogger;

        public ExceptionLessProvider(ExceptionLessLogger exceptionLessLogger)
        {
            _exceptionLessLogger = exceptionLessLogger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            _exceptionLessLogger.Key = Key;
            _exceptionLessLogger.ServiceUri = ServiceUri;
            return _exceptionLessLogger;
        }

        public void Dispose()
        {
        }
    }
}
