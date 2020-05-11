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
    /// 基于通用日志记录的日志
    /// 供所有日志服务以外的服务使用
    /// </summary>
    [Injection(InterfaceType = typeof(CommonLogLogger), Scope = InjectionScope.Singleton)]
    public class CommonLogLogger : ILogger
    {
        private ICommonLogEnvInfoGeneratorService _commonLogEnvInfoGeneratorService;
        private ICommonLogExecute _commonLogExecute;

        public CommonLogLogger(ICommonLogEnvInfoGeneratorService commonLogEnvInfoGeneratorService, ICommonLogExecute commonLogExecute)
        {
            _commonLogEnvInfoGeneratorService = commonLogEnvInfoGeneratorService;
            _commonLogExecute = commonLogExecute;
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
            var strUserInfo = _commonLogEnvInfoGeneratorService.GenerateUserInfo();
            var strParentUserInfo = _commonLogEnvInfoGeneratorService.GetParentUserInfo();

            var pID = _commonLogEnvInfoGeneratorService.GetParentID();
            var pActionName = _commonLogEnvInfoGeneratorService.GetParentActionName();
            var preID = _commonLogEnvInfoGeneratorService.GetPreLevelID();
            var currentID = _commonLogEnvInfoGeneratorService.GetCurrentLevelID();

            Guid id = Guid.NewGuid();
            Guid parentID=Guid.Empty;
            string strPActionName = string.Empty;
            bool root = true;
            if (pID!=null)
            {
                root = false;
                parentID = pID.Value;
                if (pActionName != null)
                {
                    strPActionName = pActionName;
                }
            }

            Guid preLevelID = Guid.Empty;
            if (preID!=null)
            {
                preLevelID = preID.Value;
            }

            Guid currentLevelID = Guid.Empty;
            if (currentID != null)
            {
                currentLevelID = currentID.Value;
            }





            if (strUserInfo==null)
            {
                strUserInfo = string.Empty;
            }

            if (strParentUserInfo==null)
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
                        ID = id,
                        Level = (int)logLevel,
                        ParentID = parentID,
                        ParentActionName = strPActionName,
                        PreLevelID = preLevelID,
                        CurrentLevelID=currentLevelID,
                        ActionName = string.Empty,
                        RequestBody = string.Empty,
                        ResponseBody=string.Empty,
                        RequestUri = string.Empty,
                        ContextInfo = strUserInfo,
                        ParentContextInfo=strParentUserInfo,
                        Root = root,
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
                            await _commonLogExecute.Execute(log);
                        }
                    });
                

            }
            else if (state is CommonLogContent)
            {
                
                var logContent = state as CommonLogContent;

                if (logContent.ParentID!=null)
                {
                    parentID = logContent.ParentID.Value;
                }

                if (logContent.ParentActionName!=null)
                {
                    strPActionName = logContent.ParentActionName;
                }

   

                    CommonLog log = null;
                    try
                    {
                        log = new CommonLog()
                        {
                            ID = id,
                            Level = (int)logLevel,
                            ParentID = parentID,
                            ParentActionName = strPActionName,
                            PreLevelID = preLevelID,
                            CurrentLevelID=currentLevelID,
                            ActionName = logContent.ActionName,
                            RequestBody = logContent.RequestBody,
                            ResponseBody=logContent.ResponseBody,
                            RequestUri = logContent.RequestUri,
                            ContextInfo = strUserInfo,
                            ParentContextInfo=strParentUserInfo,              
                            Root = root,
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
                            await _commonLogExecute.Execute(log);
                        }
                    });
               
                
            }
            else if (state is CommonLogRootContent)
            {

                var logContent = state as CommonLogContent;

                if (logContent.ParentID != null)
                {
                    parentID = logContent.ParentID.Value;
                }

                if (logContent.ParentActionName != null)
                {
                    strPActionName = logContent.ParentActionName;
                }



                CommonLog log = null;
                try
                {
                    log = new CommonLog()
                    {
                        ID = id,
                        Level = (int)logLevel,
                        ParentID = parentID,
                        ParentActionName = strPActionName,
                        PreLevelID = preLevelID,
                        CurrentLevelID = currentLevelID,
                        ActionName = logContent.ActionName,
                        RequestBody = logContent.RequestBody,
                        ResponseBody = logContent.ResponseBody,
                        RequestUri = logContent.RequestUri,
                        ContextInfo = strUserInfo,
                        ParentContextInfo = strParentUserInfo,
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
                        await _commonLogExecute.Execute(log);
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
                            ID = id,
                            Level = (int)logLevel,
                            ParentID = parentID,
                            ParentActionName = strPActionName,
                            PreLevelID=preLevelID,
                            CurrentLevelID=currentLevelID,
                            ActionName = string.Empty,
                            RequestBody = string.Empty,
                            RequestUri = string.Empty,
                            ContextInfo = strUserInfo,
                            ParentContextInfo=strParentUserInfo,
                            Root = root,
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
                                await _commonLogExecute.Execute(log);
                            }
                        });
                    
 

                }
            }

        }
    }

    /// <summary>
    /// 通用日志环境信息生成服务
    /// </summary>
    public interface ICommonLogEnvInfoGeneratorService
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
        string GetParentUserInfo();
        /// <summary>
        /// 获取父日志ID
        /// </summary>
        /// <returns></returns>
        Guid? GetParentID();
        /// <summary>
        /// 获取上一层级ID
        /// </summary>
        /// <returns></returns>
        Guid? GetPreLevelID();
        /// <summary>
        /// 获取当前层级ID
        /// </summary>
        /// <returns></returns>
        Guid? GetCurrentLevelID();
        /// <summary>
        /// 获取父日志动作名称
        /// </summary>
        /// <returns></returns>
        string GetParentActionName();
    }

    /// <summary>
    /// 通用日志处理服务
    /// </summary>
    public interface ICommonLogExecute
    {
        Task Execute(CommonLog log);
    }

    [DataContract]
    public class CommonLogContent
    {
        /// <summary>
        /// 动作名称
        /// </summary>
        [DataMember]
        public string ActionName { get; set; }
        /// <summary>
        /// 父日志Id
        /// </summary>
        [DataMember]
        public Guid? ParentID { get; set; }
        /// <summary>
        /// 父日志动作名称
        /// </summary>
        [DataMember]
        public string ParentActionName { get; set; }

        /// <summary>
        /// 请求内容
        /// </summary>
        [DataMember]
        public string RequestBody
        {
            get;set;
        }

        /// <summary>
        /// 响应内容
        /// </summary>
        [DataMember]
        public string ResponseBody
        {
            get; set;
        }


        /// <summary>
        /// 请求路径
        /// </summary>
        [DataMember]
        public string RequestUri
        {
            get;set;
        }


        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Message
        {
            get;set;
        }

    }

    [DataContract]
    public class CommonLogRootContent: CommonLogContent
    {

    }
}
