using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace MSLibrary.AspNet
{
    /// <summary>
    /// WebHostBuilderExtensions的扩展方法类
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseAction(this IWebHostBuilder host,Action action)
        {
            action();
            return host;
        }
    }
}
