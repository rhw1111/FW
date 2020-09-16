using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.DI
{
    /// <summary>
    /// 当前环境中的DI容器获取器
    /// 应用程序需要显式从DI容器中获取对象时，需使用该类来获取DI容器接口
    /// </summary>
    public static class DIContainerGetHelper
    {
        /// <summary>
        /// 获取当前环境中的DI容器接口
        /// </summary>
        /// <returns></returns>
        public static IDIContainer Get()
        {
            var di = ContextContainer.GetValue<IDIContainer>(ContextTypes.DI);
            if (di == null)
            {
                di = DIContainerContainer.DIContainer;
            }

            return di;
        }

    }
}
