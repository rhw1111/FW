using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.RealWorkfolwConfiguration
{
    public interface IParameterConfigurationService
    {
        Task<RealWorkfolwActivityParameter> ResolveParameter(string parameterConfiguration);
    }
}
