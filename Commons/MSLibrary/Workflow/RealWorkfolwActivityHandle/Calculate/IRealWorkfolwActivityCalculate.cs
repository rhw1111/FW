using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Calculate
{
    public interface IRealWorkfolwActivityCalculate
    {
        Task<Dictionary<string, object>> Execute(RealWorkfolwActivityDescription activityDescription, RealWorkfolwContext context,Dictionary<string,object> runtimeParameters);
    }
}
