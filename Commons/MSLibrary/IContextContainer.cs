using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 上下文容器接口
    /// 用于管理所有系统上下文数据
    /// </summary>
    public interface IContextContainer
    {
        /// <summary>
        /// 注册上下文
        /// </summary>
        /// <typeparam name="T">上下文中存储的数据类型</typeparam>
        /// <param name="name">注册名称</param>
        /// <param name="context">上下文</param>
        void Register<T>(string name, IContext<T> context);
        /// <summary>
        /// 检查指定名称的上下文是否已经注册
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsRegister(string name);

        /// <summary>
        /// 为指定注册名称的上下文赋值
        /// </summary>
        /// <typeparam name="T">上下文中存储的数据类型</typeparam>
        /// <param name="name">注册名称</param>
        /// <param name="value">上下文数据</param>
        void SetValue<T>(string name, T value);
        /// <summary>
        /// 获取指定注册名称的上下文数据
        /// </summary>
        /// <typeparam name="T">上下文中存储的数据类型<</typeparam>
        /// <param name="name">注册名称</param>
        /// <returns>上下文数据</returns>
        T GetValue<T>(string name);
        /// <summary>
        /// 检查指定注册名称的上下文数据是否是自动创建的
        /// </summary>
        /// <param name="name">注册名称</param>
        /// <returns>是否自动</returns>
        bool IsAuto<T>(string name);
    }
}
