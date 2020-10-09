using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Grpc.Interceptors
{
    public interface IExtensionInfoinject
    {
        Task SetData(object state);
    }
}
