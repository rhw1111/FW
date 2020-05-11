using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    /// <summary>
    /// 第三方登录回调处理结果
    /// </summary>
    [DataContract]
    public class ExternalLoginCallbackResult:ModelBase
    {
        /// <summary>
        /// 第三方认证绑定页面
        /// </summary>
        [DataMember]
        public string ExternalIdentityBindPage
        {
            get
            {

                return GetAttribute<string>(nameof(ExternalIdentityBindPage));
            }
            set
            {
                SetAttribute<string>(nameof(ExternalIdentityBindPage), value);
            }
        }

        /// <summary>
        /// 是否已经存在用户账号
        /// </summary>
        [DataMember]
        public bool ExistsUserAccount
        {
            get
            {

                return GetAttribute<bool>(nameof(ExistsUserAccount));
            }
            set
            {
                SetAttribute<bool>(nameof(ExistsUserAccount), value);
            }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public string SubjectID
        {
            get
            {

                return GetAttribute<string>(nameof(SubjectID));
            }
            set
            {
                SetAttribute<string>(nameof(SubjectID), value);
            }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        [DataMember]
        public string UserName
        {
            get
            {

                return GetAttribute<string>(nameof(UserName));
            }
            set
            {
                SetAttribute<string>(nameof(UserName), value);
            }
        }

        /// <summary>
        /// 第三方认证用到的Scheme名称
        /// </summary>
        [DataMember]
        public string SchemeName
        {
            get
            {

                return GetAttribute<string>(nameof(SchemeName));
            }
            set
            {
                SetAttribute<string>(nameof(SchemeName), value);
            }
        }

        /// <summary>
        /// 第三方认证的用户ID
        /// </summary>
        [DataMember]
        public string ProviderUserId
        {
            get
            {

                return GetAttribute<string>(nameof(ProviderUserId));
            }
            set
            {
                SetAttribute<string>(nameof(ProviderUserId), value);
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
