using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Grpc
{
    [Injection(InterfaceType = typeof(IServerCredentialsGeneratorRepository), Scope = InjectionScope.Singleton)]
    public class ServerCredentialsGeneratorRepository:IServerCredentialsGeneratorRepository
    {
        /// <summary>
        /// 查询指定类型的服务端凭证生成器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ServerCredentialsGenerator> QueryByType(string type)
        {
            return await Task.FromResult(new ServerCredentialsGenerator()
            {
                ID = Guid.NewGuid(),
                Type = type,
                CreateTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow
            });
        }
    }
}
