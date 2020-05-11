using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Serializer;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.SocketManagement
{
    /// <summary>
    /// Tcp客户端数据处理的默认实现
    /// 该实现对应服务端的TcpDataMainExecute，服务端返回两种响应
    /// R：表示心跳包的响应
    /// ResponseTcpData的序列化字符串：表示服务端的业务响应
    /// 将服务端的响应转换成TcpClientReceiveDataResult，
    /// 如果ResponseTcpData.Error==true，则首先尝试反序列化ResponseTcpData.Data为ErrorMessage，如果可以转换成功，则结果的Exception属性为转换成的UtilityException，否则结果的Exception属性为Exception
    /// 如果ResponseTcpData.Error==false,则结果的Data为响应的Data
    /// </summary>
    [Injection(InterfaceType = typeof(TcpClientDataMainExecute), Scope = InjectionScope.Singleton)]
    public class TcpClientDataMainExecute : ITcpClientDataExecute
    {
        public async Task<TcpClientReceiveDataResult> ReceiveExecute(byte[] receiveData)
        {
            TcpClientReceiveDataResult result = new TcpClientReceiveDataResult()
            {
                Data = new byte[0]
            };
            var strData = UTF8Encoding.UTF8.GetString(receiveData);
            if (strData.ToLower()=="r")
            {
                result.Exception = null;
                return await Task.FromResult(result);
            }

            ResponseTcpData responseData;

            try
            {
                responseData = JsonSerializerHelper.Deserialize<ResponseTcpData>(strData);
            }
            catch
            {
                responseData = null;
            }

            if (responseData == null)
            {
                result.Exception = new Exception($"response is {strData}, but it is not the json of ResponseTcpData");
            }
            else
            {
                if (responseData.Error)
                {
                    ErrorMessage errorMessage;
                    try
                    {
                        errorMessage = JsonSerializerHelper.Deserialize<ErrorMessage>(UTF8Encoding.UTF8.GetString(responseData.Data));
                    }
                    catch
                    {
                        errorMessage = null;
                    }

                    if (errorMessage == null)
                    {
                        result.Exception = new Exception(UTF8Encoding.UTF8.GetString(responseData.Data));
                    }
                    else
                    {
                        var fragment = new TextFragment()
                        {
                            Code = string.Empty,
                            DefaultFormatting = errorMessage.Message,
                            ReplaceParameters = new List<object>() { }
                        };

                        result.Exception = new UtilityException(errorMessage.Code, fragment);
                    }

                }
                else
                {
                    result.Exception = null;
                    result.Data = responseData.Data;
                }
            }


            return await Task.FromResult(result);
        }
    }
}
