using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Grpc.Context.GrpcExtensionContextHandleServices
{
    [Injection(InterfaceType = typeof(GrpcExtensionContextHandleServiceForTraceFactory), Scope = InjectionScope.Singleton)]
    public class GrpcExtensionContextHandleServiceForTraceFactory : IFactory<IGrpcExtensionContextHandleService>
    {
        private readonly GrpcExtensionContextHandleServiceForTrace _grpcExtensionContextHandleServiceForTrace;

        public GrpcExtensionContextHandleServiceForTraceFactory(GrpcExtensionContextHandleServiceForTrace grpcExtensionContextHandleServiceForTrace)
        {
            _grpcExtensionContextHandleServiceForTrace = grpcExtensionContextHandleServiceForTrace;
        }
        public IGrpcExtensionContextHandleService Create()
        {
            return _grpcExtensionContextHandleServiceForTrace;
        }
    }
}
