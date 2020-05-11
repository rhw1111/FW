using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Grpc.Filters;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// 查找指定Grpc服务上的过滤器的服务
    /// </summary>
    public interface IGrpcServiceFindFilterService
    {
        Task<IList<FilterBase>> Execute(Type serviceType);
    }
}
