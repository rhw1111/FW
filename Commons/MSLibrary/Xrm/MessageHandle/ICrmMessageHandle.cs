using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSLibrary.Xrm.MessageHandle
{
    /// <summary>
    /// Crm消息处理接口
    /// </summary>
    public interface ICrmMessageHandle
    {
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request);

        /// <summary>
        /// 处理响应
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="responseHeaders"></param>
        /// <param name="responseBody"></param>
        /// <returns></returns>
        Task<CrmResponseMessage> ExecuteResponse(object extension,string requestUrl, string requestBody,int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders,string responseBody,HttpResponseMessage response);
    }


    /// <summary>
    /// Crm请求消息的处理结果
    /// </summary>
    public class CrmRequestMessageHandleResult
    {
        /// <summary>
        /// http请求模式
        /// </summary>
        public HttpMethod Method { get; set; }
        /// <summary>
        /// http请求的Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// http请求的头信息
        /// </summary>
        public Dictionary<string, IEnumerable<string>> Headers { get; set; }
        /// <summary>
        /// http请求的正文
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 要替换的请求内容
        /// 如果该属性有值。则忽略上面的Body
        /// </summary>
        public HttpContent ReplaceHttpContent { get; set; }
        /// <summary>
        /// 附加信息，会回传给响应处理方法
        /// </summary>
        public object Extension { get; set; }
    }
}
