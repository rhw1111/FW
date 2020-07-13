﻿using System;
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

        public async Task<SurveyResponseCollectorEndpoint?> QueryByTypeName(string type, string name, CancellationToken cancellationToken = default)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _surveyResponseCollectorEndpointRepository.QueryByTypeName(type,name, cancellationToken);
                }, name
                );
        }
    }
}
