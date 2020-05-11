using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context;

namespace IdentityCenter.Main.Context.InternationalizationHandleServices
{
    [Injection(InterfaceType = typeof(InternationalizationHandleServiceForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class InternationalizationHandleServiceForDefaultFactory : IFactory<IInternationalizationHandleService>
    {
        private InternationalizationHandleServiceForDefault _internationalizationHandleServiceForDefault;

        public InternationalizationHandleServiceForDefaultFactory(InternationalizationHandleServiceForDefault internationalizationHandleServiceForDefault)
        {
            _internationalizationHandleServiceForDefault = internationalizationHandleServiceForDefault;
        }
        public IInternationalizationHandleService Create()
        {
            return _internationalizationHandleServiceForDefault;
        }
    }
}
