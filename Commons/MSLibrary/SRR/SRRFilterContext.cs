using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MSLibrary.SRR
{
    public class SRRFilterContext : ISRRFilterContext
    {
        private SRRRequest _request;
        public SRRFilterContext(SRRRequest request)
        {
            _request = request;
        }
        public SRRRequest Request
        {
            get
            {
                return _request;
            }
        }

        public ClaimsPrincipal Identity { get; set; }

        public IDictionary<string, object> Items { get; } = new Dictionary<string, object>();

        public SRRResponse Response { get; set; }
    }
}
