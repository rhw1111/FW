using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using MSLibrary.Configuration.SourceExtensions;

namespace MSLibrary.Configuration
{
    /// <summary>
    /// 配置构造扩展方法
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddJsonContent(this IConfigurationBuilder builder,string content)
        {
            return builder.Add(new JsonContentConfigurationSource(content));
        }
    }
}
