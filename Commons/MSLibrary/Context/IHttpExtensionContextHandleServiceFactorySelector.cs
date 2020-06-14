using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于Http请求的扩展上下文处理服务工厂选择器接口
    /// </summary>
    public interface IHttpExtensionContextHandleServiceFactorySelector:ISelector<IFactory<IHttpExtensionContextHandleService>>
    {
    }
}
