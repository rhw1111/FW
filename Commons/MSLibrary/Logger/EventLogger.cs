using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System.Diagnostics;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 实现Windows事件日志
    /// </summary>
    public class EventLogger : ILogger
    {
        /// <summary>
        /// 状态对象转换工厂
        /// 在系统初始化的时候需要赋值
        /// </summary>
        public static IFactory<IEventLoggerStateConvert> ConvertFactory
        {
            get;set;
        }

        private string _loggerName;
        private LoggerExternalScopeProvider _loggerExternalScopeProvider;

        public EventLogger(string loggerName)
        {
            _loggerExternalScopeProvider = new LoggerExternalScopeProvider();
            _loggerName = loggerName;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return (new LoggerExternalScopeProvider()).Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string strMessage=string.Empty;
            string strContent = null;


                if (exception != null)
                {
                    strContent = $@"Message:{exception.Message},StackTrace:{exception.StackTrace}";
                }
                else if(state!=null)
                {
                    strContent = ConvertFactory.Create().Convert(state);
                }
     
                //if (ConsoleLogScope.Current != null)
                //{
                //    strMessage = $@"{_loggerName}[{eventId.Id}]
                //                    =>{ConsoleLogScope.Current.ToString()}
                //                    {strContent}              
                //               ";
                //}
                //else
                //{
                    strMessage = $@"{_loggerName}[{eventId.Id}]
                                    {strContent}              
                               ";
                //}
            

            
            EventLog myLog = new EventLog()
            {
                Source=_loggerName
            };

            switch (logLevel)
            {
                case LogLevel.Critical:

                    myLog.WriteEntry($"{logLevel.ToString()}:{strMessage}",EventLogEntryType.Error);
                    
                    break;
                case LogLevel.Debug:
                    myLog.WriteEntry($"{logLevel.ToString()}:{strMessage}", EventLogEntryType.Warning);
                    break;
                case LogLevel.Error:
                    myLog.WriteEntry($"{logLevel.ToString()}:{strMessage}", EventLogEntryType.Error);
                    break;
                case LogLevel.Information:
                    myLog.WriteEntry($"{logLevel.ToString()}:{strMessage}", EventLogEntryType.Information);

                    break;
                case LogLevel.Trace:
                    myLog.WriteEntry($"{logLevel.ToString()}:{strMessage}", EventLogEntryType.Information);
                    break;
                case LogLevel.Warning:
                    myLog.WriteEntry($"{logLevel.ToString()}:{strMessage}", EventLogEntryType.Warning);
                    break;
                default:
                    break;
            }


        }
    }

    /// <summary>
    /// 事件日志状态对象转换接口
    /// 负责将状态对象转换成文本内容
    /// </summary>
    public interface IEventLoggerStateConvert
    {
        string Convert(object state);
    }
}
