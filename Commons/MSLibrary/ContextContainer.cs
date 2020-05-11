using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 上下文容器静态类
    /// 应用程序直接使用该静态类操作上下文数据
    /// </summary>
    public static class ContextContainer
    {
        /// <summary>
        /// 默认采用ContextContainerDefault
        /// </summary>
        private static IContextContainer _conteiner = new ContextContainerDefault();

        /// <summary>
        /// 上下文容器属性
        /// 可以用于替换默认容器
        /// </summary>
        public static IContextContainer Current
        {
            get
            {
                return _conteiner;
            }
            set
            {
                _conteiner = value;
            }
        }

        /// <summary>
        /// 为指定注册名称的上下文赋值
        /// </summary>
        /// <typeparam name="T">上下文值的类型</typeparam>
        /// <param name="name">注册名称</param>
        /// <param name="value">上下文值</param>
        public static void SetValue<T>(string name, T value)
        {
            _conteiner.SetValue(name, value);
        }

        /// <summary>
        /// 检查是否已经注册过指定名称的上下文
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsRegister(string name)
        {
            return _conteiner.IsRegister(name);
        }


        /// <summary>
        /// 获取指定名称的上下文值
        /// </summary>
        /// <typeparam name="T">上下文值的类型</typeparam>
        /// <param name="name">注册名称</param>
        /// <returns>上下文值</returns>
        public static T GetValue<T>(string name)
        {
            return _conteiner.GetValue<T>(name);
        }
        /// <summary>
        /// 指定注册名称的上下文是否是自动创建的
        /// </summary>
        /// <typeparam name="T">上下文值的类型</typeparam>
        /// <param name="name">注册名称</param>
        /// <returns>是否自动创建</returns>
        public static bool IsAuto<T>(string name)
        {
            return _conteiner.IsAuto<T>(name);
        }
    }


}
