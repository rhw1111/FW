using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Grpc.Interceptors.Application
{
    public interface IAppClientExtensionInfoGenerate
    {
       IDictionary<string, string> Do(string name, object state);
    }
}
