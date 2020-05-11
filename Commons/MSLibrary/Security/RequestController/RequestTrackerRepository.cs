using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Configuration;

namespace MSLibrary.Security.RequestController
{
    /// <summary>
    /// 请求跟踪仓储
    /// 静态变量_requestTrackerStaticData存储请求跟踪的所有实例
    /// 该变量的信息来自于应用程序目录下的文件RequestTrackerData.json
    /// </summary>
    [Injection(InterfaceType = typeof(IRequestTrackerRepository), Scope = InjectionScope.Singleton)]
    public class RequestTrackerRepository : IRequestTrackerRepository
    {
        private static RequestTrackerStaticData _requestTrackerStaticData = null;

        static RequestTrackerRepository()
        {
            ConfigurationContainer.RegisterJsonListener<RequestTrackerContainer>("RequestTrackerData.json", (container) =>
            {
                var requestTrackerStaticData = new RequestTrackerStaticData()
                {
                    Datas = new Dictionary<string, RequestTracker>()
                };

                requestTrackerStaticData.Global = new RequestTracker()
                {
                    ID = Guid.NewGuid(),
                    MaxNumber = container.Global.MaxNumber,
                    RequestKey = ""
                };
                requestTrackerStaticData.Global.Extensions.Add("Strategies", container.Global.Strategies);

                foreach(var item in container.Configuration)
                {
                    var newRequestTracker = new RequestTracker()
                    {
                        ID = Guid.NewGuid(),
                        MaxNumber = item.Value.MaxNumber,
                        RequestKey = item.Key
                    };
                    newRequestTracker.Extensions.Add("Strategies", item.Value.Strategies);
                    requestTrackerStaticData.Datas.Add(item.Key, newRequestTracker);
                }

                _requestTrackerStaticData = requestTrackerStaticData;
            });
        }
        public async Task<RequestTracker> QueryByRequestKey(string key)
        {
            if (!_requestTrackerStaticData.Datas.TryGetValue(key, out RequestTracker requestTracker))
            {
                requestTracker = null;
            }

            return await Task.FromResult(requestTracker);
        }

        public async Task<RequestTracker> QueryGlobal()
        {
            return await Task.FromResult(_requestTrackerStaticData.Global);
        }
    }



    class RequestTrackerStaticData
    {
        public RequestTrackerStaticData()
        {
            Datas = new Dictionary<string, RequestTracker>();
        }
        public RequestTracker Global
        {
            get; set;
        }

        public Dictionary<string, RequestTracker> Datas
        {
            get; set;
        }
    }


    [DataContract]
    class RequestTrackerData
    {
        [DataMember]
        public int MaxNumber { get; set; }
        [DataMember]
        public string[] Strategies { get; set; }
    }

    [DataContract]
    class RequestTrackerContainer
    {
        [DataMember]
        public RequestTrackerData Global { get; set; }
        [DataMember]
        public Dictionary<string, RequestTrackerData> Configuration { get; set; }
    }
}
