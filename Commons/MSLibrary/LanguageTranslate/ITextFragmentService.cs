using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.LanguageTranslate
{
    /// <summary>
    /// 文本片段服务
    /// </summary>
    public interface ITextFragmentService
    {
        /// <summary>
        /// 获取文本片段在指定语言编码下的输出
        /// </summary>
        /// <param name="lcid"></param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        Task<string> GetLanguageText(int lcid, TextFragment fragment);
        /// <summary>
        /// 获取文本片段在指定语言编码下的输出（同步）
        /// </summary>
        /// <param name="lcid"></param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        string GetLanguageTextSync(int lcid, TextFragment fragment);
        /// <summary>
        /// 获取文本片段的默认输出
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        Task<string> GetDefaultText(TextFragment fragment);
        /// <summary>
        /// 获取文本片段的默认输出（同步）
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        string GetDefaultTextSync(TextFragment fragment);
    }
}
