using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;

namespace MSLibrary.Grpc.Context
{
    public interface IGrpcExtensionContextHandleServiceFactorySelector:ISelector<IFactory<IGrpcExtensionContextHandleService>>
    {
    }
}
