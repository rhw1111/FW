using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Security.BusinessSecurityRule;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.Filter.Application
{
    [Injection(InterfaceType = typeof(IAppBusinessActionValidate), Scope = InjectionScope.Singleton)]
    public class AppBusinessActionValidate : IAppBusinessActionValidate
    {
        private IBusinessActionRepository _businessActionRepository;

        public AppBusinessActionValidate(IBusinessActionRepository businessActionRepository)
        {
            _businessActionRepository = businessActionRepository;
        }
        public async Task<ValidateResult> Do(string actionName, Dictionary<string, object> parameters)
        {
            var action=await _businessActionRepository.QueryByName(actionName);
            if (action==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundBusinessActionByName,
                    DefaultFormatting = "找不到名称为{0}的业务动作",
                    ReplaceParameters = new List<object>() { actionName }
                };

                throw new UtilityException((int)Errors.NotFoundBusinessActionByName, fragment);
            }

            return await action.Validate(parameters);
        }
    }
}
