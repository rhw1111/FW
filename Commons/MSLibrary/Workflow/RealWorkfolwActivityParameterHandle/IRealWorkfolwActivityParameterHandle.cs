using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.RealWorkfolwActivityParameterHandle
{
    public interface IRealWorkfolwActivityParameterHandle
    {
        Task<object> Execute(RealWorkfolwActivityParameter parameter, RealWorkfolwContext context);
    }
}
