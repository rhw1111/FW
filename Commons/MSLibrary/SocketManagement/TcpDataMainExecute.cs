using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.SocketManagement
{
    /// <summary>
    /// ITcpDataExecute的默认实现
    /// 接收的信息格式为RequestTcpData的Json格式,Type用来区分消息，
    /// 根据Type转发到实际处理类处理,传递给实际处理类的是RequestTcpData的Data属性值,返回的数据为ResponseTcpData的Json格式的字节数组
    /// 如果UtilityException被抛出，则ResponseTcpData的Error为true，Data为对应的ErrorMessage的Json格式的字节数组
    /// 如果是其他Exception被抛出，则ResponseTcpData的Error为true，Data为对应的Exception的Message属性的字节数组
    /// </summary>
    [Injection(InterfaceType = typeof(TcpDataMainExecute), Scope = InjectionScope.Singleton)]
    public class TcpDataMainExecute : ITcpDataExecute
    {
        private static Dictionary<string, IFactory<ITcpDataExecute>> _tcpDataExecuteFactories = new Dictionary<string, IFactory<ITcpDataExecute>>();

        public static Dictionary<string, IFactory<ITcpDataExecute>> TcpDataExecuteFactories
        {
            get
            {
                return _tcpDataExecuteFactories;
            }
        }

        public async Task<byte[]> Execute(byte[] data)
        {
            RequestTcpData requestData = null;
            byte[] result;
            ResponseTcpData responseData = new ResponseTcpData();
            try
            {
                var strRequestData = UTF8Encoding.UTF8.GetString(data);
                requestData = JsonSerializerHelper.Deserialize<RequestTcpData>(strRequestData);

                if (requestData == null)
                {
                    throw new Exception($"request is {strRequestData}, but it is not the json of RequestTcpData");
                }

                if (!_tcpDataExecuteFactories.TryGetValue(requestData.Type, out IFactory<ITcpDataExecute> executeFactory))
                {
                    throw new Exception($"not found IFactory<ITcpDataExecute> {requestData.Type} in TcpDataMainExecute.TcpDataExecuteFactories");
                }

                var execute = executeFactory.Create();
                result = await execute.Execute(requestData.Data);
                responseData.Error = false;
                responseData.Data = result;
            }
            catch (UtilityException ex)
            {
                ErrorMessage errorMessage = new ErrorMessage()
                {
                    Code = ex.Code,
                    Message = await ex.GetCurrentLcidMessage()
                };

                var strErrorMessage = JsonSerializerHelper.Serializer<ErrorMessage>(errorMessage);
                result = UTF8Encoding.UTF8.GetBytes(strErrorMessage);
                responseData.Error = true;
                responseData.Data = result;
            }
            catch (Exception ex)
            {
                result = UTF8Encoding.UTF8.GetBytes(ex.Message);
                responseData.Error = true;
                responseData.Data = result;
            }

            var strResponseData = JsonSerializerHelper.Serializer<ResponseTcpData>(responseData);
            return UTF8Encoding.UTF8.GetBytes(strResponseData);
        }
    }

    [DataContract]
    public class RequestTcpData
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public byte[] Data { get; set; }
    }


    [DataContract]
    public class ResponseTcpData
    {
        [DataMember]
        public bool Error { get; set; }
        [DataMember]
        public byte[] Data { get; set; }
    }
}
