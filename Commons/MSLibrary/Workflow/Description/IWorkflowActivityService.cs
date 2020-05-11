using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.Description
{
    /// <summary>
    /// 工作流活动服务
    /// </summary>
    public interface IWorkflowActivityService
    {
        Task<IList<object>> GenerateInputParameters(string configuration, IWorkflowContext context);
        Task<IWorkfalowActivityResult> Execute(IList<object> inputParameters,IWorkflowContext context);
    }

    
}
