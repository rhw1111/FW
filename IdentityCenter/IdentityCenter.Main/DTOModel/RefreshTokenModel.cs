using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class RefreshTokenModel:ModelBase
    {
        /// <summary>
        /// 过期时间(UTC)
        /// </summary>
        [DataMember]
        public DateTime Expire
        {
            get
            {

                return GetAttribute<DateTime>(nameof(Expire));
            }
            set
            {
                SetAttribute<DateTime>(nameof(Expire), value);
            }
        }

        /// <summary>
        /// 新的令牌
        /// </summary>
        [DataMember]
        public string Token
        {
            get
            {

                return GetAttribute<string>(nameof(Token));
            }
            set
            {
                SetAttribute<string>(nameof(Token), value);
            }
        }
        /// <summary>
        /// 新的刷新令牌
        /// </summary>
        [DataMember]
        public string RefreshToken
        {
            get
            {

                return GetAttribute<string>(nameof(RefreshToken));
            }
            set
            {
                SetAttribute<string>(nameof(RefreshToken), value);
            }
        }
    }
}
