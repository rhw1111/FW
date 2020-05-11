using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Logger
{
    public interface ICommonLogRootContentFactory
    {
        Task<CommonLogRootContent> CreateFromHttpContext(HttpContext context);
    }
}
