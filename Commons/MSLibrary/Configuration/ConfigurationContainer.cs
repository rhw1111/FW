using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MSLibrary.Configuration
{
    /// <summary>
    /// 配置信息容器的静态统一入口点
    /// </summary>
    public static class ConfigurationContainer
    {
        private static IConfigurationContainer _configurationContainer;

        public static IConfigurationContainer Container
        {
            set
            {
                _configurationContainer = value;
                
                
            }
        }

        private static Dictionary<string, IConfigurationRoot> _listenerJsons = new Dictionary<string, IConfigurationRoot>();
        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="configuration">配置</param>
        public static void Add(string name, IConfiguration configuration)
        {
            _configurationContainer.Add(name, configuration);
        }
        /// <summary>
        /// 获取默认配置的指定类型的配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Get<T>()
        {
            return _configurationContainer.Get<T>();
        }
        /// <summary>
        /// 获取指定类型指定名称的配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        public static T Get<T>(string name)
        {
            return _configurationContainer.Get<T>(name);
        }
        /// <summary>
        /// 获取默认配置的指定节的配置
        /// </summary>
        /// <returns></returns>
        public static IConfiguration GetSection(string sectionName)
        {
            return _configurationContainer.GetSection(sectionName);
        }
        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <returns></returns>
        public static IConfiguration GetConfiguration()
        {
            return _configurationContainer.GetConfiguration();
        }
        /// <summary>
        /// 获取指定名称的配置的指定节的配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static IConfiguration GetSection(string name, string sectionName)
        {
            return _configurationContainer.GetSection(name, sectionName);
        }
        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IConfiguration GetConfiguration(string name)
        {
            return _configurationContainer.GetConfiguration(name);
        }
        /// <summary>
        /// 获取默认配置的指定节的配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static T GetBySection<T>(string sectionName)
        {
            return _configurationContainer.GetBySection<T>(sectionName);
        }

        /// <summary>
        /// 获取指定名称的配置的指定节的配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static T GetBySection<T>(string name, string sectionName)
        {
            return _configurationContainer.GetBySection<T>(name,sectionName);
        }

        /// <summary>
        /// 注册json文件的监听器，当注册时或文件有改动时，将调用监听器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonFile">基于当前应用程序根目录的json文件</param>
        /// <param name="listener">监听器</param>
        public static void RegisterJsonListener<T>(string jsonFile,Action<T> listener)
        {
            IConfigurationRoot configuration = null;
            if (!_listenerJsons.ContainsKey(jsonFile))
            {
                lock (_listenerJsons)
                {
                    if (!_listenerJsons.ContainsKey(jsonFile))
                    {
                        configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile(jsonFile, optional: false, reloadOnChange: true)
                                    .Build();
                    }
                }
            }

            if (configuration!=null)
            {
                IDisposable registerChangeDisposable = null;
                Action<object> changeCallback = null;

                changeCallback = (configObj) =>
                {
                    var config = (IConfiguration)(configObj);
                    if (registerChangeDisposable != null)
                    {
                        registerChangeDisposable.Dispose();
                    }
                    var configValue = config.Get<T>();

                    registerChangeDisposable = config.GetReloadToken().RegisterChangeCallback(changeCallback, config);

                    listener(configValue);

                };

                changeCallback(configuration);
            }
        }
    }
}
