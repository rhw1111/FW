using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary
{
    /// <summary>
    /// 检查结果
    /// 适用于返回结果是正确或错误的操作
    /// </summary>
    [DataContract]
    public class ValidateResult
    {
        /// <summary>
        /// 是否正确
        /// </summary>
        [DataMember]
        public bool Result { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }


    /// <summary>
    /// 检查结果,如果正确，返回数据
    /// 适用于返回结果是正确或错误的操作，同时正确时需要返回数据
    /// </summary>
    [DataContract]
    public class ValidateResult<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public T Data { get; set; }
        /// <summary>
        /// 是否正确
        /// </summary>
        [DataMember]
        public bool Result { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }

}
