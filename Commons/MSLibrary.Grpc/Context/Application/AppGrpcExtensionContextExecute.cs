using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using MSLibrary.DI;

namespace MSLibrary.Grpc.Context.Application
{
    [Injection(InterfaceType = typeof(IAppGrpcExtensionContextExecute), Scope = InjectionScope.Singleton)]
    public class AppGrpcExtensionContextExecute : IAppGrpcExtensionContextExecute
    {
        private IGrpcExtensionContextHandleServiceFactorySelector _grpcExtensionContextHandleServiceFactorySelector;

        public AppGrpcExtensionContextExecute(IGrpcExtensionContextHandleServiceFactorySelector grpcExtensionContextHandleServiceFactorySelector)
        {
            _grpcExtensionContextHandleServiceFactorySelector = grpcExtensionContextHandleServiceFactorySelector;
        }

        public async Task<IGrpcExtensionContextInit> Do(ServerCallContext callContext, string name)
        {
            var grpcExtensionContextHandleService = _grpcExtensionContextHandleServiceFactorySelector.Choose(name).Create();
            var info = await grpcExtensionContextHandleService.GetInfo(callContext);

            return new GrpcExtensionContextInit(grpcExtensionContextHandleService, info);
        }

        private class GrpcExtensionContextInit : IGrpcExtensionContextInit
        {
            private IGrpcExtensionContextHandleService _grpcExtensionContextHandleService;
            private object _info;
            public GrpcExtensionContextInit(IGrpcExtensionContextHandleService grpcExtensionContextHandleService, object info)
            {
                _grpcExtensionContextHandleService = grpcExtensionContextHandleService;
                _info = info;
            }
            public void Execute()
            {
                _grpcExtensionContextHandleService.GenerateContext(_info);
            }
        }

    }
}
