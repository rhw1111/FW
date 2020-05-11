using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController
{
    [Injection(InterfaceType = typeof(IConditionService), Scope = InjectionScope.Singleton)]
    public class ConditionMainService : IConditionService
    {
        private ISelector<IElementConditionService> _validateElementServiceSelector;

        public ConditionMainService(ISelector<IElementConditionService> validateElementServiceSelector)
        {
            _validateElementServiceSelector = validateElementServiceSelector;
        }
        public async Task<ValidateResult> Validate(XDocument doc, Dictionary<string, object> parameters)
        {

            var childElements = doc.Root.Elements();

            foreach (var childItem in childElements)
            {
                var elementValidate = _validateElementServiceSelector.Choose(childItem.Name.ToString());
                var elementValidateResult = await elementValidate.Validate(childItem, parameters);
                if (!elementValidateResult.Result)
                {
                    return elementValidateResult;
                }
            }

            return new ValidateResult { Result = true, Description = string.Empty };
        }
    }


    /// <summary>
    /// 节点逻辑条件验证服务
    /// </summary>
    public interface IElementConditionService
    {
        /// <summary>
        /// 验证逻辑条件是否满足
        /// </summary>
        /// <param name="securityElement">逻辑条件节点</param>
        /// <param name="parameters">辅助参数</param>
        /// <returns></returns>
        Task<ValidateResult> Validate(XElement securityElement, Dictionary<string, object> parameters);
    }
}
