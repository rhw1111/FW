using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ConditionServices
{
    /// Or节点安全验证
    /// Or节点可以包含其他节点，在它之下的节点按或逻辑组合
    /// 任意一个通过验证直接返回
    /// 节点格式为<or></or>
    [Injection(InterfaceType = typeof(ElementConditionServiceForOr), Scope = InjectionScope.Singleton)]
    public class ElementConditionServiceForOr : IElementConditionService
    {
        private ISelector<IElementConditionService> _selector;
        public ElementConditionServiceForOr(ISelector<IElementConditionService> selector)
        {
            _selector = selector;
        }

        public async Task<ValidateResult> Validate(XElement securityElement, Dictionary<string, object> actionKeyInfo)
        {
            //获取节点下的子节点
            var childElements = securityElement.Elements();

            var strErrorReason = new StringBuilder();

            foreach (var childItem in childElements)
            {
                var elementValidate = _selector.Choose(childItem.Name.ToString());
                var elementValidateResult = await elementValidate.Validate(childItem, actionKeyInfo);
                if (elementValidateResult.Result)
                {
                    return elementValidateResult;
                }
                else
                {
                    if (!string.IsNullOrEmpty(elementValidateResult.Description))
                    {
                        strErrorReason.AppendLine(elementValidateResult.Description);
                        strErrorReason.AppendLine("\r");
                    }
                }
            }

            return new ValidateResult { Result = false, Description = strErrorReason.ToString() };
        }
    }
}
