using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.Serializer;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Workflow.Description.WorkfalowActivityServices
{
    /// <summary>
    /// 条件分支
    /// </summary>
    [Injection(InterfaceType = typeof(WorkflowActivityServiceForCondition), Scope = InjectionScope.Singleton)]
    public class WorkflowActivityServiceForCondition : WorkflowActivityServiceBase
    {

        public override async Task<IList<object>> GenerateInputParameters(string configuration, IWorkflowContext context)
        {
            var conditionConfiguration=JsonSerializerHelper.Deserialize<ActivityConfiguration>(configuration);
            return new List<object>() { conditionConfiguration };
        }


        protected override async ValueTask<string> GetActivityType()
        {
            return await new ValueTask<string>("Condition");
        }

        protected override async ValueTask<WorkflowActivityParameterDescription> GetParameterDescription(IList<object> inputParameters)
        {
            return await new ValueTask<WorkflowActivityParameterDescription>(new WorkflowActivityParameterDescription()
            {
                 Count=1,
                  Items=new List<WorkflowActivityParameterDescriptionItem>()
                  {
                      new WorkflowActivityParameterDescriptionItem()
                      {
                           AllowNull=false,
                            ExpectType=typeof(ActivityConfiguration)
                      }
                  }
            });
        }

        protected override async Task<IWorkfalowActivityResult> RealExecute(IList<object> inputParameters, IWorkflowContext context)
        {
            WorkflowActivityResultDefault activityResult = new WorkflowActivityResultDefault();

            var configuration = inputParameters[0] as ActivityConfiguration;
            foreach (var item in configuration.Items)
            {
                //计算条件表达式
                var conditionResult = await context.ExpressionCalculator.Calcualte(item.ConditionExpression, context);
                if ((bool)conditionResult)
                {
                    var blockResult = await item.Block.Execute(context);
                    activityResult.GoTo = blockResult;
                    break;
                }
            }

            return activityResult;
        }

        /// <summary>
        /// 条件项配置
        /// </summary>
        private class ConditionItemConfiguration
        {
            /// <summary>
            /// 条件表达式
            /// </summary>
            public string ConditionExpression { get; set; }
            /// <summary>
            /// 要执行的动作块
            /// </summary>
            public WorkflowActivityBlock Block { get; set; }

        }
        private class ActivityConfiguration
        {
            public List<ConditionItemConfiguration> Items { get; set; }
        }
    }


    
}
