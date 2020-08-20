using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 日志帮助器
    /// </summary>
    public static class LoggerHelper
    {
        public static string DefaultCategoryName
        {
            get;set;
        }

        /// <summary>
        /// 从DI容器获取指定目录名称的日志
        /// 提供handler回调处理
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="handler"></param>
        public static void GetLogger(string categoryName,Action<ILogger> handler)
        {
            using (var diContainer=DIContainerContainer.CreateContainer())
            {
                var logger= diContainer.Get<ILoggerFactory>().CreateLogger(categoryName);
                handler(logger);
            }                
        }

        /// <summary>
        /// 从DI容器获取默认目录名称的日志
        /// 提供handler回调处理
        /// 默认目录名称来自DefaultCategoryName属性
        /// </summary>
        /// <param name="handler"></param>
        public static void GetLogger(Action<ILogger> handler)
        {
            using (var diContainer = DIContainerContainer.CreateContainer())
            {
                var logger = diContainer.Get<ILoggerFactory>().CreateLogger(DefaultCategoryName);
                handler(logger);
            }
        }

        /// <summary>
        /// 获取指定目录的日志，如果目录为null,则使用默认目录
        /// 使用该日志写入错误信息
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="message"></param>
        public static void LogError(string categoryName, string message)
        {
            var logger = GetLogger(categoryName);

            logger.Log(LogLevel.Error, new EventId(), message, new Exception(message), (obj, ex) => { return message; });
            //logger.LogError(message);        
        }

        /// <summary>
        /// 获取指定目录的日志，如果目录为null,则使用默认目录
        /// 使用该日志写入通用信息
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="message"></param>
        public static void LogInformation(string categoryName, string message)
        {
            var logger = GetLogger(categoryName);
            logger.LogInformation(message);
        }

        /// <summary>
        /// 获取指定目录的日志，如果目录为null,则使用默认目录
        /// 使用该日志写入警告信息
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="message"></param>
        public static void LogWarning(string categoryName, string message)
        {
            var logger = GetLogger(categoryName);
            logger.LogWarning(message);
        }



        /// <summary>
        /// 获取指定目录的日志，如果目录为null,则使用默认目录
        /// 使用该日志写入错误信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggerFactory"></param>
        /// <param name="categoryName"></param>
        /// <param name="state">包含信息的对象</param>
        public static void LogError<T>(string categoryName, T state)
        {
            //尝试序列化state
            string strState = string.Empty;
            try
            {
                strState = JsonSerializerHelper.Serializer(state);
            }
            catch
            {

            }
            var logger = GetLogger(categoryName);
            
            logger.Log(LogLevel.Error,new EventId(),state,null,(obj,ex)=> { return strState; });

        }

        /// <summary>
        /// 获取指定目录的日志，如果目录为null,则使用默认目录
        /// 使用该日志写入通用信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggerFactory"></param>
        /// <param name="categoryName"></param>
        /// <param name="state">包含信息的对象</param>
        public static void LogInformation<T>(string categoryName, T state)
        {
            //尝试序列化state
            string strState = string.Empty;
            try
            {
                strState = JsonSerializerHelper.Serializer(state);
            }
            catch
            {

            }

            var logger = GetLogger(categoryName);
            logger.Log<T>(LogLevel.Information, new EventId(), state, null, (obj, ex) => { return strState; });
        }

        /// <summary>
        /// 获取指定目录的日志，如果目录为null,则使用默认目录
        /// 使用该日志写入警告信息
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="message"></param>
        public static void LogWarning<T>(string categoryName, T state)
        {
            var logger = GetLogger(categoryName);

            //尝试序列化state
            string strState = string.Empty;
            try
            {
               strState = JsonSerializerHelper.Serializer(state);
            }
            catch
            {

            }
            logger.Log<T>(LogLevel.Warning, new EventId(), state, null, (obj, ex) => { return strState; });
        }




        private static ILogger GetLogger(string categoryName)
        {
            ILogger logger = null;

            using (var diContainer = DIContainerContainer.CreateContainer())
            {
                var loggerFactory=diContainer.Get<ILoggerFactory>();
                if (categoryName == null)
                {
                    logger= loggerFactory.CreateLogger(DefaultCategoryName);
                }
                else
                {
                    logger= loggerFactory.CreateLogger(categoryName);
                }            
            }

            return logger;
        }
    }
}
