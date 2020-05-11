using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.RealWorkfolwConfiguration
{
    public interface IActivityConfigurationService
    {
        Task<List<RealWorkfolwActivity>> SeparateActivities(string workflowConfiguration);

        Task<RealWorkfolwActivity> ResolveActivity(string activityConfiguration);

        Task<Dictionary<string, RealWorkfolwActivityParameter>> ResolveActivityInputParameters(string activityConfiguration);
        Task<Dictionary<string, RealWorkfolwActivityParameter>> ResolveActivityOutputParameters(string activityConfiguration);

    }
}
