using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.MessageHandle.CrmAttributeMetadataHandle
{
    [Injection(InterfaceType = typeof(ICrmAttributeMetadataHandleSelector), Scope = InjectionScope.Singleton)]
    public class CrmAttributeMetadataHandleSelector : ICrmAttributeMetadataHandleSelector
    {
        private static Dictionary<string, IFactory<ICrmAttributeMetadataHandle>> _handleFactories = new Dictionary<string, IFactory<ICrmAttributeMetadataHandle>>();

        public static Dictionary<string, IFactory<ICrmAttributeMetadataHandle>> HandleFactories
        {
            get
            {
                return _handleFactories;
            }
        }
        public ICrmAttributeMetadataHandle Choose(string name)
        {
            if (!_handleFactories.TryGetValue(name,out IFactory<ICrmAttributeMetadataHandle> handleFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmAttributeMetadataHandleByType,
                    DefaultFormatting = "找不到类型为{0}的Crm属性元数据处理",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundCrmAttributeMetadataHandleByType, fragment);
            }

            return handleFactory.Create();
        }
    }
}
