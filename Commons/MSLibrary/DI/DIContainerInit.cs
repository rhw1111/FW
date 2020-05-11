using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.DI
{
    /// <summary>
    /// 依赖注入容器初始化的静态统一入口点
    /// </summary>
    public static class DIContainerInit
    {
        private static IDIContainerInit _diContainerInit;
        public static IDIContainerInit Init
        {
            set
            {
                _diContainerInit = value;
            }
        }
        public static void Execute(params string[] assemblyNames)
        {
            _diContainerInit.Execute(assemblyNames);
        }
    }
}
