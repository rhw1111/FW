using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MSLibrary;
using MSLibrary.Transaction;
using MSLibrary.DI;
using MSLibrary.Serializer;
using Exceptionless;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 基于ExceptionLess的日志
    /// </summary>
    [Injection(InterfaceType = typeof(ExceptionLessLogger), Scope = InjectionScope.Singleton)]
    public class ExceptionLessLogger : ILogger
    {
        private static object _lockObj = new object();
        private static bool _init = false;

        public string Key { get; set; }
        public string ServiceUri { get; set; }
        public IDisposable BeginScope<TState>(TState state)
        {
            return (new LoggerExternalScopeProvider()).Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        private void init()
        {
            if (!_init)
            {
                lock (_lockObj)
                {
                    if (!_init)
                    {
                        ExceptionlessClient.Default.Configuration.ApiKey = Key;
                        if (!string.IsNullOrEmpty(ServiceUri))
                        {
                            ExceptionlessClient.Default.Configuration.ServerUrl = ServiceUri;
                        }
                        _init = true;
                    }
                }
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            init();
            var level = convertLevel(logLevel);
            if (state is string)
            {

                ExceptionlessClient.Default.SubmitLog("", state as string, level);
            }
            else if (state is IExceptionLessLoggerContentExtension)
            {
                var content = state as IExceptionLessLoggerContentExtension;
                var source = content.GetSource();
                var tags = content.GetTags();

                //尝试序列化state
                string strState = string.Empty;
                try
                {
                    strState = JsonSerializerHelper.Serializer(state);
                }
                catch
                {

                }

                ExceptionlessClient.Default.CreateLog(source, strState, level).AddTags(tags).Submit();
            }
            else
            {
                if (formatter != null)
                {
                    ExceptionlessClient.Default.SubmitLog("", formatter(state, exception), level);
                }
            }
        }


        private Exceptionless.Logging.LogLevel convertLevel(LogLevel level)
        {
            Exceptionless.Logging.LogLevel result = Exceptionless.Logging.LogLevel.Other;
            switch (level)
            {
                case LogLevel.Information:
                    result = Exceptionless.Logging.LogLevel.Info;
                    break;
                case LogLevel.Error:
                    result = Exceptionless.Logging.LogLevel.Error;
                    break;
                case LogLevel.Warning:
                    result = Exceptionless.Logging.LogLevel.Warn;
                    break;
                case LogLevel.Critical:
                    result = Exceptionless.Logging.LogLevel.Fatal;
                    break;
                case LogLevel.Debug:
                    result = Exceptionless.Logging.LogLevel.Debug;
                    break;
                case LogLevel.Trace:
                    result = Exceptionless.Logging.LogLevel.Trace;
                    break;
                default:
                    break;
            }

            return result;
        }
    }

    /// <summary>
    /// ExceptionLessLogger的内容扩展
    /// 用于记录日志的对象可以实现该接口，用来增加ExceptionLess的记录信息
    /// </summary>
    public interface IExceptionLessLoggerContentExtension
    {
        /// <summary>
        /// 获取源信息
        /// </summary>
        /// <returns></returns>
        string GetSource();
        /// <summary>
        /// 获取标签集
        /// </summary>
        /// <returns></returns>
        string[] GetTags();
    }
}
