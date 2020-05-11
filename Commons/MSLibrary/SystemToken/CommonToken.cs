using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 通用系统令牌
    /// </summary>
    [DataContract]
    public class CommonToken : ModelBase
    {
        /// <summary>
        /// 系统名称
        /// 对应系统登录终结点中的系统名称
        /// </summary>
        [DataMember]
        public string SystemName
        {
            get
            {
                return GetAttribute<string>("SystemName");
            }
            set
            {
                SetAttribute<string>("SystemName", value);
            }
        }

        /// <summary>
        /// 验证系统名称
        /// 对应验证终结点中的终结点名称
        /// </summary>
        [DataMember]
        public string AuthorizationName
        {
            get
            {
                return GetAttribute<string>("AuthorizationName");
            }
            set
            {
                SetAttribute<string>("AuthorizationName", value);
            }
        }

        /// <summary>
        /// 用户信息属性
        /// </summary>
        [DataMember]
        public Dictionary<string, string> UserInfoAttributes
        {
            get
            {
                return GetAttribute<Dictionary<string, string>>("UserInfoAttributes");
            }
            set
            {
                SetAttribute<Dictionary<string, string>>("UserInfoAttributes", value);
            }
        }


    }
}
