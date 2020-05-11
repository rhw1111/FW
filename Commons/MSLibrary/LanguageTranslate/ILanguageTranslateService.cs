using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.LanguageTranslate
{
    /// <summary>
    /// 多语言转换接口
    /// </summary>
    public interface ILanguageTranslateService
    {
        /// <summary>
        /// 获取指定名称和指定语言编号的文本
        /// </summary>
        /// <param name="code">名称</param>
        /// <param name="lcid">语言编号</param>
        /// <returns>对应语言的文本</returns>
        string Translate(string code, int lcid);
    }
}
