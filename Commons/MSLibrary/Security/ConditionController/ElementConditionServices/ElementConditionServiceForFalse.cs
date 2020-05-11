using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.ConditionController.ElementConditionServices
{
    /// <summary>
    /// 直接返回False
    /// </summary>
    public class ElementConditionServiceForFalse : IElementConditionService
    {
        public async Task<ValidateResult> Validate(XElement securityElement, Dictionary<string, object> parameters)
        {
            var result = new ValidateResult() { Result = false, Description = StringLanguageTranslate.Translate(TextCodes.ConditionFalse, "条件直接被设置为Fasle") };
            return await Task.FromResult(result);
        }
        
    }
}
