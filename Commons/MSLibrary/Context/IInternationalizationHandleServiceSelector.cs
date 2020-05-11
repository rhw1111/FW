using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Context
{
    /// <summary>
    /// 国际化处理服务工厂选择器
    /// </summary>
    public interface IInternationalizationHandleServiceFactorySelector : ISelector<IFactory<IInternationalizationHandleService>>
    {
    }
}
