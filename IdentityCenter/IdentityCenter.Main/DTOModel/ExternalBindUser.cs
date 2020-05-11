using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    /// <summary>
    /// 第三方用户绑定DTO
    /// </summary>
    [DataContract]
    public class ExternalBindUser:ModelBase
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
    }
}
