using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    /// <summary>
    /// 第三方登录预处理结果
    /// </summary>
    [DataContract]
    public class ExternalLoginPreResult:ModelBase
    {
        /// <summary>
        /// 第三方认证回调地址
        /// </summary>
        [DataMember]
        public string ExternalCallbackUri
        {
            get
            {

                return GetAttribute<string>(nameof(ExternalCallbackUri));
            }
            set
            {
                SetAttribute<string>(nameof(ExternalCallbackUri), value);
            }
        }

    }
}
