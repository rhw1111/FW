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
    [Injection(InterfaceType = typeof(CommonLogLocalLogger), Scope = InjectionScope.Transient)]
    public class CommonLogLocalLogger : ILogger
    {
        public string CategoryName { get; set; }

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
                         CategoryName=CategoryName,
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
            else if (state is ICommonLogLocalContent)
            {

                var logContent = state as ICommonLogLocalContent;

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
                        CategoryName = CategoryName,
                        ActionName = logContent.ActionName,
                        RequestBody = logContent.RequestBody,
                        ResponseBody=logContent.ResponseBody,
                        RequestUri = logContent.RequestUri,
                        ContextInfo = strUserInfo,
                        ParentContextInfo=strParentUserInfo,
                        Root = true,
                         Duration= logContent.Duration,
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
                            CategoryName =CategoryName,
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
    /// 本地通用日志内容接口
    /// </summary>
    public interface ICommonLogLocalContent
    {
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName 
        { 
            get;
        }

        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequestBody
        {
            get;
        }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseBody
        {
            get;
        }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestUri
        {
            get;
        }


        /// <summary>
        /// 内容
        /// </summary>
        public string Message
        {
            get;
        }

        /// <summary>
        /// 持续时间
        /// </summary>
        public long Duration
        {
            get; 
        }
    }
}
