using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context;
using MSLibrary.Context.ClaimContextGeneratorServices;
using System.Security.Claims;

namespace FW.TestPlatform.Main.Context.ClaimContextGeneratorServices
{
    /// <summary>
    /// 默认的声明上下文生成
    /// </summary>
    [Injection(InterfaceType = typeof(ClaimContextGeneratorServiceForDefault), Scope = InjectionScope.Singleton)]
    public class ClaimContextGeneratorServiceForDefault : IClaimContextGeneratorService
    {

        public void Do(IEnumerable<Claim> claims)
        {
            var strUserID = (from item in claims
                            where item.Type == ClaimsTypes.UserID
                            select item.Value).FirstOrDefault();

            var strLcid = (from item in claims
                             where item.Type == ClaimsTypes.Lcid
                             select item.Value).FirstOrDefault();

            var strTO = (from item in claims
                           where item.Type == ClaimsTypes.TimezoneOffset
                           select item.Value).FirstOrDefault();


            ContextContainer.SetValue(ContextTypes.CurrentUserId, Guid.Parse(strUserID));
            ContextContainer.SetValue(ContextTypes.CurrentUserLcid, int.Parse(strLcid));
            ContextContainer.SetValue(ContextTypes.CurrentUserTimezoneOffset, int.Parse(strTO));

            var dict = new ConcurrentDictionary<string, object>();
            ContextContainer.SetValue(ContextTypes.Dictionary, dict);
        }
    }
}
