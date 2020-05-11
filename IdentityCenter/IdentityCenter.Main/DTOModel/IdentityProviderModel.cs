using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class IdentityProviderModel : ModelBase
    {
        /// <summary>
        /// 认证Scheme的名称
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
        /// 显示名称
        /// </summary>
        [DataMember]
        public string DisplayName
        {
            get
            {

                return GetAttribute<string>(nameof(DisplayName));
            }
            set
            {
                SetAttribute<string>(nameof(DisplayName), value);
            }
        }

        /// <summary>
        /// 图标地址
        /// </summary>
        [DataMember]
        public string Icon
        {
            get
            {

                return GetAttribute<string>(nameof(Icon));
            }
            set
            {
                SetAttribute<string>(nameof(Icon), value);
            }
        }
    }
}
