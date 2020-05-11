using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Workflow.DAL;

namespace MSLibrary.Workflow
{
    [Injection(InterfaceType = typeof(ICommonSignConfigurationRepository), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationRepository : ICommonSignConfigurationRepository
    {
        private ICommonSignConfigurationStore _commonSignConfigurationStore;

        public CommonSignConfigurationRepository(ICommonSignConfigurationStore commonSignConfigurationStore)
        {
            _commonSignConfigurationStore = commonSignConfigurationStore;
        }

        public async Task QueryByEntityType(string entityType, Func<CommonSignConfiguration, Task> callback)
        {
            await _commonSignConfigurationStore.QueryByEntityType(entityType, callback);
        }

        public async Task<CommonSignConfiguration> QueryByWorkflowResourceType(string workflowResourceType)
        {
            return await _commonSignConfigurationStore.QueryByWorkflowResourceType(workflowResourceType);
        }
    }
}
