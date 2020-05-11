using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.OData.Client;

namespace MSLibrary.OData
{
    /// <summary>
    /// OData客户端工厂
    /// </summary>
    public interface IODataClientFactory
    {
        /// <summary>
        /// 创建指定连接配置的OData客户端
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<T> Create<T>(ODataClientConnectConfiguration configuration) where T : DataServiceContext;
    }

    /// <summary>
    /// OData客户端连接配置
    /// </summary>
    [DataContract]
    public class ODataClientConnectConfiguration
    {
        /// <summary>
        /// 初始化类型
        /// </summary>
        [DataMember]
        public string InitType { get; set; }
        /// <summary>
        /// 初始化使用的配置字符串
        /// </summary>
        [DataMember]
        public string InitConfigurationString { get; set; }
        /// <summary>
        /// 生成使用的配置字符串
        /// </summary>
        [DataMember]
        public string GenerateConfigurationString { get; set; }
    }
}
