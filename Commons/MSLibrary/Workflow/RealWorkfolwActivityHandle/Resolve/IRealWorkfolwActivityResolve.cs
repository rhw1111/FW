using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve
{
    public interface IRealWorkfolwActivityResolve
    {
        Task<RealWorkfolwActivityDescription> Execute(string activityConfiguration);
    }

    public class RealWorkfolwActivityDescription
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, RealWorkfolwActivityParameter> InputParameters { get; set; }
        public Dictionary<string, RealWorkfolwActivityParameter> OutputParameters { get; set; }
        public object Data { get; set; }
    }
}
