using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Survey.DAL;
using MSLibrary.Transaction;
using MSLibrary.Thread;
using MSLibrary.Logger;

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
        /// 所属组
        /// </summary>
        public string Group
        {
            get
            {

                return GetAttribute<string>(nameof(Group));
            }
            set
            {
                SetAttribute<string>(nameof(Group), value);
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

        public string BindingInfo
        {
            get
            {
                return GetAttribute<string>("BindingInfo");
            }
            set
            {
                SetAttribute<string>("BindingInfo", value);
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

        /// <summary>
        /// 收集响应（批量）
        /// </summary>
        /// <param name="responseHandle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CollectResponse(Func<string, Task> responseHandle, CancellationToken cancellationToken = default)
        {
            await _imp.CollectResponse(this, responseHandle, cancellationToken);
        }
        /// <summary>
        /// 收集响应（单个）
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseHandle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CollectResponse(string response, Func<string, Task> responseHandle, CancellationToken cancellationToken = default)
        {
            await _imp.CollectResponse(this, response, responseHandle, cancellationToken);
        }
    }

    public interface ISurveyResponseCollectorIMP
    {
        Task CollectResponse(SurveyResponseCollector collector,Func<string,Task> responseHandle, CancellationToken cancellationToken = default);
        Task CollectResponse(SurveyResponseCollector collector, string response, Func<string, Task> responseHandle, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey响应数据查询服务
    /// </summary>
    public interface ISurveyResponseDataQueryService
    {
        IAsyncEnumerable<string> Query(string endpointConfiguration,string surveyID,DateTime minTime, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey响应数据的ID解析服务
    /// 需要解析出响应ID，响应产生的时间
    /// </summary>
    public interface ISurveyResponseIDResolveService
    {
        Task<(string,DateTime)> Resolve(string responseData, CancellationToken cancellationToken = default);
    }


    [Injection(InterfaceType = typeof(ISurveyResponseCollectorIMP), Scope = InjectionScope.Transient)]
    public class SurveyResponseCollectorIMP : ISurveyResponseCollectorIMP
    {
        public static string ErrorLoggerCategoryName { get; set; } = null!;

        public static int BatchSize { get; set; } = 50;

        private readonly ISurveyResponseCollectorStore _surveyResponseCollectorStore;
        private readonly ISurveyResponseLogStore _surveyResponseLogStore;

        public SurveyResponseCollectorIMP(ISurveyResponseCollectorStore surveyResponseCollectorStore,ISurveyResponseLogStore surveyResponseLogStore)
        {
            _surveyResponseCollectorStore = surveyResponseCollectorStore;
            _surveyResponseLogStore = surveyResponseLogStore;
        }

        public async Task CollectResponse(SurveyResponseCollector collector, Func<string, Task> responseHandle, CancellationToken cancellationToken = default)
        {
            var responseIDResolveService = SurveyResponseExtensionCollection.GetSurveyResponseIDResolveService(collector.Endpoint.Type);
            var responseDataQueryService = SurveyResponseExtensionCollection.GetSurveyResponseDataQueryService(collector.Endpoint.Type);

            //从源中获取从上一次处理后的所有响应
            var responseDatas=responseDataQueryService.Query(collector.Endpoint.Configuration, collector.SurveyID, collector.LatestHandleTime, cancellationToken);
            DateTime latestTime=DateTime.MinValue;
            DateTime? errorLatestTime = null;
            object lockObj=new object();
            int executeCount = 0;

            await ParallelHelper.ForEach(responseDatas, 10,
                async(data)=>
                {
                    var responseObj = await responseIDResolveService.Resolve(data, cancellationToken);

                    try
                    {
                        var responseLog = await _surveyResponseLogStore.Query(collector.Endpoint.Type, collector.SurveyID, responseObj.Item1, cancellationToken);
                        if (responseLog != null)
                        {
                            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
                            {
                                var newLog = new SurveyResponseLog()
                                {
                                    ID = Guid.NewGuid(),
                                    SurveyType = collector.Endpoint.Type,
                                    SurveyID = collector.SurveyID,
                                    ResponseID = responseObj.Item1,
                                    CreateTime = DateTime.UtcNow,
                                    ModifyTime = DateTime.UtcNow
                                };

                                await _surveyResponseLogStore.Add(newLog, cancellationToken);

                                var canContinue = true;
                                //检查是否有重复的
                                var newId = await _surveyResponseLogStore.QueryNoLock(collector.Endpoint.Type, collector.SurveyID, newLog.ResponseID, cancellationToken);
                                if (newLog.ID != newId)
                                {
                                    canContinue = false;
                                }

                                if (canContinue)
                                {
                                    //执行响应处理
                                    await responseHandle(data);
                                    scope.Complete();
                                }

                            }
                        }

                        lock(lockObj)
                        {
                            executeCount++;
                            if (responseObj.Item2> latestTime)
                            {
                                latestTime = responseObj.Item2;
                            }

                            if (errorLatestTime!=null && errorLatestTime.Value< latestTime)
                            {
                                latestTime = errorLatestTime.Value;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        LoggerHelper.LogError(ErrorLoggerCategoryName, ex.ToStackTraceString());
                        lock (lockObj)
                        {
                            if (errorLatestTime== null)
                            {
                                errorLatestTime = responseObj.Item2;
                            }
                            else if (errorLatestTime.Value> responseObj.Item2)
                            {
                                errorLatestTime = responseObj.Item2;
                            }

                            latestTime = errorLatestTime.Value;
                        }
                    }
                }
                );

            await _surveyResponseCollectorStore.UpdateLatestHandleTime(collector.ID, latestTime, cancellationToken);
        }

        public async Task CollectResponse(SurveyResponseCollector collector, string response, Func<string, Task> responseHandle, CancellationToken cancellationToken = default)
        {
            var responseIDResolveService = SurveyResponseExtensionCollection.GetSurveyResponseIDResolveService(collector.Endpoint.Type);
            var responseDataQueryService= SurveyResponseExtensionCollection.GetSurveyResponseDataQueryService(collector.Endpoint.Type);

            var responseObj=await responseIDResolveService.Resolve(response, cancellationToken);

            var responseLog=await _surveyResponseLogStore.Query(collector.Endpoint.Type, collector.SurveyID, responseObj.Item1, cancellationToken);
            if (responseLog!=null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
                {
                    var newLog = new SurveyResponseLog()
                    {
                        ID=Guid.NewGuid(),
                        SurveyType = collector.Endpoint.Type,
                        SurveyID = collector.SurveyID,
                        ResponseID = responseObj.Item1,
                        CreateTime = DateTime.UtcNow,
                        ModifyTime = DateTime.UtcNow
                    };

                    await _surveyResponseLogStore.Add(newLog, cancellationToken);

                    var canContinue = true;
                    //检查是否有重复的
                    var newId=await _surveyResponseLogStore.QueryNoLock(collector.Endpoint.Type, collector.SurveyID, newLog.ResponseID, cancellationToken);
                    if (newLog.ID != newId)
                    {
                        canContinue = false;
                    }   
                    
                    if (canContinue)
                    {
                        //执行响应处理
                        await responseHandle(response);
                        scope.Complete();
                    }

                }
            }
        }
    }
}
