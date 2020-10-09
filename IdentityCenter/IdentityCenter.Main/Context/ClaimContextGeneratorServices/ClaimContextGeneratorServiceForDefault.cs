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

namespace IdentityCenter.Main.Context.ClaimContextGeneratorServices
{
    /// <summary>
    /// 默认的声明上下文生成
    /// </summary>
    [Injection(InterfaceType = typeof(ClaimContextGeneratorServiceForDefault), Scope = InjectionScope.Singleton)]
    public class ClaimContextGeneratorServiceForDefault : IClaimContextGeneratorService
    {
        public void Do(IEnumerable<Claim> claims)
        {
            var dict = new ConcurrentDictionary<string, object>();
            ContextContainer.SetValue(ContextTypes.Dictionary, dict);
            ContextContainer.SetValue(ContextTypes.CurrentUserLcid, 1024);
        }
    }
}
