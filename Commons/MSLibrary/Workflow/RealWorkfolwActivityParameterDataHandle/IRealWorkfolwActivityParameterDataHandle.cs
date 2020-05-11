using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.RealWorkfolwActivityParameterDataHandle
{
    public interface IRealWorkfolwActivityParameterDataHandle
    {
        Task<object> Convert(object data);
        Task<bool> ValidateType(object data);
    }
}
