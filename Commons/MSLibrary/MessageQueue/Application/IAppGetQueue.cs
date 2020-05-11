using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MSLibrary.MessageQueue.Application
{
    /// <summary>
    /// 应用层获取队列
    /// </summary>
    public interface IAppGetQueue
    {
        /// <summary>
        /// 根据key获取活队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<SQueueData> Do(string key);
    }


    [DataContract]
    public class SQueueData
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        [DataMember]
        public string GroupName { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        [DataMember]
        public string ServerName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 分组Code
        /// </summary>
        [DataMember]
        public int Code { get; set; }
    }
}
