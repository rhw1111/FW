using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// 服务端凭证生成器的仓储
    /// </summary>
    public interface IServerCredentialsGeneratorRepository
    {
        /// <summary>
        /// 查询指定类型的服务端凭证生成器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ServerCredentialsGenerator> QueryByType(string type);
    }
}
