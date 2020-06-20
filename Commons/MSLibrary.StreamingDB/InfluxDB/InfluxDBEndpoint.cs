using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Template;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.StreamingDB.InfluxDB
{
    /// <summary>
    /// InfluxDB终结点
    /// </summary>
    public class InfluxDBEndpoint : EntityBase<IInfluxDBEndpointIMP>
    {
        private static IFactory<IInfluxDBEndpointIMP>? _influxDBEndpointIMPFactory;

        public static IFactory<IInfluxDBEndpointIMP>? InfluxDBEndpointIMPFactory
        {
            set
            {
                _influxDBEndpointIMPFactory = value;
            }
        }
        public override IFactory<IInfluxDBEndpointIMP>? GetIMPFactory()
        {
            return _influxDBEndpointIMPFactory;
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
        /// 数据库地址
        /// </summary>
        public string Address
        {
            get
            {

                return GetAttribute<string>(nameof(Address));
            }
            set
            {
                SetAttribute<string>(nameof(Address), value);
            }
        }

        /// <summary>
        /// 是否需要鉴权
        /// </summary>
        public bool IsAuth
        {
            get
            {

                return GetAttribute<bool>(nameof(IsAuth));
            }
            set
            {
                SetAttribute<bool>(nameof(IsAuth), value);
            }
        }

        /// <summary>
        /// 鉴权用户名
        /// </summary>
        public string? UserName
        {
            get
            {

                return GetAttribute<string?>(nameof(UserName));
            }
            set
            {
                SetAttribute<string?>(nameof(UserName), value);
            }
        }

        /// <summary>
        /// 鉴权密码
        /// </summary>
        public string? Password
        {
            get
            {

                return GetAttribute<string?>(nameof(Password));
            }
            set
            {
                SetAttribute<string?>(nameof(Password), value);
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
        /// 创建数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public async Task CreateDataBase(string dbName)
        {
            await _imp.CreateDataBase(this, dbName);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task AddData(string dbName, InfluxDBRecord record)
        {
            await _imp.AddData(this, dbName, record);
        }

        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task AddDatas(string dbName, IList<InfluxDBRecord> records)
        {
            await _imp.AddDatas(this, dbName, records);
        }

    }

    public interface IInfluxDBEndpointIMP
    {
        Task AddData(InfluxDBEndpoint endpoint, string dbName, InfluxDBRecord record);
        Task AddDatas(InfluxDBEndpoint endpoint, string dbName, IList<InfluxDBRecord> records);
        Task CreateDataBase(InfluxDBEndpoint endpoint, string dbName);
    }

    [Injection(InterfaceType = typeof(IInfluxDBEndpointIMP), Scope = InjectionScope.Transient)]
    public class InfluxDBEndpointIMP : IInfluxDBEndpointIMP
    {
        /// <summary>
        /// 文本替换服务
        /// 如果该属性赋值，则configuration中的内容将首先使用该服务来替换占位符
        /// </summary>
        public static ITextReplaceService? TextReplaceService { set; get; }

        private readonly IHttpClientFactory _httpClientFactory;

        public InfluxDBEndpointIMP(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task AddData(InfluxDBEndpoint endpoint, string dbName, InfluxDBRecord record)
        {
            using (var httpClient= _httpClientFactory.CreateClient())
            {
                initHttpClient(endpoint, httpClient);
                HttpContent content = new StringContent(record.ToDataString());
                var response=await httpClient.PostAsync($"{ getContent(endpoint.Address)}/write?db={dbName.ToUrlEncode()}", content);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = response.ReasonPhrase;
                    }

                    var fragment = new TextFragment()
                    {
                        Code = StreamingDBTextCodes.InfluxDBEndpointOperateError,
                        DefaultFormatting = "找不到类型为{0}的SSH终结点服务，发生位置为{1}",
                        ReplaceParameters = new List<object>() { endpoint.Name,errorMessage, $"{this.GetType().FullName}.AddData" }
                    };

                    throw new UtilityException((int)StreamingDBErrorCodes.InfluxDBEndpointOperateError, fragment, 1, 0);
                }

            }
        }

        public async Task AddDatas(InfluxDBEndpoint endpoint, string dbName, IList<InfluxDBRecord> records)
        {
            if (records.Count==0)
            {
                return;
            }
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                initHttpClient(endpoint, httpClient);

                HttpContent content = new StringContent(records.ToDisplayString((v) => v.ToDataString(), () => "\r\n"));
                var response = await httpClient.PostAsync($"{ getContent(endpoint.Address)}/write?db={dbName.ToUrlEncode()}", content);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = response.ReasonPhrase;
                    }

                    var fragment = new TextFragment()
                    {
                        Code = StreamingDBTextCodes.InfluxDBEndpointOperateError,
                        DefaultFormatting = "找不到类型为{0}的SSH终结点服务，发生位置为{1}",
                        ReplaceParameters = new List<object>() { endpoint.Name, errorMessage, $"{this.GetType().FullName}.AddDatas" }
                    };

                    throw new UtilityException((int)StreamingDBErrorCodes.InfluxDBEndpointOperateError, fragment, 1, 0);
                }

            }
        }

        public async Task CreateDataBase(InfluxDBEndpoint endpoint, string dbName)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                initHttpClient(endpoint, httpClient);

                HttpContent content = new StringContent($"q=CREATE DATABASE {dbName}");
                var response = await httpClient.PostAsync($"{ getContent(endpoint.Address)}/query", content);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = response.ReasonPhrase;
                    }

                    var fragment = new TextFragment()
                    {
                        Code = StreamingDBTextCodes.InfluxDBEndpointOperateError,
                        DefaultFormatting = "找不到类型为{0}的SSH终结点服务，发生位置为{1}",
                        ReplaceParameters = new List<object>() { endpoint.Name, errorMessage, $"{this.GetType().FullName}.CreateDataBase" }
                    };

                    throw new UtilityException((int)StreamingDBErrorCodes.InfluxDBEndpointOperateError, fragment, 1, 0);
                }

            }
        }

        private void initHttpClient(InfluxDBEndpoint endpoint,HttpClient httpClient)
        {
            if (endpoint.IsAuth)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", $"{endpoint.UserName}:{endpoint.Password}".Base64Encode());
            }
        }

        private async Task<string> getContent(string content)
        {
            if (TextReplaceService!=null)
            {
                return await TextReplaceService.Replace(content);
            }
            else
            {
                return content;
            }
        }
    }
}
