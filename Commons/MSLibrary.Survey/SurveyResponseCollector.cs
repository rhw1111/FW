using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Survey.DAL;

namespace MSLibrary.Survey
{
    /// <summary>
    /// Survey响应收集器
    /// EndpointID+SurveyID唯一
    /// </summary>
    public class SurveyResponseCollector : EntityBase<ISurveyResponseCollectorIMP>
    {
        public override IFactory<ISurveyResponseCollectorIMP>? GetIMPFactory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        /// <summary>
        /// Survey标识
        /// </summary>
        public string SurveyID
        {
            get
            {

                return GetAttribute<string>(nameof(SurveyID));
            }
            set
            {
                SetAttribute<string>(nameof(SurveyID), value);
            }
        }

        /// <summary>
        /// 终结点ID
        /// </summary>
        public Guid EndpointID
        {
            get
            {

                return GetAttribute<Guid>(nameof(EndpointID));
            }
            set
            {
                SetAttribute<Guid>(nameof(EndpointID), value);
            }
        }

        public SurveyResponseCollectorEndpoint Endpoint
        {
            get
            {

                return GetAttribute<SurveyResponseCollectorEndpoint>(nameof(Endpoint));
            }
            set
            {
                SetAttribute<SurveyResponseCollectorEndpoint>(nameof(Endpoint), value);
            }
        }

        public DateTime LatestHandleTime
        {
            get
            {
                return GetAttribute<DateTime>("LatestHandleTime");
            }
            set
            {
                SetAttribute<DateTime>("LatestHandleTime", value);
            }
        }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }
    }

    public interface ISurveyResponseCollectorIMP
    {
        Task CollectResponse(SurveyResponseCollector collector,bool inTransaction,Func<string,Task> responseHandle, CancellationToken cancellationToken = default);
        Task CollectResponse(SurveyResponseCollector collector, bool inTransaction,string response, CancellationToken cancellationToken = default);
    }

    public interface ISurveyResponseDataQueryService
    {
        IAsyncEnumerable<string> Query(string endpointConfiguration,string surveyID,DateTime minTime, CancellationToken cancellationToken = default);
    }


    [Injection(InterfaceType = typeof(ISurveyResponseCollectorIMP), Scope = InjectionScope.Transient)]
    public class SurveyResponseCollectorIMP : ISurveyResponseCollectorIMP
    {
        public static int BatchSize { get; set; } = 20;

        private readonly ISurveyResponseLogStore _surveyResponseLogStore;

        public SurveyResponseCollectorIMP(ISurveyResponseLogStore surveyResponseLogStore)
        {
            _surveyResponseLogStore = surveyResponseLogStore;
        }

        public Task CollectResponse(SurveyResponseCollector collector, bool inTransaction, Func<string, Task> responseHandle, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task CollectResponse(SurveyResponseCollector collector, bool inTransaction, string response, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
