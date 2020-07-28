using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using MSLibrary;
using MSLibrary.AspNet.Filter;
using MSLibrary.AspNet.Middleware;
using MSLibrary.Oauth.ADFS;
using MSLibrary.Configuration;

namespace FW.TestPlatform.Main
{
    public static class ScheduledStartupHelper
    {
        /// <summary>
        /// 初始化配置容器
        /// 首先尝试从后缀名-{环境名}的文件加载配置
        /// 如果该文件不存在，则加载去除后缀名后的文件
        /// <paramref name="environmentName">当前环境名称</paramref>
        /// <paramref name="fileBaseUrl">配置文件基地址</paramref>
        /// </summary>
        public static void InitConfigurationContainer(string environmentName, string fileBaseUrl)
        {

        }



        /// <summary>
        /// 初始化上下文
        /// </summary>
        public static void InitContext()
        {

        }



        /// <summary>
        /// 初始化DI容器
        /// 自动装载被标识的对象
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="dISetting"></param>
        public static void InitDI(IServiceCollection serviceCollection, DISetting dISetting)
        {
        }

        /// <summary>
        /// 初始化静态化信息
        /// 所有通过静态属性来配置的信息，都在该方法中初始化
        /// </summary>
        public static void InitStaticInfo()
        {

        }





        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="builder"></param>
        public static void InitLogger(ILoggingBuilder builder)
        {

        }
    }
}
