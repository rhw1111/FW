using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context;
using MSLibrary.Serializer;

namespace IdentityCenter.Main.Context.InternationalizationHandleServices
{
    /// <summary>
    /// 国际化处理服务的默认实现
    /// 从Querysring和Header中获取国际化信息
    /// Querysring优先
    /// </summary>
    [Injection(InterfaceType = typeof(InternationalizationHandleServiceForDefault), Scope = InjectionScope.Singleton)]
    public class InternationalizationHandleServiceForDefault : IInternationalizationHandleService
    {
        private const string _lang = "lang";
        private const string _tzo = "tzo";
        public void GenerateContext(object internationalizationInfo)
        {
            var realInfo = (InternationalizationInfo)internationalizationInfo;
            ContextContainer.SetValue<int>(ContextTypes.CurrentUserLcid, realInfo.Lcid);
            ContextContainer.SetValue<int>(ContextTypes.CurrentUserTimezoneOffset, realInfo.TimezoneOffset);
        }

        public async Task<object> GetInternationalizationInfo(HttpRequest request)
        {
            bool lcidLoad = false;
            bool tzoLoad = false;
            InternationalizationInfo infoResult = new InternationalizationInfo()
            {
                Lcid = 2052,
                TimezoneOffset = -480
            };
            if (request.Query.TryGetValue(_lang, out StringValues langValues))
            {
                if (int.TryParse(langValues[0], out int intLang))
                {
                    infoResult.Lcid = intLang;
                    lcidLoad = true;
                }
            }

            if (!lcidLoad)
            {
                if (request.Headers.TryGetValue(_lang, out langValues))
                {
                    if (int.TryParse(langValues[0], out int intLang))
                    {
                        infoResult.Lcid = intLang;
                    }
                }
            }

            if (request.Query.TryGetValue(_tzo, out StringValues tzoValues))
            {
                if (int.TryParse(tzoValues[0], out int intTzo))
                {
                    infoResult.TimezoneOffset = intTzo;
                    tzoLoad = true;
                }
            }

            if (!tzoLoad)
            {
                if (request.Headers.TryGetValue(_tzo, out tzoValues))
                {
                    if (int.TryParse(tzoValues[0], out int intTzo))
                    {
                        infoResult.TimezoneOffset = intTzo;
                    }
                }
            }

            return await Task.FromResult(infoResult);
        }

        private class InternationalizationInfo
        {
            public int Lcid { get; set; }

            public int TimezoneOffset { get; set; }
        }
    }
}
