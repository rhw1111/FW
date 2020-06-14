using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MSLibrary;
using MSLibrary.Context;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Context.HttpExtensionContextHandleServices
{

    [Injection(InterfaceType = typeof(HttpExtensionContextHandleServiceForInternationalization), Scope = InjectionScope.Singleton)]
    public class HttpExtensionContextHandleServiceForInternationalization : IHttpExtensionContextHandleService
    {
        public void GenerateContext(object info)
        {
            var realInfo = (InternationalizationInfo)info;
            ContextContainer.SetValue<int>(ContextTypes.CurrentUserLcid, realInfo.Lcid);
            ContextContainer.SetValue<int>(ContextTypes.CurrentUserTimezoneOffset, realInfo.TimezoneOffset);
        }

        public async Task<object> GetInfo(HttpRequest request)
        {
            int lang = 2052;
            int to = -480;
            if (request.Headers.TryGetValue("lc", out StringValues langValues))
            {
                var strLang = langValues[0].Trim();
                if (!int.TryParse(strLang,out lang))
                {
                    lang = 1033;
                }
            }


            if (request.Headers.TryGetValue("to", out StringValues toValues))
            {
                var strTO = langValues[0].Trim();
                if (!int.TryParse(strTO, out to))
                {
                    to = 480;
                }
            }


            return await Task.FromResult(
                            new InternationalizationInfo()
                            {
                                Lcid = lang,
                                TimezoneOffset = to
                            }
                            );
        }


        private class InternationalizationInfo
        {
            public int Lcid { get; set; }

            public int TimezoneOffset { get; set; }
        }
    }
}
