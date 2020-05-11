using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    /// <summary>
    /// 用户登录请求DTO
    /// </summary>
    [DataContract]
    public class LocalLoginRequest:ModelBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username
        {
            get
            {

                return GetAttribute<string>(nameof(Username));
            }
            set
            {
                SetAttribute<string>(nameof(Username), value);
            }
        }


        /// <summary>
        /// 密码 
        /// </summary>

        [DataMember]
        public string Password
        {
            get
            {

                return GetAttribute<string>(nameof(Password));
            }
            set
            {
                SetAttribute<string>(nameof(Password), value);
            }
        }

        /// <summary>
        /// 是否保持登录
        /// </summary>
        [DataMember]
        public bool RememberLogin
        {
            get
            {

                return GetAttribute<bool>(nameof(RememberLogin));
            }
            set
            {
                SetAttribute<bool>(nameof(RememberLogin), value);
            }
        }

        /// <summary>
        /// 重定向地址
        /// </summary>
        [DataMember]
        public string ReturnUrl
        {
            get
            {

                return GetAttribute<string>(nameof(ReturnUrl));
            }
            set
            {
                SetAttribute<string>(nameof(ReturnUrl), value);
            }
        }
    }
}
