using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SRR.Filter
{
    public class SRRFilterForOverride : ISRRFilter, ISRROverrideFilter
    {
        public async Task ExecutePre(ISRRFilterContext context)
        {
            await Task.FromResult(0);
        }
        public async Task ExecutePost(ISRRFilterContext context)
        {
            await Task.FromResult(0);
        }

        public async Task Finally(ISRRFilterContext context)
        {
            await Task.FromResult(0);
        }

        public void ExecutePreSync(ISRRFilterContext context)
        {
            throw new NotImplementedException();
        }

        public void ExecutePostSync(ISRRFilterContext context)
        {
            throw new NotImplementedException();
        }

        public void FinallySync(ISRRFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
