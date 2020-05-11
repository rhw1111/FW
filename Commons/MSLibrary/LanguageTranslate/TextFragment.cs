using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.LanguageTranslate
{
    /// <summary>
    /// 文本片段
    /// 存储文本代码、替代参数、默认格式化文本
    /// </summary>
    public class TextFragment
    {
        /// <summary>
        /// 文本代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 默认的格式化文本
        /// </summary>
        public string DefaultFormatting { get; set; }
        /// <summary>
        /// 替代参数集合
        /// </summary>
        public List<object> ReplaceParameters { get; set; }
    }
}
