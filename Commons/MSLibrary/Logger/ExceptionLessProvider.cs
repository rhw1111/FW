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


        private Dictionary<string, ExceptionLessLogger> _loggers=new Dictionary<string, ExceptionLessLogger>();

       

        public ExceptionLessProvider()
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (!_loggers.TryGetValue(categoryName,out ExceptionLessLogger logger))
            {
                lock(_loggers)
                {
                    if (!_loggers.TryGetValue(categoryName, out logger))
                    {
                        logger= new ExceptionLessLogger()
                        {
                            CategoryName = categoryName,
                            Key = Key,
                            ServiceUri = ServiceUri
                        };
                        _loggers[categoryName] = logger;
                    }
                }
            }

            return logger;
        }

        public void Dispose()
        {
        }
    }
}
