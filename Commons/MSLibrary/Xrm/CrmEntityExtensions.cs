using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Xrm.Convert;
using Newtonsoft.Json.Linq;
using MSLibrary.LanguageTranslate;
using System.Net.Http.Headers;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// CrmEntity的扩展方法
    /// </summary>
    public static class CrmEntityExtensions
    {
        public static ICrmRetrieveJTokenConvertService JTokenConvertService { private get; set; } = new CrmRetrieveJTokenConvertService();
        public static async Task<T> GetAttributeValue<T>(this CrmEntity entity,string attributeName, Dictionary<string, object> extensionParameters = null)
        {
            if (!entity.Attributes.TryGetValue(attributeName,out JToken jValue ))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAttributeNameInRetrieveCrmEntity,
                    DefaultFormatting = "在查询获取的CrmEntity中的属性中，找不到名称为{0}的属性，实体名称为{1}",
                    ReplaceParameters = new List<object>() { attributeName, entity .EntityName}
                };

                throw new UtilityException((int)Errors.NotFoundAttributeNameInRetrieveCrmEntity, fragment);
            }
            T result=default(T);
            Type type = typeof(T);
            if (type==typeof(int?) || type == typeof(float?) || type == typeof(DateTime?) || type == typeof(bool?)
                || type == typeof(string) || type == typeof(decimal?)|| type == typeof(double?)|| type == typeof(long?)
                )
            {
                result= jValue.ToObject<T>();
            }
            else
            {
                result=await JTokenConvertService.Convert<T>(jValue, extensionParameters);
            }

            return result;


        }
    }
}
