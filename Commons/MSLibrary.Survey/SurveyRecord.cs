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
    /// Survey收集器
    /// EndpointID+SurveyID唯一
    /// </summary>
    public class SurveyRecord : EntityBase<ISurveyRecordIMP>
    {
        private static IFactory<ISurveyRecordIMP>? _surveyRecordIMPFactory;

        public static IFactory<ISurveyRecordIMP> SurveyRecordIMPFactory
        {
            set
            {
                _surveyRecordIMPFactory = value;
            }
        }
        public override IFactory<ISurveyRecordIMP>? GetIMPFactory()
        {
            return _surveyRecordIMPFactory;
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
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {

                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
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

        /// <summary>
        /// Survey终结点
        /// </summary>
        public SurveyEndpoint Endpoint
        {
            get
            {

                return GetAttribute<SurveyEndpoint>(nameof(Endpoint));
            }
            set
            {
                SetAttribute<SurveyEndpoint>(nameof(Endpoint), value);
            }
        }

        /// <summary>
        /// 最后接收处理时间
        /// </summary>
        public DateTime LatestResponseHandleTime
        {
            get
            {
                return GetAttribute<DateTime>("LatestResponseHandleTime");
            }
            set
            {
                SetAttribute<DateTime>("LatestResponseHandleTime", value);
            }
        }

        /// <summary>
        /// 绑定信息
        /// </summary>
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
        /// 接收者配置类型
        /// 不同的类型有不同的配置
        /// </summary>
        public string RecipientConfigurationType
        {
            get
            {

                return GetAttribute<string>(nameof(RecipientConfigurationType));
            }
            set
            {
                SetAttribute<string>(nameof(RecipientConfigurationType), value);
            }
        }

        /// <summary>
        /// 接收者配置
        /// </summary>
        public string RecipientConfiguration
        {
            get
            {

                return GetAttribute<string>(nameof(RecipientConfiguration));
            }
            set
            {
                SetAttribute<string>(nameof(RecipientConfiguration), value);
            }
        }

        /// <summary>
        /// 最后一次接收者生成时间
        /// </summary>
        public DateTime? LatestRecipientGenerateTime
        {
            get
            {

                return GetAttribute<DateTime?>(nameof(LatestRecipientGenerateTime));
            }
            set
            {
                SetAttribute<DateTime?>(nameof(LatestRecipientGenerateTime), value);
            }
        }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalInfo
        {
            get
            {

                return GetAttribute<string>(nameof(AdditionalInfo));
            }
            set
            {
                SetAttribute<string>(nameof(AdditionalInfo), value);
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
        public async Task CollectResponse(Func<object, Task> responseHandle, CancellationToken cancellationToken = default)
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
        public async Task CollectResponse(string response, Func<object, Task> responseHandle, CancellationToken cancellationToken = default)
        {
            await _imp.CollectResponse(this, response, responseHandle, cancellationToken);
        }

        /// <summary>
        /// 生成接收者
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task GenerateRecipient(CancellationToken cancellationToken = default)
        {
            await _imp.GenerateRecipient(this, cancellationToken);
        }

        public async Task UpdateRecipientConfiguration(string type, string configuration, CancellationToken cancellationToken = default)
        {
            await _imp.UpdateRecipientConfiguration(this,type,configuration, cancellationToken);
        }

        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }
    }

    public interface ISurveyRecordIMP
    {
        Task UpdateRecipientConfiguration(SurveyRecord record, string type, string configuration, CancellationToken cancellationToken = default);
        /// <summary>
        /// 生成接收者
        /// </summary>
        /// <param name="record"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task GenerateRecipient(SurveyRecord record, CancellationToken cancellationToken = default);
        Task CollectResponse(SurveyRecord collector,Func<object,Task> responseHandle, CancellationToken cancellationToken = default);
        Task CollectResponse(SurveyRecord collector, string response, Func<object, Task> responseHandle, CancellationToken cancellationToken = default);

        Task Update(SurveyRecord collector, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey响应数据查询服务
    /// </summary>
    public interface ISurveyResponseDataQueryService
    {
        IAsyncEnumerable<string> Query(string endpointConfiguration,string surveyID,DateTime minTime, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey接收者生成服务
    /// </summary>
    public interface ISurveyRecipientGenerateService
    {
        Task<string> Generate(string endpointConfiguration, string recipientConfiguration, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Surve收集器可用性检查服务
    /// </summary>
    public interface ISurveyCollectorEnableCheckService
    {
        Task<bool> Check(string endpointConfiguration, string surveyID, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey响应数据的ID解析服务
    /// 需要解析出响应ID，响应产生的时间
    /// </summary>
    public interface ISurveyResponseIDResolveService
    {
        Task<(string,DateTime)> Resolve(string responseData, CancellationToken cancellationToken = default);
        Task<(string, DateTime)> ResolveFromDirect(string responseData, CancellationToken cancellationToken = default);

    }

    /// <summary>
    /// Survey响应数据转换服务
    /// 将原始数据转换成需要的对象
    /// </summary>
    public interface ISurveyResponseConvertService
    {
        Task<object> Convert(string responseData, CancellationToken cancellationToken = default);
        Task<object> ConvertFromDirect(string responseData, CancellationToken cancellationToken = default);
    }


    [Injection(InterfaceType = typeof(ISurveyRecordIMP), Scope = InjectionScope.Transient)]
    public class SurveyRecordIMP : ISurveyRecordIMP
    {
        public static string ErrorLoggerCategoryName { get; set; } = null!;

        public static int BatchSize { get; set; } = 50;

        private readonly ISurveyRecordStore _surveyRecordStore;
        private readonly ISurveyResponseLogStore _surveyResponseLogStore;

        public SurveyRecordIMP(ISurveyRecordStore surveyRecordStore,ISurveyResponseLogStore surveyResponseLogStore)
        {
            _surveyRecordStore = surveyRecordStore;
            _surveyResponseLogStore = surveyResponseLogStore;
        }

        public async Task CollectResponse(SurveyRecord collector, Func<object, Task> responseHandle, CancellationToken cancellationToken = default)
        {
            var responseIDResolveService = SurveyExtensionCollection.GetSurveyResponseIDResolveService(collector.Endpoint.Type);
            var responseDataQueryService = SurveyExtensionCollection.GetSurveyResponseDataQueryService(collector.Endpoint.Type);
            var responseCollectorEnableCheckService = SurveyExtensionCollection.GetSurveyCollectorEnableCheckService(collector.Endpoint.Type);
            var responseConvertService = SurveyExtensionCollection.GetSurveyResponseConvertService(collector.Endpoint.Type);


            //检查收集器是否可用，如果不可用，则直接结束
            var enabled=await responseCollectorEnableCheckService.Check(collector.Endpoint.Configuration, collector.SurveyID, cancellationToken);

            if (!enabled)
            {
                return;
            }

            //从源中获取从上一次处理后的所有响应
            var responseDatas=responseDataQueryService.Query(collector.Endpoint.Configuration, collector.SurveyID, collector.LatestResponseHandleTime, cancellationToken);
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
                                    var realResponseObj=responseConvertService.Convert(data);
                                    //执行响应处理
                                    await responseHandle(realResponseObj);
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

            await _surveyRecordStore.UpdateLatestHandleTime(collector.ID, latestTime, cancellationToken);
        }

        public async Task CollectResponse(SurveyRecord collector, string response, Func<object, Task> responseHandle, CancellationToken cancellationToken = default)
        {
            var responseIDResolveService = SurveyExtensionCollection.GetSurveyResponseIDResolveService(collector.Endpoint.Type);
            var responseDataQueryService= SurveyExtensionCollection.GetSurveyResponseDataQueryService(collector.Endpoint.Type);
            var responseConvertService = SurveyExtensionCollection.GetSurveyResponseConvertService(collector.Endpoint.Type);


            var responseObj =await responseIDResolveService.ResolveFromDirect(response, cancellationToken);

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
                        var realResponseObj = responseConvertService.ConvertFromDirect(response);
                        //执行响应处理
                        await responseHandle(realResponseObj);
                        scope.Complete();
                    }

                }
            }
        }

        public async Task GenerateRecipient(SurveyRecord record, CancellationToken cancellationToken = default)
        {
            var recipientGenerateService = SurveyExtensionCollection.GetSurveyRecordRecipientGenerateService($"{record.Endpoint.Type}-{record.RecipientConfigurationType}");

            await recipientGenerateService.Generate(record.Endpoint.Configuration, record.RecipientConfiguration, cancellationToken);
            await _surveyRecordStore.UpdateLatestRecipientGenerateTime(record.ID, DateTime.UtcNow, cancellationToken);
        }

        public async Task UpdateRecipientConfiguration(SurveyRecord record, string type, string configuration, CancellationToken cancellationToken = default)
        {
            await _surveyRecordStore.UpdateRecipientConfiguration(record.ID, type, configuration, cancellationToken);
        }

        public async Task Update(SurveyRecord record, CancellationToken cancellationToken = default)
        {
            await _surveyRecordStore.Update(record, cancellationToken);
        }
    }
}
