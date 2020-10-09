using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Buffers;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Xrm.MessageHandle;
using MSLibrary.Xrm.Token;
using MSLibrary.Xrm.Message.AssociateCollection;
using MSLibrary.Xrm.Message.BoundAction;
using MSLibrary.Xrm.Message.BoundFunction;
using MSLibrary.Xrm.Message.UnBoundAction;
using MSLibrary.Xrm.Message.UnBoundFunction;
using MSLibrary.Xrm.Message.Retrieve;
using MSLibrary.Xrm.Message.RetrieveMultiple;
using MSLibrary.Xrm.Message.RetrieveMultipleFetch;
using MSLibrary.Xrm.Message.RetrieveMultiplePage;
using MSLibrary.Xrm.Message.RetrieveMultipleSavedQuery;
using MSLibrary.Xrm.Message.RetrieveMultipleUserQuery;
using MSLibrary.Xrm.Message.Update;
using MSLibrary.Xrm.Message.UpdateRetrieve;
using MSLibrary.Xrm.Message.Upsert;
using MSLibrary.Xrm.Message.UpsertRetrieve;
using MSLibrary.Xrm.Message.Create;
using MSLibrary.Xrm.Message.CreateRetrieve;
using MSLibrary.Xrm.Message.Delete;
using MSLibrary.Xrm.Message.DisAssociateCollection;
using MSLibrary.Xrm.Message.FileAttributeDeleteData;
using MSLibrary.Xrm.Message.FileAttributeDownloadChunking;
using MSLibrary.Xrm.Message.FileAttributeUploadChunking;
using MSLibrary.Xrm.Message.GetFileAttributeUploadInfo;
using MSLibrary.Xrm.IOExtensions;

namespace MSLibrary.Xrm
{
    [Injection(InterfaceType = typeof(CrmService), Scope = InjectionScope.Transient)]
    public class CrmService : ICrmService
    {
        public static int MajorVersion { set; get; } = 2;
        public static int MinorVersion { set; get; } = 0;

        private IHttpClientFactoryWrapper _httpClientFactory;
        private ICrmServiceTokenGenerateServiceSelector _crmServiceTokenGenerateServiceSelector;
        private ICrmMessageHandleSelector _crmMessageHandleSelector;
        private ICrmMessageResponseHandle _crmMessageResponseHandle;

        public CrmService(IHttpClientFactoryWrapper httpClientFactory,ICrmServiceTokenGenerateServiceSelector crmServiceTokenGenerateServiceSelector,ICrmMessageHandleSelector crmMessageHandleSelector, ICrmMessageResponseHandle crmMessageResponseHandle)
        {
            _httpClientFactory = httpClientFactory;
            _crmServiceTokenGenerateServiceSelector = crmServiceTokenGenerateServiceSelector;
            _crmMessageHandleSelector = crmMessageHandleSelector;
            _crmMessageResponseHandle = crmMessageResponseHandle;
        }


        /// <summary>
        /// 令牌服务类型
        /// </summary>
        public string TokenServiceType
        {
            get;set;
        }

        /// <summary>
        /// Crm的Url
        /// </summary>
        public string CrmUrl
        {
            get;set;
        }

        /// <summary>
        /// Crm的Api版本
        /// </summary>
        public string CrmApiVersion
        {
            get;set;
        }

        /// <summary>
        /// 服务的最大重试次数
        /// </summary>
        public int CrmApiMaxRetry
        {
            get;set;
        }
        /// <summary>
        /// 令牌服务的参数集合
        /// </summary>
        public Dictionary<string, object> TokenServiceParameters
        {
            get;
        } = new Dictionary<string, object>();

        public async Task Associate(string entityName, string associateEntityName, string relationName, Guid entityId, Guid associateEntityId, Guid? proxyUserId = null)
        {
            CrmAssociateCollectionRequestMessage message = new CrmAssociateCollectionRequestMessage()
            {
                EntityName = entityName,
                EntityId = entityId,
                AssociateEntityName = associateEntityName,
                AssociateEntityId = associateEntityId,
                AttributeName = relationName,
                ProxyUserId = proxyUserId
            };
            await Execute(message);
        }

        public async Task<Guid> Create(CrmExecuteEntity entity, Guid? proxyUserId = null)
        {
            CrmCreateRequestMessage request = new CrmCreateRequestMessage()
            {
                Entity = entity,
                ProxyUserId = proxyUserId
            };

            var response = await Execute(request);
            return ((CrmCreateResponseMessage)response).Id;
        }

        public async Task<CrmEntity> Create(CrmExecuteEntity entity, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes)
        {
            CrmCreateRetrieveRequestMessage request = new CrmCreateRetrieveRequestMessage()
            {
                Entity = entity,
                ProxyUserId = proxyUserId,
                 Attributes=attributes,
            };

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response = await Execute(request);
            return ((CrmCreateRetrieveResponseMessage)response).Entity;
        }

        public async Task Delete(string entityName, Guid entityId, Guid? proxyUserId = null)
        {
            CrmDeleteRequestMessage request = new CrmDeleteRequestMessage()
            {
                EntityId = entityId,
                EntityName = entityName,
                ProxyUserId = proxyUserId
            };


            await Execute(request);
        }

        public async Task DeleteAlternate(string entityName, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null)
        {
            CrmDeleteRequestMessage request = new CrmDeleteRequestMessage()
            {
                 EntityName=entityName,
                  AlternateKeys=alternateKeys,
                ProxyUserId = proxyUserId,
            };
            await Execute(request);
        }

        public async Task DisAssociate(string entityName, string relationName, Guid entityId, Guid associateEntityId, Guid? proxyUserId = null)
        {
            CrmDisAssociateCollectionRequestMessage request = new CrmDisAssociateCollectionRequestMessage()
            {
                EntityName = entityName,
                EntityId = entityId,
                AttributeName = relationName,
                DisAssociateEntityId = associateEntityId,
                ProxyUserId = proxyUserId
            };
            await Execute(request);
        }

        public async Task<CrmResponseMessage> Execute(CrmRequestMessage request)
        {
            //填充request属性
            request.ApiVersion = CrmApiVersion;
            request.MaxRetry = CrmApiMaxRetry;
            request.OrganizationURI = CrmUrl;

            var handle = _crmMessageHandleSelector.Choose(request.GetType().FullName);
            var requestResult = await handle.ExecuteRequest(request);

            string strContentType = null;
            string strContentChartSet = null;
            Dictionary<string, string> contentParameters =new Dictionary<string, string>();

            HttpClient httpClient = null;
            if (TokenServiceType.ToLower() == CrmServiceTokenGenerateServiceNames.AD.ToLower())
            {
                var userName = TokenServiceParameters[CrmServiceTokenGenerateServiceParameterNames.UserName].ToString();
                var password = TokenServiceParameters[CrmServiceTokenGenerateServiceParameterNames.Password].ToString();
                var domain = TokenServiceParameters[CrmServiceTokenGenerateServiceParameterNames.Domain].ToString();
                httpClient = new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(userName, password, domain) });
            }
            else
            {
                httpClient = _httpClientFactory.CreateClient();
            }

            using (httpClient)
            {

                    foreach (var headerItem in requestResult.Headers)
                    {
                        switch (headerItem.Key.ToLower())
                        {
                            case "content-type":
                                strContentType = await headerItem.Value.ToDisplayString(
                                    async (item) =>
                                    {
                                        return await Task.FromResult(item);
                                    },
                                    async () =>
                                    {
                                        return await Task.FromResult(";");
                                    }
                                    );
                                break;
                            case "content-type-chartset":
                                strContentChartSet = headerItem.Value.First();
                                break;
                            case "accept":
                                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(headerItem.Value.First()));
                                break;
                            default:
                                if (headerItem.Key.ToLower().StartsWith("content-type-"))
                                {
                                    contentParameters[headerItem.Key.Substring(13)] = headerItem.Value.First();
                                    break;
                                }
                                httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                                break;

                        }
                    }
                

                //判断是否需要加入代理
                if (request.ProxyUserId != null)
                {
                    httpClient.DefaultRequestHeaders.Add("MSCRMCallerID", request.ProxyUserId.ToString());
                }


                HttpResponseMessage responseMessage = null;
                HttpContent httpContent = null;
                try
                {
                    await _crmMessageResponseHandle.Execute(request, async () =>
                    {
                    //获取令牌
                    if (TokenServiceType.ToLower() != CrmServiceTokenGenerateServiceNames.AD.ToLower())
                        {
                            var tokenService = _crmServiceTokenGenerateServiceSelector.Choose(TokenServiceType);
                            var strToken = await tokenService.Genereate(TokenServiceParameters);

                            httpClient.DefaultRequestHeaders.Add("Authorization", strToken);
                        }

                       
                        switch (requestResult.Method.Method.ToLower())
                        {
                            case "get":
                                var req = new HttpRequestMessage(HttpMethod.Get, requestResult.Url)
                                {
                                    Version = new Version(MajorVersion, MinorVersion),
                                };

                                responseMessage = await httpClient.SendAsync(req);
                                break;
                            case "post":

                                if (requestResult.ReplaceHttpContent == null)
                                {
                                    httpContent = new StringContent(requestResult.Body);
                                    if (strContentType != null)
                                    {
                                        httpContent.Headers.ContentType = new MediaTypeHeaderValue(strContentType);
                                        if (strContentChartSet != null)
                                        {
                                            httpContent.Headers.ContentType.CharSet = strContentChartSet;
                                        }
                                        foreach (var item in contentParameters)
                                        {
                                            httpContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue(item.Key, item.Value));
                                        }
                                    }
                                }
                                else
                                {
                                    httpContent = requestResult.ReplaceHttpContent;
                                }

                                 req = new HttpRequestMessage(HttpMethod.Post, requestResult.Url)
                                {
                                    Version = new Version(MajorVersion, MinorVersion),
                                    Content = httpContent
                                };

                                responseMessage = await httpClient.SendAsync(req);

                                break;
                            case "put":

                                if (requestResult.ReplaceHttpContent == null)
                                {
                                    httpContent = new StringContent(requestResult.Body);
                                    if (strContentType != null)
                                    {
                                        httpContent.Headers.ContentType = new MediaTypeHeaderValue(strContentType);
                                        if (strContentChartSet != null)
                                        {
                                            httpContent.Headers.ContentType.CharSet = strContentChartSet;
                                        }
                                        foreach (var item in contentParameters)
                                        {
                                            httpContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue(item.Key, item.Value));
                                        }
                                    }
                                }
                                else
                                {
                                    httpContent = requestResult.ReplaceHttpContent;
                                }

                                req = new HttpRequestMessage(HttpMethod.Put, requestResult.Url)
                                {
                                    Version = new Version(MajorVersion, MinorVersion),
                                    Content = httpContent
                                };

                                responseMessage = await httpClient.SendAsync(req);

                                break;
                            case "patch":

                                if (requestResult.ReplaceHttpContent == null)
                                {
                                    httpContent = new StringContent(requestResult.Body);
                                    if (strContentType != null)
                                    {
                                        httpContent.Headers.ContentType = new MediaTypeHeaderValue(strContentType);
                                        if (strContentChartSet != null)
                                        {
                                            httpContent.Headers.ContentType.CharSet = strContentChartSet;
                                        }
                                        foreach (var item in contentParameters)
                                        {
                                            httpContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue(item.Key, item.Value));
                                        }
                                    }
                                }
                                else
                                {
                                    httpContent = requestResult.ReplaceHttpContent;
                                }

                                req = new HttpRequestMessage(HttpMethod.Patch, requestResult.Url)
                                {
                                    Version = new Version(MajorVersion, MinorVersion),
                                    Content = httpContent
                                };

                                responseMessage = await httpClient.SendAsync(req);

                                break;
                            case "delete":
                                req = new HttpRequestMessage(HttpMethod.Delete, requestResult.Url)
                                {
                                    Version = new Version(MajorVersion, MinorVersion),
                                };

                                responseMessage = await httpClient.SendAsync(req);
                                break;
                            default:
                                TextFragment fragment = new TextFragment()
                                {
                                    Code = TextCodes.CrmMessageExecuteNotSupportMethod,
                                    DefaultFormatting = "Crm消息处理不支持名称为{0}的HttpMethod",
                                    ReplaceParameters = new List<object>() { requestResult.Method.Method }
                                };
                                throw new UtilityException((int)Errors.CrmMessageExecuteNotSupportMethod, fragment);
                        }

                        return responseMessage;
                    });
                }
                finally
                {
                    if (httpContent != null)
                    {
                        httpContent.Dispose();
                    }
                }
                Dictionary<string, IEnumerable<string>> responseHeaders = new Dictionary<string, IEnumerable<string>>();
                foreach (var headerItem in responseMessage.Headers)
                {
                    responseHeaders.Add(headerItem.Key, headerItem.Value);
                }
                var result = await handle.ExecuteResponse(requestResult.Extension, requestResult.Url, requestResult.Body, (int)responseMessage.StatusCode, responseHeaders, await responseMessage.Content.ReadAsStringAsync(),responseMessage);

                return result;
            }        
        }

        public async Task<JObject> ExecuteBoundAction(string entityName, Guid entityId, string actionName, Guid? proxyUserId = null, params CrmActionParameter[] parameters)
        {
            CrmBoundActionRequestMessage request = new CrmBoundActionRequestMessage()
            {
                EntityName = entityName,
                EntityId = entityId,
                ActionName = actionName,
                Parameters = parameters.ToList(),
                ProxyUserId = proxyUserId
            };
            var response = await Execute(request);
            return ((CrmBoundActionResponseMessage)response).Value;       
        }

        public async Task ExecuteBoundActionVoid(string entityName, Guid entityId, string actionName, Guid? proxyUserId = null, params CrmActionParameter[] parameters)
        {
            CrmBoundActionRequestMessage request = new CrmBoundActionRequestMessage()
            {
                EntityName = entityName,
                EntityId = entityId,
                ActionName = actionName,
                Parameters = parameters.ToList(),
                ProxyUserId = proxyUserId
            };
             await Execute(request);
        }

        public async Task<JObject> ExecuteBoundFunction(string entityName, Guid entityId, string functionName, Guid? proxyUserId = null, params CrmFunctionParameter[] parameters)
        {
            CrmBoundFunctionRequestMessage request = new CrmBoundFunctionRequestMessage()
            {
                EntityName = entityName,
                EntityId = entityId,
                FunctionName = functionName,
                ProxyUserId = proxyUserId,
                Parameters = parameters.ToList()
            };
            var response=await Execute(request);
            return ((CrmBoundFunctionResponseMessage)response).Value;
        }

        public async Task<JObject> ExecuteUnBoundAction(string actionName, Guid? proxyUserId = null, params CrmActionParameter[] parameters)
        {
            CrmUnBoundActionRequestMessage request = new CrmUnBoundActionRequestMessage()
            {
                ActionName = actionName,
                Parameters = parameters.ToList(),
                ProxyUserId = proxyUserId
            };

            var response= await Execute(request);
            return ((CrmUnBoundActionResponseMessage)response).Value;

        }

        public async Task ExecuteUnBoundActionVoid(string actionName, Guid? proxyUserId = null, params CrmActionParameter[] parameters)
        {
            CrmUnBoundActionRequestMessage request = new CrmUnBoundActionRequestMessage()
            {
                ActionName = actionName,
                Parameters = parameters.ToList(),
                ProxyUserId = proxyUserId
            };

            await Execute(request);
           
        }

        public async Task<JObject> ExecuteUnBoundFunction(string functionName, Guid? proxyUserId = null, params CrmFunctionParameter[] parameters)
        {
            CrmUnBoundFunctionRequestMessage request = new CrmUnBoundFunctionRequestMessage()
            {
                FunctionName = functionName,
                Parameters = parameters.ToList(),
                ProxyUserId = proxyUserId
            };

            var response = await Execute(request);
            return ((CrmUnBoundFunctionResponseMessage)response).Value;

        }

        public async Task<CrmEntity> Retrieve(string entityName, Guid entityId, string queryExpression, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null)
        {
            CrmRetrieveRequestMessage request = new CrmRetrieveRequestMessage()
            {
                EntityId = entityId,
                EntityName = entityName,
                ProxyUserId = proxyUserId,
                QueryExpression = queryExpression
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            var response = await Execute(request);
            return ((CrmRetrieveResponseMessage)response).Entity;
        }

        public async Task<CrmEntity> RetrieveAlternate(string entityName, Dictionary<string, object> alternateKeys, string queryExpression, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null)
        {
            CrmRetrieveRequestMessage request = new CrmRetrieveRequestMessage()
            {
                AlternateKeys=alternateKeys,
                EntityName = entityName,
                ProxyUserId = proxyUserId,
                QueryExpression = queryExpression
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            var response = await Execute(request);
            return ((CrmRetrieveResponseMessage)response).Entity;
        }

        public async Task<CrmEntityCollection> RetrieveMultiple(string entityName, string queryExpression, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null)
        {
            CrmRetrieveMultipleRequestMessage request = new CrmRetrieveMultipleRequestMessage()
            {
                EntityName = entityName,
                ProxyUserId = proxyUserId,
                QueryExpression = queryExpression,                 
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response = await Execute(request);
            return ((CrmRetrieveMultipleResponseMessage)response).Value;
        }

        public async Task<CrmEntityCollection> RetrieveMultipleNextPage(string entityName, string nextLinkExpression, int pageSize, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null)
        {
            CrmRetrieveMultiplePageRequestMessage request = new CrmRetrieveMultiplePageRequestMessage()
            {
                EntityName = entityName,
                ProxyUserId = proxyUserId,
                QueryExpression = nextLinkExpression,
                PageSize = pageSize
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response = await Execute(request);
            return ((CrmRetrieveMultiplePageResponseMessage)response).Value;
        }

        public async Task<CrmEntityCollection> RetrieveMultiplePage(string entityName, string queryExpression, int pageSize, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null)
        {
            CrmRetrieveMultiplePageRequestMessage request = new CrmRetrieveMultiplePageRequestMessage()
            {
                EntityName = entityName,
                ProxyUserId = proxyUserId,
                QueryExpression = queryExpression,
                PageSize = pageSize
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response = await Execute(request);
            return ((CrmRetrieveMultiplePageResponseMessage)response).Value;
        }

        public async Task<CrmEntityCollection> RetrieveMultipleSavedQuery(string entityName, Guid saveQueryId, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null)
        {
            CrmRetrieveMultipleSavedQueryRequestMessage request = new CrmRetrieveMultipleSavedQueryRequestMessage()
            {
                EntityName = entityName,
                ProxyUserId = proxyUserId,
                SavedQueryId = saveQueryId
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            var response = await Execute(request);
            return ((CrmRetrieveMultipleSavedQueryResponseMessage)response).Value;

        }

        public async Task<CrmEntityCollection> RetrieveMultipleUserQuery(string entityName, Guid userQueryId, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null)
        {
            CrmRetrieveMultipleUserQueryRequestMessage request = new CrmRetrieveMultipleUserQueryRequestMessage()
            {
                EntityName = entityName,
                ProxyUserId = proxyUserId,
                 UserQueryId = userQueryId
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            var response = await Execute(request);
            return ((CrmRetrieveMultipleUserQueryResponseMessage)response).Value;
        }

        public async Task Update(CrmExecuteEntity entity, Guid? proxyUserId = null)
        {
            CrmUpdateRequestMessage request = new CrmUpdateRequestMessage()
            {
                Entity = entity,
                ProxyUserId = proxyUserId,
            };

            await Execute(request);
        }

        public async Task<CrmEntity> Update(CrmExecuteEntity entity, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes)
        {
            CrmUpdateRetrieveRequestMessage request = new CrmUpdateRetrieveRequestMessage()
            {
                Entity = entity,
                ProxyUserId = proxyUserId,
                Attributes = attributes
            };

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response=await Execute(request);
            return ((CrmUpdateRetrieveResponseMessage)response).Entity;
        }

        public async Task UpdateAlternate(CrmExecuteEntity entity, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null)
        {
            CrmUpdateRequestMessage request = new CrmUpdateRequestMessage()
            {
                Entity = entity,
                AlternateKeys = alternateKeys,
                ProxyUserId = proxyUserId,
            };

            await Execute(request);
        }

        public async Task<CrmEntity> UpdateAlternate(CrmExecuteEntity entity, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes)
        {
            CrmUpdateRetrieveRequestMessage request = new CrmUpdateRetrieveRequestMessage()
            {
                Entity = entity,
                AlternateKeys=alternateKeys,
                ProxyUserId = proxyUserId,
                Attributes = attributes
            };

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response = await Execute(request);
            return ((CrmUpdateRetrieveResponseMessage)response).Entity;
        }

        public async Task Upsert(CrmExecuteEntity entity, Guid? proxyUserId = null)
        {
            CrmUpsertRequestMessage request = new CrmUpsertRequestMessage()
            {
                Entity = entity,
                ProxyUserId = proxyUserId,
            };

            await Execute(request);
        }

        public async Task<CrmEntity> Upsert(CrmExecuteEntity entity, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes)
        {
            CrmUpsertRetrieveRequestMessage request = new CrmUpsertRetrieveRequestMessage()
            {
                Entity = entity,
                ProxyUserId = proxyUserId,
                Attributes = attributes
            };

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response = await Execute(request);
            return ((CrmUpsertRetrieveResponseMessage)response).Entity;
        }

        public async Task UpsertAlternate(CrmExecuteEntity entity, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null)
        {
            CrmUpsertRequestMessage request = new CrmUpsertRequestMessage()
            {
                Entity = entity,
                AlternateKeys=alternateKeys,
                ProxyUserId = proxyUserId,
            };

            await Execute(request);
        }

        public async Task<CrmEntity> UpsertAlternate(CrmExecuteEntity entity, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes)
        {
            CrmUpsertRetrieveRequestMessage request = new CrmUpsertRetrieveRequestMessage()
            {
                Entity = entity,
                AlternateKeys = alternateKeys,
                ProxyUserId = proxyUserId,
                Attributes = attributes
            };

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            var response = await Execute(request);
            return ((CrmUpsertRetrieveResponseMessage)response).Entity;
        }


        /// <summary>
        /// 为文件类型属性上传文件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fileAttributeName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        public async Task UploadAttributeFile(CrmEntityReference entityID, string fileAttributeName, string fileName, string fileMimeType, Stream fileStream, Guid? proxyUserId = null)
        {

            CrmGetFileAttributeUploadInfoRequestMessage getRequest = new CrmGetFileAttributeUploadInfoRequestMessage()
            {
                EntityName = entityID.EntityName,
                EntityId = entityID.Id,
                AttributeName = fileAttributeName,
                FileName = fileName
            };


            var getResponse = (CrmGetFileAttributeUploadInfoResponseMessage)await this.Execute(getRequest);

            int perSize = getResponse.PerSize;
            int currentSize = perSize;

            using (var buffOwner = MemoryPool<byte>.Shared.Rent(perSize))
            {
                var buff = buffOwner.Memory;
                List<string> blockIDs = new List<string>();
                int position = 0;
                while (currentSize == perSize)
                {
                    currentSize = await fileStream.ReadAsync(buff);
                    if (currentSize != 0)
                    {

                        var blockID = Guid.NewGuid().ToString();
                        blockIDs.Add(blockID);

                        CrmFileAttributeUploadChunkingRequestMessage uploadRequest = new CrmFileAttributeUploadChunkingRequestMessage()
                        {
                            UploadUrl = getResponse.UploadUrl,
                            Data = buff.Slice(0, currentSize).ToArray(),
                            FileName = fileName,
                            Start = position,
                            End = currentSize - 1,
                            Total = fileStream.Length
                        };

                        position += currentSize;
                        await this.Execute(uploadRequest);
                    }
                }
            }



                

        }
        /// <summary>
        /// 下载文件类型属性的文件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fileAttributeName"></param>
        /// <param name="action"></param>
        public async Task DownloadAttributeFile(CrmEntityReference entityID, string fileAttributeName, Func<string, Stream,Task> action, Guid? proxyUserId = null)
        {
            CrmFileAttributeDownloadChunkingRequestMessage request = new CrmFileAttributeDownloadChunkingRequestMessage()
            {
                EntityName = entityID.EntityName,
                EntityId = entityID.Id,
                AttributeName = fileAttributeName,
                Start = 0,
                End = 1                 
            };

            var response=(CrmFileAttributeDownloadChunkingResponseMessage)await this.Execute(request);

            await using (var stream = new CrmFileBlocksStream (entityID.EntityName,entityID.Id, response.FileName, response.Total, this,proxyUserId))
            {
                
                  await action(response.FileName, stream);
            }
        }
        /// <summary>
        /// 删除文件类型属性的文件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fileAttributeName"></param>
        public async Task DeleteAttributeFileData(CrmEntityReference entityID, string fileAttributeName, Guid? proxyUserId = null)
        {
            CrmFileAttributeDeleteDataRequestMessage request = new CrmFileAttributeDeleteDataRequestMessage()
            {
                EntityName = entityID.EntityName,
                EntityId = entityID.Id,
                AttributeName = fileAttributeName,
            };

            await this.Execute(request);

        }


    }


    public interface ICrmMessageResponseHandle
    {
        Task Execute(CrmRequestMessage request, Func<Task<HttpResponseMessage>> action);
    }


    [Injection(InterfaceType = typeof(ICrmMessageResponseHandle), Scope = InjectionScope.Transient)]
    public class CrmMessageResponseHandle : ICrmMessageResponseHandle
    {
        public async Task Execute(CrmRequestMessage request,Func<Task<HttpResponseMessage>> action)
        {
            int retryNumber = 0;
            while (true)
            {
                var response = await action();
                retryNumber++;
                if (!response.IsSuccessStatusCode)
                {
                    //先判断content为空
                    var requestBody = string.Empty;
                    if (response.RequestMessage.Content != null)
                    {
                        requestBody = await response.RequestMessage.Content.ReadAsStringAsync();
                    }
                    var strContent = await response.Content.ReadAsStringAsync();

                    //这个后面没用到，还容易报错，暂时注释
                    //var error = JsonSerializerHelper.Deserialize<CrmWebApiError>(strContent);
                    UtilityException ex = null;
           
                    TextFragment fragment;
                    switch ((int)response.StatusCode)
                    {
                        case 412:
                            fragment = new TextFragment()
                            {
                                Code = TextCodes.CrmWebApiConcurrencyError,
                                DefaultFormatting = "调用Crm的webapi出现并发性错误，Uri:{0},Body：{1}，错误信息：{2}",
                                ReplaceParameters = new List<object>() { response.RequestMessage.RequestUri.ToString(), requestBody, strContent }
                            };
                            ex = new UtilityException((int)Errors.CrmWebApiConcurrencyError, fragment);
                            break;
                        case 429:
                            if (retryNumber >= request.MaxRetry)
                            {
                                fragment = new TextFragment()
                                {
                                    Code = TextCodes.CrmWebApiLimitError,
                                    DefaultFormatting = "调用Crm的webapi出现限制性错误，Uri:{0},Body：{1}，错误信息：{2}",
                                    ReplaceParameters = new List<object>() { response.RequestMessage.RequestUri.ToString(), requestBody, strContent }
                                };

                                ex = new UtilityException((int)Errors.CrmWebApiLimitError, fragment);
                            }
                            else
                            {
                                await Task.Delay(response.Headers.RetryAfter.Delta.Value);
                                //System.Threading.Thread.Sleep(response.Headers.RetryAfter.Delta.Value);                             
                            }
                            break;
                        default:
                            fragment = new TextFragment()
                            {
                                Code = TextCodes.CrmWebApiCommonError,
                                DefaultFormatting = "调用Crm的webapi出现错误，Uri:{0},Body：{1}，错误信息：{2}",
                                ReplaceParameters = new List<object>() { response.RequestMessage.RequestUri.ToString(), requestBody, strContent }
                            };

                            ex = new UtilityException((int)Errors.CrmWebApiCommonError, fragment);
                            break;
                    }

                    if (ex != null)
                    {
                        throw ex;
                    }

                }
                else
                {
                    break;
                }
            }

        }
    }
}
