using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(ICrmMessageHandleSelector), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleSelector : ICrmMessageHandleSelector
    {
        private static Dictionary<string, IFactory<ICrmMessageHandle>> _handleFactories = new Dictionary<string, IFactory<ICrmMessageHandle>>();

        public static Dictionary<string, IFactory<ICrmMessageHandle>> HandleFactories
        {
            get
            {
                return _handleFactories;
            }
        }
        public ICrmMessageHandle Choose(string name)
        {
            
            if (!_handleFactories.TryGetValue(name, out IFactory<ICrmMessageHandle> handleFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmMessageHandleByRequestTypeFullName,
                    DefaultFormatting = "找不到请求类型名称为{0}的Crm消息处理，发生位置：{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.Choose" }
                };

                throw new UtilityException((int)Errors.NotFoundCrmMessageHandleByRequestTypeFullName, fragment);
            }
            return handleFactory.Create();
        }
    }
}
