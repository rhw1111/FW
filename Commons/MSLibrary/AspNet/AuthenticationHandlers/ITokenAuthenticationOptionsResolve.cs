using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.AspNet.AuthenticationHandlers
{
    public interface ITokenAuthenticationOptionsResolve
    {
        Task<string> GetHeaderName(HttpRequest request);
        Task<string> GetTokenControllerName(HttpRequest request);
    }
}
