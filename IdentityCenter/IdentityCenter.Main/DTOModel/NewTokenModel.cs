using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class NewTokenModel:ModelBase
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
    }
}
