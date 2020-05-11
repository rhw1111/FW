using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.LanguageTranslate
{
    public static class TextFragmentExtension
    {     
        public static IFactory<ITextFragmentService> _textFragmentServiceFactory = new TextFragmentServiceDefalutFactory();

        public static IFactory<ITextFragmentService> TextFragmentServiceFactory
        {
            set
            {
                _textFragmentServiceFactory = value;
            }
        }

        /// <summary>
        /// 获取文本片段在指定语言编码下的输出
        /// </summary>
        /// <param name="fragment"></param>
        /// <param name="lcid"></param>
        /// <returns></returns>
        public static async Task<string> GetLanguageText(this TextFragment fragment,int lcid)
        {
            return await _textFragmentServiceFactory.Create().GetLanguageText(lcid, fragment);
        }

        /// <summary>
        /// 获取文本片段在指定语言编码下的输出（同步）
        /// </summary>
        /// <param name="fragment"></param>
        /// <param name="lcid"></param>
        /// <returns></returns>
        public static string GetLanguageTextSync(this TextFragment fragment,int lcid)
        {
            return _textFragmentServiceFactory.Create().GetLanguageTextSync(lcid, fragment);
        }

        /// <summary>
        /// 获取文本片段的默认输出
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public static async Task<string> GetDefaultText(this TextFragment fragment)
        {
            return await _textFragmentServiceFactory.Create().GetDefaultText(fragment);
        }
        /// <summary>
        /// 获取文本片段的默认输出（同步）
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public static string GetDefaultTextSync(this TextFragment fragment)
        {
            return _textFragmentServiceFactory.Create().GetDefaultTextSync(fragment);
        }
    }
}
