using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Survey.SurveyMonkey
{

    [Injection(InterfaceType = typeof(ISurveyMonkeyEndpointRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class SurveyMonkeyEndpointRepositoryCacheProxy : ISurveyMonkeyEndpointRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_SurveyMonkeyEndpointRepository",
            ExpireSeconds = 600,
            MaxLength = 500
        };


        private ISurveyMonkeyEndpointRepository _surveyMonkeyEndpointRepository;

        public SurveyMonkeyEndpointRepositoryCacheProxy(ISurveyMonkeyEndpointRepository surveyMonkeyEndpointRepository)
        {
            _surveyMonkeyEndpointRepository = surveyMonkeyEndpointRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<SurveyMonkeyEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _surveyMonkeyEndpointRepository.QueryByName(name, cancellationToken);
                }, name
                );
        }
    }
}
