using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSLibrary.Grpc.Context.Application
{
    public interface IAppGrpcExtensionContextExecute
    {
        Task<IGrpcExtensionContextInit> Do(ServerCallContext callContext, string name);
    }

    public interface IGrpcExtensionContextInit
    {
        void Execute();
    }
}
