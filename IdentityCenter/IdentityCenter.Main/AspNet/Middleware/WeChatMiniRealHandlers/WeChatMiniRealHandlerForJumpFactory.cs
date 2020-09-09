using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace IdentityCenter.Main.AspNet.Middleware.WeChatMiniRealHandlers
{
    [Injection(InterfaceType = typeof(WeChatMiniRealHandlerForJumpFactory), Scope = InjectionScope.Singleton)]
    public class WeChatMiniRealHandlerForJumpFactory : IFactory<IWeChatMiniRealHandler>
    {
        private readonly WeChatMiniRealHandlerForJump _weChatMiniRealHandlerForJump;

        public WeChatMiniRealHandlerForJumpFactory(WeChatMiniRealHandlerForJump weChatMiniRealHandlerForJump)
        {
            _weChatMiniRealHandlerForJump = weChatMiniRealHandlerForJump;
        }
        public IWeChatMiniRealHandler Create()
        {
            return _weChatMiniRealHandlerForJump;
        }
    }
}
