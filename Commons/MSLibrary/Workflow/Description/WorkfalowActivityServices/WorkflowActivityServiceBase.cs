using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Workflow.Description.WorkfalowActivityServices
{
    public abstract class WorkflowActivityServiceBase : IWorkflowActivityService
    {
        public async Task<IWorkfalowActivityResult> Execute(IList<object> inputParameters, IWorkflowContext context)
        {
            var activityType = await GetActivityType();
            var description = await GetParameterDescription(inputParameters);

            if (description.Count.HasValue && inputParameters.Count!= description.Count)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.WorkflowActivityServiceParameterCountError,
                    DefaultFormatting = "类型为{0}的工作流活动服务参数数量不正确，需要的数量为{1}，实际数量为{2}",
                    ReplaceParameters = new List<object>() { activityType, description.Count, inputParameters.Count }
                };

                throw new UtilityException((int)Errors.WorkflowActivityServiceParameterCountError, fragment);
            }

            for(var index=0;index<=description.Items.Count-1;index++)
            {
                if (!description.Items[index].AllowNull && inputParameters[index]==null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.WorkflowActivityServiceParameterCountError,
                        DefaultFormatting = "类型为{0}的工作流活动服务的第{1}位参数类型不正确，期待的类型为{2}，实际类型为{3}",
                        ReplaceParameters = new List<object>() { activityType, index, description.Items[index].ExpectType.FullName, "null" }
                    };

                    throw new UtilityException((int)Errors.WorkflowActivityServiceParameterCountError, fragment);
                }

                if (inputParameters[index]!=null && !description.Items[index].ExpectType.IsInstanceOfType(inputParameters[index]))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.WorkflowActivityServiceParameterCountError,
                        DefaultFormatting = "类型为{0}的工作流活动服务的第{1}位参数类型不正确，期待的类型为{2}，实际类型为{3}",
                        ReplaceParameters = new List<object>() { activityType, index, description.Items[index].ExpectType.FullName, inputParameters[index].GetType().FullName }
                    };

                    throw new UtilityException((int)Errors.WorkflowActivityServiceParameterCountError, fragment);
                }
            }

            return await RealExecute(inputParameters, context);
        }

        protected abstract Task<IWorkfalowActivityResult> RealExecute(IList<object> inputParameters, IWorkflowContext context);

        protected abstract ValueTask<WorkflowActivityParameterDescription> GetParameterDescription(IList<object> inputParameters);

        protected abstract ValueTask<string> GetActivityType();

        public abstract Task<IList<object>> GenerateInputParameters(string configuration, IWorkflowContext context);
    }

    public class WorkflowActivityParameterDescriptionItem
    {
        public Type ExpectType { get; set; }
        public bool AllowNull { get; set; }
    }

    public class WorkflowActivityParameterDescription
    {
        
        public int? Count { get; set; }
        public List<WorkflowActivityParameterDescriptionItem> Items { get; set; }
    }
}
