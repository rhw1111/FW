using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Calculate
{
    [Injection(InterfaceType = typeof(IRealWorkfolwActivityCalculate), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityMainCalculate : IRealWorkfolwActivityCalculate
    {
        private static Dictionary<string, IFactory<IRealWorkfolwActivityCalculate>> _calculateFactories=new Dictionary<string, IFactory<IRealWorkfolwActivityCalculate>>();

        public static Dictionary<string, IFactory<IRealWorkfolwActivityCalculate>> CalculateFactories
        {
            get
            {
                return _calculateFactories;
            }
        }
        public async Task<Dictionary<string, object>> Execute(RealWorkfolwActivityDescription activityDescription, RealWorkfolwContext context, Dictionary<string, object> runtimeParameters)
        {
            if (!_calculateFactories.TryGetValue(activityDescription.Name,out IFactory<IRealWorkfolwActivityCalculate> calculateFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRealWorkflowActivityCalculateByName,
                    DefaultFormatting = "找不到名称为{0}的工作流活动计算",
                    ReplaceParameters = new List<object>() { activityDescription.Name }
                };

                throw new UtilityException((int)Errors.NotFoundRealWorkflowActivityCalculateByName, fragment);
            }

            return await calculateFactory.Create().Execute(activityDescription, context,runtimeParameters);
        }
    }
}
