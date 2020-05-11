using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Grpc
{
    [Injection(InterfaceType = typeof(IChannelCredentialsGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class ChannelCredentialsGeneratorRepository:IChannelCredentialsGeneratorRepository
    {
        /// <summary>
        /// 查询指定类型的通道凭证生成器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ChannelCredentialsGenerator> QueryByType(string type)
        {
            return await Task.FromResult(new ChannelCredentialsGenerator()
            {
                ID = Guid.NewGuid(),
                Type = type,
                CreateTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow
            });
        }
    }
}
