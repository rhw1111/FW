using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MSLibrary.Configuration
{
    /// <summary>
    /// 配置信息容器接口
    /// </summary>
    public interface IConfigurationContainer
    {
        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="configuration">配置</param>
        void Add(string name, IConfiguration configuration);
        /// <summary>
        /// 获取默认配置的指定类型的配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T Get<T>();
        /// <summary>
        /// 获取指定类型指定名称的配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        T Get<T>(string name);

        /// <summary>
        /// 获取默认配置的指定节的配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        T GetBySection<T>(string sectionName);

        /// <summary>
        /// 获取指定名称的配置的指定节的配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        T GetBySection<T>(string name, string sectionName);
        /// <summary>
        /// 获取默认配置的指定节的配置
        /// </summary>
        /// <returns></returns>
        IConfiguration GetSection(string sectionName);
        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <returns></returns>
        IConfiguration GetConfiguration();
        /// <summary>
        /// 获取指定名称的配置的指定节的配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        IConfiguration GetSection(string name, string sectionName);
        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IConfiguration GetConfiguration(string name);
    }
}
