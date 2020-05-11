using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle;

namespace MSLibrary.Xrm.Convert
{
    [Injection(InterfaceType = typeof(ICrmRetrieveJTokenConvertService), Scope = InjectionScope.Singleton)]
    public class CrmRetrieveJTokenConvertService : ICrmRetrieveJTokenConvertService
    {
        private static Dictionary<Type, IFactory<ICrmRetrieveJTokenHandle>> _handleFactories = new Dictionary<Type, IFactory<ICrmRetrieveJTokenHandle>>();

        public static Dictionary<Type, IFactory<ICrmRetrieveJTokenHandle>> HandleFactories
        {
            get
            {
                return _handleFactories;
            }
        }

        public async Task<T> Convert<T>(JToken json, Dictionary<string, object> extensionParameters = null)
        {
            var type=typeof(T);
            if (!_handleFactories.TryGetValue(type,out IFactory<ICrmRetrieveJTokenHandle> handleFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmRetrieveJTokenHandleByType,
                    DefaultFormatting = "找不到类型为{0}的Crm查询结果JToken处理",
                    ReplaceParameters = new List<object>() { type.FullName }
                };

                throw new UtilityException((int)Errors.NotFoundCrmRetrieveJTokenHandleByType, fragment);
            }

            var handleService = handleFactory.Create();
            var handleResult=await handleService.Execute(json, extensionParameters);

            if (handleResult==null)
            {
                return default(T);
            }

            if (!(handleResult is T))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRetrieveJTokenHandleResultTypeNotMatch,
                    DefaultFormatting = "类型为{0}的Crm查询结果JToken处理返回的结果的类型与期望类型不匹配，期望类型为{1}，实际类型为{2}",
                    ReplaceParameters = new List<object>() { handleService.GetType().FullName, typeof(T).FullName, handleResult.GetType().FullName }
                };

                throw new UtilityException((int)Errors.CrmRetrieveJTokenHandleResultTypeNotMatch, fragment);
            }

            return (T)handleResult;
        }
    }
}
