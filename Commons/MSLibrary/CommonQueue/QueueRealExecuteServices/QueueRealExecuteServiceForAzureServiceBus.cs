using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Transactions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Logger;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.From;
using MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.To;
using Microsoft.Extensions.Primitives;
using System.Runtime.InteropServices.ComTypes;

namespace MSLibrary.CommonQueue.QueueRealExecuteServices
{
    /// <summary>
    /// 针对Azure服务总线的队列处理服务
    /// </summary>
    [Injection(InterfaceType = typeof(QueueRealExecuteServiceForAzureServiceBus), Scope = InjectionScope.Singleton)]
    public class QueueRealExecuteServiceForAzureServiceBus : IQueueRealExecuteService
    {
        private static Dictionary<string, Dictionary<int, TopicClient>> _productClients = new Dictionary<string, Dictionary<int, TopicClient>>();

        private static Dictionary<string, IFactory<IAzureServiceBusMessageConvertFrom>> _convertFromServiceFactories = new Dictionary<string, IFactory<IAzureServiceBusMessageConvertFrom>>();
        private static Dictionary<string, IFactory<IAzureServiceBusMessageConvertTo>> _convertToServiceFactories = new Dictionary<string, IFactory<IAzureServiceBusMessageConvertTo>>();

        public static Dictionary<string, IFactory<IAzureServiceBusMessageConvertFrom>> ConvertFromServiceFactories
        {
            get
            {
                return _convertFromServiceFactories;
            }
        }

        public static Dictionary<string, IFactory<IAzureServiceBusMessageConvertTo>> ConvertToServiceFactories
        {
            get
            {
                return _convertToServiceFactories;
            }
        }


    

        public QueueRealExecuteServiceForAzureServiceBus( )
        {
         
        }

        public static string ConsumeErrorLoggerCategoryName { get; set; }

        public async Task<ICommonQueueEndpointConsumeController> Consume(CommonQueueConsumeEndpoint endpoint, string configuration, Func<CommonMessage, Task<MessageHandleResult>> messageHandle)
        {
            var consumeConfiguration = JsonSerializerHelper.Deserialize<ConsumeQueueRealExecuteServiceForAzureServiceBusConfiguration>(configuration);

            List<SubscriptionClient> clients = new List<SubscriptionClient>();
            List<MessageReceiver> dlqReceivers = new List<MessageReceiver>();

            Dictionary<string, DateTime> loggerDatetimes = new Dictionary<string, DateTime>();

            try
            {
                foreach (var item in consumeConfiguration.Items)
                {
                    var newClient = new SubscriptionClient(consumeConfiguration.ConnectionString, item.Topic, consumeConfiguration.Subscription);

                    //EntityNameHelper.FormatDeadLetterPath

                    var tempItem = item;
                    loggerDatetimes[item.Topic] = DateTime.UtcNow;
                    var messageHandlerOptions = new MessageHandlerOptions(async (args) =>
                    {

                        if ((DateTime.UtcNow - loggerDatetimes[tempItem.Topic]).TotalSeconds > 300)
                        {
                            loggerDatetimes[tempItem.Topic] = DateTime.UtcNow;
                            LoggerHelper.LogError(ConsumeErrorLoggerCategoryName, $"Message:{args.Exception.Message},stack:{args.Exception.StackTrace},Endpoint: {args.ExceptionReceivedContext.Endpoint},Entity Path: {args.ExceptionReceivedContext.EntityPath},Executing Action: {args.ExceptionReceivedContext.Action}");
                        }

                        await Task.CompletedTask;
                    })
                    {
                        MaxConcurrentCalls = 1,
                        MaxAutoRenewDuration = new TimeSpan(2, 0, 0),
                        AutoComplete = false
                    };

                    //注册执行消息
                    newClient.RegisterMessageHandler(async (azureMessage, cancellation) =>
                    {


                        var fromService = getAzureServiceBusMessageConvertFromService(tempItem.ConvertFromServiceName);
                        var message = await fromService.From(azureMessage);
                        if (message != null)
                        {

                        //如果执行出错，根据MaxRetry重试，达到最大重试值后，延迟执行
                        MessageHandleResult? handleResult = null;
                            int retry = 0;
                            while (true)
                            {
                                try
                                {
                                    handleResult = await messageHandle(message);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    if (ex is UtilityException || retry >= consumeConfiguration.MaxRetry)
                                    {

                                        var strError = ex.ToStackTraceString();


                                        if (strError.Length > 5000)
                                        {
                                            strError = strError.Remove(4999, strError.Length - 5000);
                                        }


                                    //移到死信队列
                                    //await newClient.DeadLetterAsync(azureMessage.SystemProperties.LockToken, strError);

                                    //延迟执行
                                    await delayMessage(azureMessage, newClient, strError, true, tempItem.Topic);
                                        break;
                                    }
                                    else
                                    {
                                        retry++;
                                        await Task.Delay(100);
                                    }
                                }
                            }


                            if (handleResult == MessageHandleResult.Complete)
                            {
                                await newClient.CompleteAsync(azureMessage.SystemProperties.LockToken);
                            }
                            else
                            {
                                await delayMessage(azureMessage, newClient, string.Empty, false, tempItem.Topic);
                            }

                        }
                        else
                        {
                            await newClient.CompleteAsync(azureMessage.SystemProperties.LockToken);
                        }



                    }, messageHandlerOptions);
                    clients.Add(newClient);

                    //处理死信队列
                    //遇到死信消息，延迟一分钟重新送回Topic
                    var dlqReceiver = new MessageReceiver(consumeConfiguration.ConnectionString, EntityNameHelper.FormatDeadLetterPath($"{item.Topic}/Subscriptions/{consumeConfiguration.Subscription}"), ReceiveMode.PeekLock);

                    dlqReceiver.RegisterMessageHandler(
                            async (message, cancellationToken1) =>
                            {

                                var resubmitMessage = message.Clone();
                                resubmitMessage.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(180);
                                var topicClient = new TopicClient(consumeConfiguration.ConnectionString, item.Topic, null);

                                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                                {
                                    await topicClient.SendAsync(resubmitMessage);
                                    await dlqReceiver.CompleteAsync(message.SystemProperties.LockToken);

                                    ts.Complete();
                                }
                            },
                            messageHandlerOptions
                            );

                    dlqReceivers.Add(dlqReceiver);
                }
            }
            catch
            {
                List<Task> tasks = new List<Task>();

                foreach (var item in clients)
                {
                    tasks.Add(item.CloseAsync());
                }

                foreach (var item in dlqReceivers)
                {
                    tasks.Add(item.CloseAsync());
                }

                foreach (var item in tasks)
                {
                    await item;
                }

                throw;
            }

            CommonQueueEndpointConsumeControllerForServiceBus controller = new CommonQueueEndpointConsumeControllerForServiceBus(clients, dlqReceivers);

            return await Task.FromResult(controller);
        }

        public async Task Product(CommonQueueProductEndpoint endpint, string configuration, CommonMessage message)
        {
            var productConfiguration = JsonSerializerHelper.Deserialize<ProductQueueRealExecuteServiceForAzureServiceBusConfiguration>(configuration);
            if (!_productClients.TryGetValue(endpint.Name,out Dictionary<int, TopicClient> clients))
            {
                lock(_productClients)
                {
                    if (!_productClients.TryGetValue(endpint.Name, out clients))
                    {
                        clients = new Dictionary<int, TopicClient>();
                        foreach (var item in productConfiguration.Items)
                        {
                            foreach (var topicItem in item.Value)
                            {
                                clients.Add(topicItem.Code, new TopicClient(item.Key, topicItem.TopicItem));
                            }
                        }
                        _productClients[endpint.Name] = clients;
                    }
                }
            }

            var mod = message.Key.ToInt() % clients.Count;
            var client= clients[mod];
           

            var toService=getAzureServiceBusMessageConvertToService(productConfiguration.ConvertToServiceName);

            var azureMessage=await toService.To(message);
            await client.SendAsync(azureMessage);
        }


        private IAzureServiceBusMessageConvertTo getAzureServiceBusMessageConvertToService(string name)
        {
            if (!_convertToServiceFactories.TryGetValue(name,out IFactory<IAzureServiceBusMessageConvertTo> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAzureServiceBusMessageConvertToServiceByName,
                    DefaultFormatting = "找不到名称为{0}的Azure服务总线From转换服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name,$"{this.GetType().FullName}.ConvertToServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundAzureServiceBusMessageConvertToServiceByName, fragment);
            }
            return serviceFactory.Create();
        }
               
        private IAzureServiceBusMessageConvertFrom getAzureServiceBusMessageConvertFromService(string name)
        {
                                                                             
            if (!_convertFromServiceFactories.TryGetValue(name, out IFactory<IAzureServiceBusMessageConvertFrom> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAzureServiceBusMessageConvertFromServiceByName,
                    DefaultFormatting = "找不到名称为{0}的Azure服务总线From转换服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ConvertFromServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundAzureServiceBusMessageConvertFromServiceByName, fragment);
            }
            return serviceFactory.Create();
        }


        private async Task delayMessage(Message azureMessage,SubscriptionClient client,string strError,bool exceptionRetry, string topic)
        {
            var newAzureMessage = new Message(azureMessage.Body);
            newAzureMessage.UserProperties["Exception"] = strError.ToString();
            newAzureMessage.UserProperties["ExceptionRetry"] = exceptionRetry;
            newAzureMessage.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(60);


            var topicClient = new TopicClient(client.ServiceBusConnection, topic, null);

            using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
              
                await client.CompleteAsync(azureMessage.SystemProperties.LockToken);
                await topicClient.SendAsync(newAzureMessage).ConfigureAwait(false);

                ts.Complete();
            }
        }

    }

    [DataContract]
    public class ConsumeQueueRealExecuteServiceForAzureServiceBusConfiguration
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        [DataMember]
        public string ConnectionString { get; set; }
        /// <summary>
        /// 订阅
        /// </summary>
        [DataMember]
        public string Subscription { get; set; }
        /// <summary>
        /// 当发生未知异常时的最大重试次数
        /// </summary>
        public int MaxRetry { get; set; } = 1;
        /// <summary>
        /// 配置项列表
        /// </summary>
        [DataMember]
        public List<ConsumeQueueRealExecuteServiceForAzureServiceBusConfigurationItem> Items { get; set; }
    }

    [DataContract]
    public class ConsumeQueueRealExecuteServiceForAzureServiceBusConfigurationItem
    {
        /// <summary>
        /// 从原始消息转换为通用消息的服务名称，对应QueueRealExecuteServiceForAzureServiceBus.ConvertFromServiceFactories的键
        /// </summary>
        [DataMember]
        public string ConvertFromServiceName { get; set; }
        /// <summary>
        /// 队列主题
        /// </summary>
        [DataMember]
        public string Topic { get; set; }
    }


    [DataContract]
    public class ProductQueueRealExecuteServiceForAzureServiceBusConfiguration 
    {
        [DataMember]
        public string ConvertToServiceName { get; set; }
        [DataMember]
        public Dictionary<string, List<ProductQueueRealExecuteServiceForAzureServiceBusConfigurationItem>> Items { get; set; } 
    }


    public class ProductQueueRealExecuteServiceForAzureServiceBusConfigurationItem
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string TopicItem { get; set; }
    }


    public class CommonQueueEndpointConsumeControllerForServiceBus : ICommonQueueEndpointConsumeController
    {
        private List<SubscriptionClient> _clients;
        private List<MessageReceiver> _dlqReceivers;

        public CommonQueueEndpointConsumeControllerForServiceBus(List<SubscriptionClient> clients, List<MessageReceiver> dlqReceivers)
        {
            _clients = clients;
            _dlqReceivers = dlqReceivers;
        }

        public async Task Stop()
        {
            List<Task> tasks = new List<Task>();

            foreach(var item in _clients)
            {
                tasks.Add(item.CloseAsync());
            }

            foreach (var item in _dlqReceivers)
            {
                tasks.Add(item.CloseAsync());
            }

            foreach (var item in tasks)
            {
                await item;
            }
        }
    }
}
