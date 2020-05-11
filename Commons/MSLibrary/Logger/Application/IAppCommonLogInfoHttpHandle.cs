using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Logger.Application
{
    public interface IAppCommonLogInfoHttpHandle
    {
        Task<bool> Do(HttpContext context);
    }
}
