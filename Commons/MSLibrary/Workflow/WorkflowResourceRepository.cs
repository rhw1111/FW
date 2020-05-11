using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Workflow.DAL;

namespace MSLibrary.Workflow
{
    [Injection(InterfaceType = typeof(IWorkflowResourceRepository), Scope = InjectionScope.Singleton)]
    public class WorkflowResourceRepository : IWorkflowResourceRepository
    {
        private IWorkflowResourceStore _workflowResourceStore;

        public WorkflowResourceRepository(IWorkflowResourceStore workflowResourceStore)
        {
            _workflowResourceStore = workflowResourceStore;
        }
        public async Task<WorkflowResource> QueryByKey(string type, string key)
        {
            return await _workflowResourceStore.QueryByKey(type,key);
        }
    }
}
