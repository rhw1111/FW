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

namespace MSLibrary.Logger
{
    /// <summary>
    /// 基于通用日志本地记录的日志
    /// 供日志服务使用
    /// </summary>
    [Injection(InterfaceType = typeof(CommonLogLocalLogger), Scope = InjectionScope.Singleton)]
    public class CommonLogLocalLogger : ILogger
    {

        private ICommonLogLocalEnvInfoGeneratorService _commonLogLocalEnvInfoGeneratorService;

        public CommonLogLocalLogger(ICommonLogLocalEnvInfoGeneratorService commonLogLocalEnvInfoGeneratorService)
        {
            _commonLogLocalEnvInfoGeneratorService = commonLogLocalEnvInfoGeneratorService;
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
            var strUserInfo = _commonLogLocalEnvInfoGeneratorService.GenerateUserInfo();
            var strParentUserInfo = _commonLogLocalEnvInfoGeneratorService.GenerateParentUserInfo();


            if (strUserInfo == null)
            {
                strUserInfo = string.Empty;
            }

            if (strParentUserInfo == null)
            {
                strParentUserInfo = string.Empty;
            }

            if (state is string)
            {

                CommonLog log = null;
                try
                {
                    log = new CommonLog()
                    {
                        ID = Guid.NewGuid(),
                        Level = (int)logLevel,
                        ParentID = Guid.Empty,
                        ParentActionName = string.Empty,
                        PreLevelID = Guid.Empty,
                        CurrentLevelID = Guid.Empty,
                        ActionName = string.Empty,
                        RequestBody = string.Empty,
                        ResponseBody = string.Empty,
                        RequestUri = string.Empty,
                        ContextInfo = strUserInfo,
                        ParentContextInfo=strParentUserInfo,
                        Root = true,
                        Message = state as string
                    };
                }
                catch
                {

                }

                Task.Run(async () =>
                {
                    if (log != null)
                    {
                        await log.AddLocal();
                    }
                });


            }
            else if (state is CommonLogLocalContent)
            {

                var logContent = state as CommonLogLocalContent;

                CommonLog log = null;
                try
                {
                    log = new CommonLog()
                    {
                        ID = Guid.NewGuid(),
                        Level = (int)logLevel,
                        ParentID = Guid.Empty,
                        ParentActionName = string.Empty,
                        PreLevelID = Guid.Empty,
                        CurrentLevelID=Guid.Empty,
                        ActionName = logContent.ActionName,
                        RequestBody = logContent.RequestBody,
                        ResponseBody=logContent.ResponseBody,
                        RequestUri = logContent.RequestUri,
                        ContextInfo = strUserInfo,
                        ParentContextInfo=strParentUserInfo,
                        Root = true,
                        Message = logContent.Message
                    };
                }
                catch
                {

                }

                Task.Run(async () =>
                {
                    if (log != null)
                    {
                        await log.AddLocal();
                    }
                });


            }
            else
            {
                if (formatter != null)
                {

                    CommonLog log = null;
                    try
                    {

                        log = new CommonLog()
                        {
                            ID = Guid.NewGuid(),
                            Level = (int)logLevel,
                            ParentID = Guid.Empty,
                            ParentActionName = string.Empty,
                            PreLevelID = Guid.Empty,
                            CurrentLevelID=Guid.Empty,
                            ActionName = string.Empty,
                            RequestBody = string.Empty,
                            RequestUri = string.Empty,
                            ContextInfo = strUserInfo,
                            ParentContextInfo = strParentUserInfo,
                            Root = true,
                            Message = formatter(state, exception)
                        };

                    }
                    catch
                    {

                    }

                    Task.Run(async () =>
                    {
                        if (log != null)
                        {
                            await log.AddLocal();
                        }
                    });



                }
            }

        }
    }


    /// <summary>
    /// 本地通用日志环境信息生成服务
    /// </summary>
    public interface ICommonLogLocalEnvInfoGeneratorService
    {
        /// <summary>
        /// 生成用户信息
        /// </summary>
        /// <returns></returns>
        string GenerateUserInfo();
        /// <summary>
        /// 生成父用户信息
        /// </summary>
        /// <returns></returns>
        string GenerateParentUserInfo();
    }


    /// <summary>
    /// 本地通用日志内容
    /// </summary>
    [DataContract]
    public class CommonLogLocalContent
    {
        /// <summary>
        /// 动作名称
        /// </summary>
        [DataMember]
        public string ActionName { get; set; }

        /// <summary>
        /// 请求内容
        /// </summary>
        [DataMember]
        public string RequestBody
        {
            get; set;
        }

        /// <summary>
        /// 响应内容
        /// </summary>
        [DataMember]
        public string ResponseBody
        {
            get;set;
        }

        /// <summary>
        /// 请求路径
        /// </summary>
        [DataMember]
        public string RequestUri
        {
            get; set;
        }


        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Message
        {
            get; set;
        }

    }


}
