using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Logger;

namespace FW.TestPlatform.Main.Logger
{
    /// <summary>
    /// 日志内容
    /// 所有格式化日志记录均使用此对象
    /// </summary>
    [DataContract]
    public class LoggerContent : ICommonLogLocalContent, IExceptionLessLoggerContentExtension
    {


        /// <summary>
        /// 标签
        /// </summary>
        [DataMember]
        public List<string> Tags
        {
            get; set;
        } = new List<string>();



        public string[] GetTags()
        {
            return Tags.ToArray();
        }


        /// <summary>
        /// 动作名称
        /// </summary>
        [DataMember]
        public string ActionName
        {
            get; set;
        } = string.Empty;

        /// <summary>
        /// 请求内容
        /// </summary>
        [DataMember]
        public string RequestBody
        {
            get; set;
        } = string.Empty;

        /// <summary>
        /// 响应内容
        /// </summary>
        [DataMember]
        public string ResponseBody
        {
            get; set;
        } = string.Empty;

        /// <summary>
        /// 请求路径
        /// </summary>
        [DataMember]
        public string RequestUri
        {
            get; set;
        } = string.Empty;


        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Message
        {
            get;set;
        } = string.Empty;

        /// <summary>
        /// 持续时间
        /// </summary>
        [DataMember]
        public long Duration
        {
            get; set;
        }
    }
}
