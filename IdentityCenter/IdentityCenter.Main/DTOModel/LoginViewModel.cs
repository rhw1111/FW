using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    /// <summary>
    /// 登录视图DTO
    /// </summary>
    [DataContract]
    public class LoginViewModel:ModelBase
    {
        /// <summary>
        /// 是否允许记住登录
        /// </summary>
        [DataMember]
        public bool AllowRememberLogin
        {
            get
            {

                return GetAttribute<bool>(nameof(AllowRememberLogin));
            }
            set
            {
                SetAttribute<bool>(nameof(AllowRememberLogin), value);
            }
        }
        /// <summary>
        /// 是否允许本地登录
        /// </summary>
        [DataMember]
        public bool EnableLocalLogin
        {
            get
            {

                return GetAttribute<bool>(nameof(EnableLocalLogin));
            }
            set
            {
                SetAttribute<bool>(nameof(EnableLocalLogin), value);
            }
        }

        /// <summary>
        /// 带入的用户名称
        /// </summary>
        [DataMember]
        public string HintUserName
        {
            get
            {

                return GetAttribute<string>(nameof(HintUserName));
            }
            set
            {
                SetAttribute<string>(nameof(HintUserName), value);
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
        /// <summary>
        /// 认证提供方列表
        /// </summary>
        [DataMember]
        public List<IdentityProviderModel> IdentityProviders
        {
            get
            {

                return GetAttribute<List<IdentityProviderModel>>(nameof(IdentityProviders));
            }
            set
            {
                SetAttribute<List<IdentityProviderModel>>(nameof(IdentityProviders), value);
            }
        }
    }
}
