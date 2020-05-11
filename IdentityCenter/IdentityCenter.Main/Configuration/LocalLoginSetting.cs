using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace IdentityCenter.Main.Configuration
{
    /// <summary>
    /// 本地登录设置
    /// </summary>
    [DataContract]
    public class LocalLoginSetting
    {
        /// <summary>
        /// 是否允许本地登录
        /// </summary>
        [DataMember]
        public bool AllowLocalLogin
        {
            get;set;
        }
        /// <summary>
        /// 是否保持登录
        /// </summary>
        [DataMember]
        public bool AllowRememberLogin
        {
            get;
            set;
        }
        /// <summary>
        /// 保持登录的时间（天）
        /// </summary>
        [DataMember]
        public int RememberLoginDuration
        {
            get;
            set;
        }
        /// <summary>
        /// 是否显示登出窗口
        /// </summary>
        [DataMember]
        public bool ShowLogoutPrompt
        {
            get;
            set;
        }
    }
}
