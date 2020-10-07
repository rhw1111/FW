using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSLibrary.Grpc.Interceptors.Application
{
    public interface IAppExceptionGrpcContextLogConvert
    {
        Task<object> Convert(ServerCallContext context);
    }
}
