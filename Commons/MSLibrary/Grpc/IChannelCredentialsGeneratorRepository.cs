using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Grpc
{
    public interface IChannelCredentialsGeneratorRepository
    {
        /// <summary>
        /// 查询指定类型的通道凭证生成器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ChannelCredentialsGenerator> QueryByType(string type);
    }
}
