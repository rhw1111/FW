using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Configuration;

namespace IdentityCenter.Main.Configuration
{
    /// <summary>
    /// 系统配置信息
    /// 扩展至
    /// </summary>
    [DataContract]
    public class ApplicationConfiguration:CoreConfiguration
    {
        /// <summary>
        /// 应用程序名称
        /// </summary>
        [DataMember]
        public string ApplicationName { get; set; } = null!;

    }

}
