using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve;
using MSLibrary.Workflow.RealWorkfolwActivityParameterHandle;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Calculate
{
    [Injection(InterfaceType = typeof(RealWorkfolwActivityCalculateForResult), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityCalculateForResult : IRealWorkfolwActivityCalculate
    {
        private IRealWorkfolwActivityParameterHandle _realWorkfolwActivityParameterHandle;
        private IRealWorkfolwActivityCalculate _realWorkfolwActivityCalculate;

        public RealWorkfolwActivityCalculateForResult(IRealWorkfolwActivityParameterHandle realWorkfolwActivityParameterHandle, IRealWorkfolwActivityCalculate realWorkfolwActivityCalculate)
        {
            _realWorkfolwActivityParameterHandle = realWorkfolwActivityParameterHandle;
            _realWorkfolwActivityCalculate = realWorkfolwActivityCalculate;
        }
        public async Task<Dictionary<string, object>> Execute(RealWorkfolwActivityDescription activityDescription, RealWorkfolwContext context, Dictionary<string, object> runtimeParameters)
        {
            RealWorkfolwActivityDescriptionDataForResult data = activityDescription.Data as RealWorkfolwActivityDescriptionDataForResult;
            if (data == null)
            {
                string realType;
                if (activityDescription.Data == null)
                {
                    realType = "null";
                }
                else
                {
                    realType = activityDescription.Data.GetType().FullName;
                }

                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityDescriptionDataTypeNotMatch,
                    DefaultFormatting = "工作流活动描述的Data属性的类型不匹配，期待的类型为{0}，实际类型为{1}，发生位置：{2}",
                    ReplaceParameters = new List<object>() { typeof(RealWorkfolwActivityDescriptionDataForResult).FullName, realType, $"{this.GetType().FullName}.Execute" }
                };

                throw new UtilityException((int)Errors.RealWorkfolwActivityDescriptionDataTypeNotMatch, fragment);
            }

            foreach(var parameterItem in data.ResultParameters)
            {
                var conditionParameterResultObj = await _realWorkfolwActivityParameterHandle.Execute(parameterItem, context);
                context.Result[parameterItem.Name] = conditionParameterResultObj;
            }

            return new Dictionary<string, object>();
        }
    }
}
