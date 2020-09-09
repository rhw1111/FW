using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Survey
{
    [Injection(InterfaceType = typeof(ISurveyResponseCollectorEndpointRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class SurveyResponseCollectorEndpointRepositoryCacheProxy : ISurveyResponseCollectorEndpointRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_SurveyResponseCollectorEndpointRepository",
            ExpireSeconds = 600,
            MaxLength = 500
        };


        private ISurveyResponseCollectorEndpointRepository _surveyResponseCollectorEndpointRepository;

        public SurveyResponseCollectorEndpointRepositoryCacheProxy(ISurveyResponseCollectorEndpointRepository surveyResponseCollectorEndpointRepository)
        {
            _surveyResponseCollectorEndpointRepository = surveyResponseCollectorEndpointRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<SurveyEndpoint?> QueryByTypeName(string type, string name, CancellationToken cancellationToken = default)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _surveyResponseCollectorEndpointRepository.QueryByTypeName(type, name, cancellationToken);
                    if (obj == null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                }, name
                )).Item1;
        }
    }
}
