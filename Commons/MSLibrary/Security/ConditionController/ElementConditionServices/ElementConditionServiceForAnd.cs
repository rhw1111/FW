using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ConditionServices
{
    /// And节点安全验证
    /// And节点可以包含其他节点，在它之下的节点按与逻辑组合
    /// 任意一个不通过验证直接返回
    /// 节点格式为<and></and>
    [Injection(InterfaceType = typeof(ElementConditionServiceForAnd), Scope = InjectionScope.Singleton)]
    public class ElementConditionServiceForAnd : IElementConditionService
    {
        private ISelector<IElementConditionService> _selector;
        public ElementConditionServiceForAnd(ISelector<IElementConditionService> selector)
        {
            _selector = selector;
        }

        public async Task<ValidateResult> Validate(XElement securityElement, Dictionary<string, object> actionKeyInfo)
        {
            //获取节点下的子节点
            var childElements = securityElement.Elements();

            foreach (var childItem in childElements)
            {
                var elementValidate = _selector.Choose(childItem.Name.ToString());

                var elementValidateResult = await elementValidate.Validate(childItem, actionKeyInfo);
                if (!elementValidateResult.Result)
                {
                    return elementValidateResult;
                }
            }

            return new ValidateResult { Result = true, Description = string.Empty };
        }
    }
}
