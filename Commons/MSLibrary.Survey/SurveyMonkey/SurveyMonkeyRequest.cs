using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey.SurveyMonkey
{
    /// <summary>
    /// SurveyMonkey的请求
    /// </summary>
    public class SurveyMonkeyRequest
    {
        protected SurveyMonkeyRequest(string type)
        {
            Type = type;
        }
        /// <summary>
        /// 请求类型
        /// </summary>
        public string Type { get; private set; } = null!;
        /// <summary>
        /// 服务地址
        /// </summary>
       public string Address { get; internal set; } = null!;
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; internal set; } = null!;
    }
}
