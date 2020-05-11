using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Configuration;


namespace MSLibrary.LanguageTranslate
{
    /// <summary>
    /// 默认实现多语言转换接口
    /// 此实现采用读取json的方式实现
    /// json格式为
    /// {
    ///    "code1":
    ///    {
    ///         "1033":"XXXXXXXXX",
    ///         "2052":"XXXXXXXXX"
    ///    },
    ///   "code2":
    ///   {
    ///         "1033":"XXXXXXXXX",
    ///         "2052":"XXXXXXXXX"
    ///   }
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(ILanguageTranslateService), Scope = InjectionScope.Singleton)]
    public class LanguageTranslateService : ILanguageTranslateService
    {
        private static Dictionary<string, Dictionary<int, string>> _textList = null;

        static LanguageTranslateService()
        {

            //因为ConfigurationBuilder不支持键类型非string类型的Dictionary，因此只能先获取Dictionary<string, Dictionary<string, string>>的数据
            //然后再转换成Dictionary<string, Dictionary<int, string>>

            ConfigurationContainer.RegisterJsonListener<Dictionary<string, Dictionary<string, string>>>($"Configurations{Path.DirectorySeparatorChar}Langs{Path.DirectorySeparatorChar}language.json", (textList) =>
            {
                if (textList != null)
                {
                    var strJson = JsonSerializerHelper.Serializer(textList);
                    _textList = JsonSerializerHelper.Deserialize<Dictionary<string, Dictionary<int, string>>>(strJson);
                }
                else
                {
                    _textList = new Dictionary<string, Dictionary<int, string>>();
                }
            });


        }


        public string Translate(string code, int lcid)
        {

            string text = null;
            if (_textList.TryGetValue(code, out Dictionary<int, string>  lcidList))
            {
                if (lcidList.TryGetValue(lcid, out text))
                {
                    return text;
                }
            }

            return text;
        }


    }
}
