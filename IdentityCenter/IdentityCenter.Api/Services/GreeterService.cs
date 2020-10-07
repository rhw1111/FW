using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSLibrary;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace IdentityCenter.Api
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            throw new Exception("aaa");
            var trace=ContextContainer.GetValue<IRequestTraceInofContext>(ContextTypes.Trace);
            var lcid=ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
