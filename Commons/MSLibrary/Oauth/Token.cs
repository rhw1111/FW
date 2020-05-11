using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Oauth
{
    /// <summary>
    /// Oauth的令牌
    /// </summary>
    [DataContract]
    public class Token:ModelBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember]
        public string UserId
        {
            get
            {
                return GetAttribute<string>("UserId");
            }
            set
            {
                SetAttribute<string>("UserId", value);
            }
        }
        /// <summary>
        /// 过期时间（UTC）
        /// </summary>
        [DataMember]
        public DateTime Expire
        {
            get
            {
                return GetAttribute<DateTime>("Expire");
            }
            set
            {
                SetAttribute<DateTime>("Expire", value);
            }
        }

        /// <summary>
        /// 附加属性
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Extensions
        {
            get
            {
                return GetAttribute<Dictionary<string, string>>("Extensions");
            }
            set
            {
                SetAttribute<Dictionary<string, string>>("Extensions", value);
            }
        }
    }
}
