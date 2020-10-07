using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSLibrary.Grpc.Interceptors.Application
{
    /// <summary>
    /// 应用层转换Grpc日志上下文
    /// 将GrpcLogContextData转换成实际业务中的日志对象
    /// </summary>
    public interface IAppGrpcLogContextConvert
    {
        Task<object> Convert(GrpcLogContextData contextData);
    }

}
