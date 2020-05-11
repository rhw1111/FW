using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Cache
{
    [Injection(InterfaceType = typeof(IKVCacheVisitorRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class KVCacheVisitorRepositoryCacheProxy : IKVCacheVisitorRepositoryCacheProxy
    {
        public static int MaxLength { get; set; } = 500;
        public static int ExpireSeconds { get; set; } = 600;

        private KVCacheVisitor _timeoutVisitor = new KVCacheVisitor()
        {
            Name = "_KVCacheVisitorRepository",
            CacheType = KVCacheTypes.LocalTimeout,
            CacheConfiguration = string.Format(@"{{
                        ""MaxLength"":{0},
                        ""ExpireSeconds"":{1}
              }}", MaxLength.ToString(), ExpireSeconds.ToString())
        };
        private IKVCacheVisitorRepository _kvCacheVisitorRepository;

        public KVCacheVisitorRepositoryCacheProxy(IKVCacheVisitorRepository kvCacheVisitorRepository)
        {
            _kvCacheVisitorRepository = kvCacheVisitorRepository;
        }


        public async Task<KVCacheVisitor> QueryByName(string name)
        {
            return await _timeoutVisitor.Get<string, KVCacheVisitor>(async (k) =>
            {
                return await _kvCacheVisitorRepository.QueryByName(name);
            }, name);
        }

        public  KVCacheVisitor QueryByNameSync(string name)
        {
            return _timeoutVisitor.GetSync<string, KVCacheVisitor>((k) =>
            {
                return _kvCacheVisitorRepository.QueryByNameSync(name);
            }, name);
        }
    }
}
