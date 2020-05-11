using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    /// <summary>
    /// 认证客户端主机信息模型
    /// </summary>
    [DataContract]
    public class IdentityClientHostInfoModel:ModelBase
    {
        /// <summary>
        /// 允许的跨域源列表
        /// </summary>
        [DataMember]
        public List<string> AllowedCorsOrigins
        {
            get
            {

                return GetAttribute<List<string>>(nameof(AllowedCorsOrigins));
            }
            set
            {
                SetAttribute<List<string>>(nameof(AllowedCorsOrigins), value);
            }
        }
    }
}
