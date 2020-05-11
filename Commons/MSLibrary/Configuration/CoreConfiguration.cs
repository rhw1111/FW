using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Configuration
{
    /// <summary>
    /// 核心配置信息
    /// </summary>
    [DataContract]
    public class CoreConfiguration
    {
        /// <summary>
        /// 连接字符串列表
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Connections { get; set; }
        /// <summary>
        /// DI容器设置
        /// </summary>
        [DataMember]
        public DISetting DISetting { get; set; }
        /// <summary>
        /// 是否是调试
        /// </summary>
        [DataMember]
        public bool Debug { get; set; }
        [DataMember]
        public ThreadPoolSetting ThreadPoolSetting { get; set; }
    }
    /// <summary>
    /// DI容器设置
    /// </summary>
    [DataContract]
    public class DISetting
    {
        /// <summary>
        /// 自动加载要搜索的程序集名称集合
        /// </summary>
        [DataMember]
        public string[] SearchAssemblyNames { get; set; }
    }

    /// <summary>
    /// 线程池设置
    /// </summary>
    [DataContract]
    public class ThreadPoolSetting
    {
        /// <summary>
        /// 最大工作线程(实际数值为该数*核数)
        /// </summary>
        [DataMember]
        public int MaxWorkThread { get; set; }
        /// <summary>
        /// 最小工作线程(实际数值为该数*核数)
        /// </summary>
        [DataMember]
        public int MinWorkThread { get; set; }
        /// <summary>
        /// 最大IO线程(实际数值为该数*核数)
        /// </summary>
        [DataMember]
        public int MaxIOThread { get; set; }
        /// <summary>
        /// 最小IO线程(实际数值为该数*核数)
        /// </summary>
        [DataMember]
        public int MinIOThread { get; set; }
    }
 
}
