using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Cache
{
    [Injection(InterfaceType = typeof(IKVCacheVisitorRepository), Scope = InjectionScope.Singleton)]
    public class KVCacheVisitorRepository : IKVCacheVisitorRepository
    {
        public static IDictionary<string, KVCacheVisitor> Datas { get; } = new Dictionary<string, KVCacheVisitor>();

        public async Task<KVCacheVisitor> QueryByName(string name)
        {
            if (Datas.TryGetValue(name, out KVCacheVisitor result))
            {
                return await Task.FromResult(result);                
            }

            return null;
        }

        public KVCacheVisitor QueryByNameSync(string name)
        {
            if (Datas.TryGetValue(name, out KVCacheVisitor result))
            {
                return result;
            }

            return null;
        }
    }
}
