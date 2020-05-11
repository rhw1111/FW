using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm;
using MSLibrary.Xrm.Message.RetrieveSignleAttribute;
using MSLibrary.Xrm.Message.RetrieveMultipleFetch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MSLibrary.MessageQueue.DAL.SMessageStores
{
    /// <summary>
    /// 基于D365实体记录实现的消息数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(SMessageStoreForD365), Scope = InjectionScope.Singleton)]
    public class SMessageStoreForD365 : ISMessageStore
    {
        private const string _queueName = "Queue";

        private ICrmServiceFactoryRepositoryCacheProxy _crmServiceFactoryRepositoryCacheProxy;

        public SMessageStoreForD365(ICrmServiceFactoryRepositoryCacheProxy crmServiceFactoryRepositoryCacheProxy)
        {
            _crmServiceFactoryRepositoryCacheProxy = crmServiceFactoryRepositoryCacheProxy;
        }
        public async Task Add(SQueue queue, SMessage message)
        {
            var crmService=await getCrmService(queue.ServerName);
            CrmExecuteEntity entity = new CrmExecuteEntity(queue.Name, message.ID)
            {
                 IsActivity=false,
                 Attributes=new Dictionary<string, object>()
                 {
                     { "ms_name",message.Key},
                     { "ms_type",message.Type},
                     { "ms_data",message.Data},
                     { "ms_delaymessageid",message.DelayMessageID?.ToString()},
                     { "ms_exceptionmessage",message.ExceptionMessage},
                     { "ms_expectationexecutetime",message.ExpectationExecuteTime},
                     { "ms_extensionmessage",message.ExtensionMessage},
                      { "ms_lastexecutetime",message.LastExecuteTime},
                      { "ms_retrynumber",message.RetryNumber},
                      { "ms_isdead",message.IsDead},
                 }
            };

            var newID=await crmService.Create(entity);

            message.ID = newID;
        }

        public async Task AddRetry(SQueue queue, Guid id, string exceptionMessage)
        {
            var crmService = await getCrmService(queue.ServerName);

            CrmRetrieveSignleAttributeRequestMessage request = new CrmRetrieveSignleAttributeRequestMessage()
            {
                 EntityName= queue.Name,
                  EntityId=id,
                   AttributeName= "ms_retrynumber"
            };
            var response=(CrmRetrieveSignleAttributeResponseMessage)await crmService.Execute(request);

            var objRetry=response.Value.ToObject<int?>();
            int retry = 0;
            if (objRetry.HasValue)
            {
                retry = objRetry.Value;
            }

            CrmExecuteEntity entity = new CrmExecuteEntity(queue.Name, id)
            {
                IsActivity = false,
                Attributes = new Dictionary<string, object>()
                 {
   
                      { "ms_retrynumber",retry++},
                 }
            };
            await crmService.Update(entity);       
        }

        public async Task AddToDead(SQueue queue, SMessage message)
        {
            if (!queue.IsDead)
            {
                throw new Exception(string.Format("SQueue {0}.{1} is not dead, can't be used in SMessageStoreForSQLDB.AddToDead", queue.GroupName, queue.Name));
            }

            var crmService = await getCrmService(queue.ServerName);
            CrmExecuteEntity entity = new CrmExecuteEntity(queue.Name, message.ID)
            {
                IsActivity = false,
                Attributes = new Dictionary<string, object>()
                 {
                     { "ms_name",message.Key},
                     { "ms_type",message.Type},
                     { "ms_data",message.Data},
                     { "ms_delaymessageid",message.DelayMessageID?.ToString()},
                     { "ms_exceptionmessage",message.ExceptionMessage},
                     { "ms_expectationexecutetime",message.ExpectationExecuteTime},
                     { "ms_extensionmessage",message.ExtensionMessage},
                      { "ms_lastexecutetime",null},
                      { "ms_retrynumber",0},
                      { "ms_isdead",true},
                 }
            };

            var newID = await crmService.Create(entity);

            message.ID = newID;

        }

        public async Task Delete(SQueue queue, Guid id)
        {
            var crmService = await getCrmService(queue.ServerName);
            await crmService.Delete(queue.Name,id);
        }

        public async Task QueryAllByQueue(SQueue queue, int pageSize, Func<List<SMessage>, Task<bool>> callBack)
        {
            var crmService = await getCrmService(queue.ServerName);
            string strFetch = @"<fetch mapping=""logical"" version=""1.0"" distinct=""false"" page=""{1}"" count=""{2}"" output-format=""xml-platform"">
                                    <entity name=""{0}"">
                                          <attribute name=""{0}id"" />
                                          <attribute name=""ms_name"" />
                                          <attribute name=""ms_type"" />
                                          <attribute name=""ms_data"" />
                                          <attribute name=""ms_delaymessageid"" />
                                          <attribute name=""ms_exceptionmessage"" />
                                          <attribute name=""ms_expectationexecutetime"" />
                                          <attribute name=""ms_extensionmessage"" />
                                          <attribute name=""ms_lastexecutetime"" />
                                          <attribute name=""ms_retrynumber"" />
                                          <attribute name=""ms_isdead"" />
                                          <attribute name=""createdon"" />
                                          <attribute name=""modifiedon"" />
                                    <order descending=""false"" attribute=""createdon"" />
                                    </entity>
                               </fetch>";

            int index = 0;
            int currentSize = pageSize;
            while(currentSize== pageSize)
            {
                CrmRetrieveMultipleFetchRequestMessage request = new CrmRetrieveMultipleFetchRequestMessage()
                {
                    FetchXml = XDocument.Parse(string.Format(strFetch, queue.Name, index++, pageSize)),
                    EntityName = queue.Name                     
                };

                var response = (CrmRetrieveMultipleFetchResponseMessage)await crmService.Execute(request);

                currentSize=response.Value.Results.Count;

                List<SMessage> messages = new List<SMessage>();

                foreach(var item in response.Value.Results)
                {
                    var isDead=await item.GetAttributeValue<bool?>("ms_isdead");
                    var expectationExecuteTime = await item.GetAttributeValue<DateTime?>("ms_expectationexecutetime");
                    var retry = await item.GetAttributeValue<int?>("ms_retrynumber");
                    var delayMessageid = await item.GetAttributeValue<string>("ms_delaymessageid");
                    Guid? delayMessageidObj = null;
                    if (!string.IsNullOrEmpty(delayMessageid))
                    {
                        delayMessageidObj = Guid.Parse(delayMessageid);
                    }


                    var message = new SMessage()
                    {
                        ID = item.Id,
                        CreateTime = (await item.GetAttributeValue<DateTime?>("createdon")).Value,
                        Key = await item.GetAttributeValue<string>("ms_name"),
                        Type = await item.GetAttributeValue<string>("ms_type"),
                        Data = await item.GetAttributeValue<string>("ms_data"),
                        IsDead = isDead != null ? isDead.Value : false,
                        DelayMessageID = delayMessageidObj,
                        ExceptionMessage = await item.GetAttributeValue<string>("ms_exceptionmessage"),
                        ExpectationExecuteTime = expectationExecuteTime != null ? expectationExecuteTime.Value : DateTime.MinValue,
                        ExtensionMessage = await item.GetAttributeValue<string>("ms_extensionmessage"),
                        LastExecuteTime = await item.GetAttributeValue<DateTime?>("ms_lastexecutetime"),
                        RetryNumber = retry != null ? retry.Value : 0
                    };
                    message.Extensions[_queueName] = queue;
                    messages.Add(message);

                    var executeResult = await callBack(messages);
                    if (!executeResult)
                    {
                        break;
                    }
                }

                if (currentSize!=pageSize)
                {
                    break;
                }
            }

        }

        public async Task<SMessage> QueryByDelayID(SQueue queue, Guid delayMessageID)
        {
            var crmService = await getCrmService(queue.ServerName);
            string strFetch = string.Format(@"<fetch mapping=""logical"" version=""1.0"" distinct=""false"" count=""1""  output-format=""xml-platform"">
                                    <entity name=""{0}"">
                                          <attribute name=""{0}id"" />
                                          <attribute name=""ms_name"" />
                                          <attribute name=""ms_type"" />
                                          <attribute name=""ms_data"" />
                                          <attribute name=""ms_delaymessageid"" />
                                          <attribute name=""ms_exceptionmessage"" />
                                          <attribute name=""ms_expectationexecutetime"" />
                                          <attribute name=""ms_extensionmessage"" />
                                          <attribute name=""ms_lastexecutetime"" />
                                          <attribute name=""ms_retrynumber"" />
                                          <attribute name=""ms_isdead"" />
                                          <attribute name=""createdon"" />
                                          <attribute name=""modifiedon"" />
                                        <order descending=""false"" attribute=""createdon"" />
                                        <filter type=""and"">
                                            <condition attribute=""ms_delaymessageid"" operator=""eq"" value=""{1}"" />
                                        </filter>
                                       </entity>
                               </fetch>", queue.Name, delayMessageID.ToString());

            CrmRetrieveMultipleFetchRequestMessage request = new CrmRetrieveMultipleFetchRequestMessage()
            {
                FetchXml = XDocument.Parse(strFetch),
                EntityName = queue.Name
            };

            var response = (CrmRetrieveMultipleFetchResponseMessage)await crmService.Execute(request);

            if (response.Value.Results.Count==0)
            {
                return null;
            }
            var entity = response.Value.Results[0];
            var isDead = await entity.GetAttributeValue<bool?>("ms_isdead");
            var expectationExecuteTime = await entity.GetAttributeValue<DateTime?>("ms_expectationexecutetime");
            var retry = await entity.GetAttributeValue<int?>("ms_retrynumber");
            var delayMessageid = await entity.GetAttributeValue<string>("ms_delaymessageid");
            Guid? delayMessageidObj = null;
            if (!string.IsNullOrEmpty(delayMessageid))
            {
                delayMessageidObj = Guid.Parse(delayMessageid);
            }

            var message= new SMessage()
            {
                ID = entity.Id,
                CreateTime = (await entity.GetAttributeValue<DateTime?>("createdon")).Value,
                Key = await entity.GetAttributeValue<string>("ms_name"),
                Type = await entity.GetAttributeValue<string>("ms_type"),
                Data = await entity.GetAttributeValue<string>("ms_data"),
                IsDead = isDead != null ? isDead.Value : false,
                DelayMessageID = delayMessageidObj,
                ExceptionMessage = await entity.GetAttributeValue<string>("ms_exceptionmessage"),
                ExpectationExecuteTime = expectationExecuteTime != null ? expectationExecuteTime.Value : DateTime.MinValue,
                ExtensionMessage = await entity.GetAttributeValue<string>("ms_extensionmessage"),
                LastExecuteTime = await entity.GetAttributeValue<DateTime?>("ms_lastexecutetime"),
                RetryNumber = retry != null ? retry.Value : 0
            };
            message.Extensions[_queueName] = queue;
            return message;
        }

        public async Task<SMessage> QueryByKeyAndBeforeExpectTime(SQueue queue, string key, DateTime expectTime)
        {
            var crmService = await getCrmService(queue.ServerName);
            string strFetch = string.Format(@"<fetch mapping=""logical"" version=""1.0"" distinct=""false"" count=""1""  output-format=""xml-platform"">
                                    <entity name=""{0}"">
                                          <attribute name=""{0}id"" />
                                          <attribute name=""ms_name"" />
                                          <attribute name=""ms_type"" />
                                          <attribute name=""ms_data"" />
                                          <attribute name=""ms_delaymessageid"" />
                                          <attribute name=""ms_exceptionmessage"" />
                                          <attribute name=""ms_expectationexecutetime"" />
                                          <attribute name=""ms_extensionmessage"" />
                                          <attribute name=""ms_lastexecutetime"" />
                                          <attribute name=""ms_retrynumber"" />
                                          <attribute name=""ms_isdead"" />
                                          <attribute name=""createdon"" />
                                          <attribute name=""modifiedon"" />
                                        <order descending=""false"" attribute=""createdon"" />
                                        <filter type=""and"">
                                            <condition attribute=""ms_name"" operator=""eq"" value=""{1}"" />
                                            <condition attribute=""ms_expectationexecutetime"" operator=""lt"" value=""{2}"" />
                                        </filter>
                                       </entity>
                               </fetch>", queue.Name, key.ToXML(),expectTime.ToString("yyyy-MM-ddT hh:mm:ss.ffffZ"));

            CrmRetrieveMultipleFetchRequestMessage request = new CrmRetrieveMultipleFetchRequestMessage()
            {
                FetchXml = XDocument.Parse(strFetch),
                EntityName = queue.Name
            };

            var response = (CrmRetrieveMultipleFetchResponseMessage)await crmService.Execute(request);

            if (response.Value.Results.Count == 0)
            {
                return null;
            }
            var entity = response.Value.Results[0];
            var isDead = await entity.GetAttributeValue<bool?>("ms_isdead");
            var expectationExecuteTime = await entity.GetAttributeValue<DateTime?>("ms_expectationexecutetime");
            var retry = await entity.GetAttributeValue<int?>("ms_retrynumber");
            var delayMessageid = await entity.GetAttributeValue<string>("ms_delaymessageid");
            Guid? delayMessageidObj = null;
            if (!string.IsNullOrEmpty(delayMessageid))
            {
                delayMessageidObj = Guid.Parse(delayMessageid);
            }

            var message= new SMessage()
            {
                ID = entity.Id,
                CreateTime = (await entity.GetAttributeValue<DateTime?>("createdon")).Value,
                Key = await entity.GetAttributeValue<string>("ms_name"),
                Type = await entity.GetAttributeValue<string>("ms_type"),
                Data = await entity.GetAttributeValue<string>("ms_data"),
                IsDead = isDead != null ? isDead.Value : false,
                DelayMessageID = delayMessageidObj,
                ExceptionMessage = await entity.GetAttributeValue<string>("ms_exceptionmessage"),
                ExpectationExecuteTime = expectationExecuteTime != null ? expectationExecuteTime.Value : DateTime.MinValue,
                ExtensionMessage = await entity.GetAttributeValue<string>("ms_extensionmessage"),
                LastExecuteTime = await entity.GetAttributeValue<DateTime?>("ms_lastexecutetime"),
                RetryNumber = retry != null ? retry.Value : 0
            };

            message.Extensions[_queueName] = queue;
            return message;
        }

        public async Task<SMessage> QueryByOriginalID(SQueue queue, Guid originalMessageID, Guid listenerID)
        {
            throw new NotImplementedException();
        }

        public async Task<QueryResult<SMessage>> QueryByQueue(SQueue queue, int page, int pageSize)
        {
            var crmService = await getCrmService(queue.ServerName);
            string strFetch =string.Format(@"<fetch mapping=""logical"" version=""1.0"" distinct=""false"" page=""{1}"" count=""{2}"" output-format=""xml-platform"">
                                    <entity name=""{0}"">
                                          <attribute name=""{0}id"" />
                                          <attribute name=""ms_name"" />
                                          <attribute name=""ms_type"" />
                                          <attribute name=""ms_data"" />
                                          <attribute name=""ms_delaymessageid"" />
                                          <attribute name=""ms_exceptionmessage"" />
                                          <attribute name=""ms_expectationexecutetime"" />
                                          <attribute name=""ms_extensionmessage"" />
                                          <attribute name=""ms_lastexecutetime"" />
                                          <attribute name=""ms_retrynumber"" />
                                          <attribute name=""ms_isdead"" />
                                          <attribute name=""createdon"" />
                                          <attribute name=""modifiedon"" />
                                    <order descending=""false"" attribute=""createdon"" />
                                    </entity>
                               </fetch>", queue.Name,page.ToString(),pageSize.ToString());


                CrmRetrieveMultipleFetchRequestMessage request = new CrmRetrieveMultipleFetchRequestMessage()
                {
                    FetchXml = XDocument.Parse(strFetch),
                    EntityName = queue.Name
                };

                var response = (CrmRetrieveMultipleFetchResponseMessage)await crmService.Execute(request);

                List<SMessage> messages = new List<SMessage>();

            foreach (var item in response.Value.Results)
            {
                var isDead = await item.GetAttributeValue<bool?>("ms_isdead");
                var expectationExecuteTime = await item.GetAttributeValue<DateTime?>("ms_expectationexecutetime");
                var retry = await item.GetAttributeValue<int?>("ms_retrynumber");
                var delayMessageid = await item.GetAttributeValue<string>("ms_delaymessageid");
                Guid? delayMessageidObj = null;
                if (!string.IsNullOrEmpty(delayMessageid))
                {
                    delayMessageidObj = Guid.Parse(delayMessageid);
                }

                var message = new SMessage()
                {
                    ID = item.Id,
                    CreateTime = (await item.GetAttributeValue<DateTime?>("createdon")).Value,
                    Key = await item.GetAttributeValue<string>("ms_name"),
                    Type = await item.GetAttributeValue<string>("ms_type"),
                    Data = await item.GetAttributeValue<string>("ms_data"),
                    IsDead = isDead != null ? isDead.Value : false,
                    DelayMessageID = delayMessageidObj,
                    ExceptionMessage = await item.GetAttributeValue<string>("ms_exceptionmessage"),
                    ExpectationExecuteTime = expectationExecuteTime != null ? expectationExecuteTime.Value : DateTime.MinValue,
                    ExtensionMessage = await item.GetAttributeValue<string>("ms_extensionmessage"),
                    LastExecuteTime = await item.GetAttributeValue<DateTime?>("ms_lastexecutetime"),
                    RetryNumber = retry != null ? retry.Value : 0
                };

                message.Extensions[_queueName] = queue;
                messages.Add(message);


            }

            QueryResult<SMessage> result = new QueryResult<SMessage>()
            {
                 CurrentPage=page,
                  Results= messages
            };
            return result;
        }

        public async Task UpdateLastExecuteTime(SQueue queue, Guid id)
        {
            var crmService = await getCrmService(queue.ServerName);

            CrmExecuteEntity entity = new CrmExecuteEntity(queue.Name, id)
            {
                IsActivity = false,
                Attributes = new Dictionary<string, object>()
                 {

                      { "ms_lastexecutetime",DateTime.UtcNow},
                 }
            };
            await crmService.Update(entity);
        }

        private async Task<ICrmService> getCrmService(string factoryName)
        {
           var crmServiceFacotry=await _crmServiceFactoryRepositoryCacheProxy.QueryByName(factoryName);
            if (crmServiceFacotry == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFountCrmServiceFactorybyName,
                    DefaultFormatting = "找不到名称为{0}的Crm服务工厂",
                    ReplaceParameters = new List<object>() { factoryName }
                };

                throw new UtilityException((int)Errors.NotFountCrmServiceFactorybyName, fragment);
            }
            return await crmServiceFacotry.Create();
        }
    }
}
