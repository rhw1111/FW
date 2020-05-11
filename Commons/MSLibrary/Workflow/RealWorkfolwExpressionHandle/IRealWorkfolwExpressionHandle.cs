using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.RealWorkfolwExpressionHandle
{
    public interface IRealWorkfolwExpressionHandle
    {
        Task<object> Execute(List<object> inputs, RealWorkfolwContext context);
    }
}
