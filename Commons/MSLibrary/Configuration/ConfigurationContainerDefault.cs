using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MSLibrary.Configuration
{
    /// <summary>
    /// 配置信息容器默认实现
    /// </summary>
    public class ConfigurationContainerDefault : IConfigurationContainer
    {
        private static ConcurrentDictionary<string, IConfiguration> _configurationList = new ConcurrentDictionary<string, IConfiguration>();
        private static List<string> _names = new List<string>();

        public void Add(string name, IConfiguration configuration)
        {
            if (!_names.Contains(name))
            {
                lock (_names)
                {
                    if (!_names.Contains(name))
                    {
                        _names.Add(name);
                    }
                }
            }
            _configurationList[name] = configuration;
        }

        public T Get<T>()
        {

            //取最后一个配置

            var configuration = GetLastConfiguration();
            if (configuration != null)
            {
                return configuration.Get<T>();
            }

            return default(T);
        }

        public T Get<T>(string name)
        {
            IConfiguration configuration;
            if (_configurationList.TryGetValue(name, out configuration))
            {
                return configuration.Get<T>();
            }

            return default(T);
        }

        public T GetBySection<T>(string sectionName)
        {
            //取最后一个配置
            var configuration =  GetLastConfiguration();
            if (configuration != null)
            {
                var sectinConfiguration = configuration.GetSection(sectionName);
                if (sectinConfiguration != null)
                {
                    return sectinConfiguration.Get<T>();
                }
            }

            return default(T);
        }

        public T GetBySection<T>(string name, string sectionName)
        {
            IConfiguration configuration;
            if (_configurationList.TryGetValue(name, out configuration))
            {
                var sectinConfiguration = configuration.GetSection(sectionName);
                if (sectinConfiguration != null)
                {
                    return sectinConfiguration.Get<T>();
                }
            }

            return default(T);
        }

        public IConfiguration GetConfiguration()
        {
            //取最后一个配置
            var configuration = GetLastConfiguration();
            if (configuration != null)
            {
                return configuration;
            }

            return null;
        }

        public IConfiguration GetConfiguration(string name)
        {
            IConfiguration configuration;
            if (_configurationList.TryGetValue(name, out configuration))
            {
                return configuration;
            }

            return null;
        }

        public IConfiguration GetSection(string sectionName)
        {
            //取最后一个配置
            var configuration = GetLastConfiguration();
            if (configuration != null)
            {
                return configuration.GetSection(sectionName);
            }

            return null;
        }

        public IConfiguration GetSection(string name, string sectionName)
        {
            IConfiguration configuration;
            if (_configurationList.TryGetValue(name, out configuration))
            {
                return configuration.GetSection(sectionName);
            }

            return null;
        }


        private IConfiguration GetLastConfiguration()
        {
            IConfiguration result = null;
            if (_names.Count > 0)
            {
                if (_configurationList.TryGetValue(_names[_names.Count - 1], out result))
                {

                }
            }

            return result;
        }
    }
}
