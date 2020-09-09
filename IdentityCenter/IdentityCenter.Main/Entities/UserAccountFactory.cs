using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace IdentityCenter.Main.Entities
{
    [Injection(InterfaceType = typeof(IUserAccountFactory), Scope = InjectionScope.Singleton)]
    public class UserAccountFactory : IUserAccountFactory
    {
        public async Task<UserAccount> Create(string serializeData)
        {
            return await Task.FromResult(JsonSerializerHelper.Deserialize<UserAccount>(serializeData));
        }
    }
}
