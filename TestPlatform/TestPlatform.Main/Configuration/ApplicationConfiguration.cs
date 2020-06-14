using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary.Configuration;

namespace FW.TestPlatform.Main.Configuration
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    [DataContract]
    public class ApplicationConfiguration : CoreConfiguration
    {
        /// <summary>
        /// 应用程序名称
        /// </summary>
        [DataMember]
        public string ApplicationName { get; set; } = null!;
    }
}
