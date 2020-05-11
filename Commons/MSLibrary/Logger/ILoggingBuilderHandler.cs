using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 日志构建器处理接口
    /// 日志配置装配入口点
    /// </summary>
    public interface ILoggingBuilderHandler
    {
        Task Execute(ILoggingBuilder builder, LoggerConfiguration configuration);
    }
}
