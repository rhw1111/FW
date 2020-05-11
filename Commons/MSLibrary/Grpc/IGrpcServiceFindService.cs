using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// 查找Grpc服务类型的服务
    /// </summary>
    public interface IGrpcServiceFindService
    {
        Task<IList<GrpcTypeData>> Execute();
    }

    public class GrpcTypeData
    {
        public Type NameSpaceType { get; set; }
        public Type ServiceType { get; set; }
    }
}
