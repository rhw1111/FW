using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.LanguageTranslate
{
    /// <summary>
    /// 字符串类型的多语言转换扩展方法类
    /// 所有需要多语言的文本必须从该类中获取
    /// </summary>
    public class StringLanguageTranslate
    {

        private static ILanguageTranslateService _languageTranslateService = new LanguageTranslateService();

        /// <summary>
        /// 多语言转换需要用到的转换服务
        /// 默认为LanguageTranslateService
        /// </summary>
        public static ILanguageTranslateService LanguageTranslateService
        {
            get { return _languageTranslateService; }
            set { _languageTranslateService = value; }
        }

        /// <summary>
        /// 多语言转换
        /// 如果多语言文件中可以找到对饮文本编号和当前用户语言编号的文本
        /// 则返回找到的文本，否则返回默认文本
        /// </summary>
        /// <param name="code">文本编号</param>
        /// <param name="strDefault">默认文本</param>
        /// <returns></returns>
        public static string Translate(string code, string strDefault)
        {
            //从上下文中获取当前用户语言编码
            var lcid = ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);
            return Translate(code, lcid, strDefault);
        }


        public static string Translate(string code,int lcid, string strDefault)
        {
            var text = _languageTranslateService.Translate(code, lcid);

            if (text != null)
            {
                return text;
            }
            else
            {
                return strDefault;
            }
        }

    }
}
