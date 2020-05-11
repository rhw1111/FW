using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using MSLibrary.Serializer;

namespace MSLibrary.AspNet
{
    /// <summary>
    /// HttpResponse的扩展方法类
    /// </summary>
    public static class HttpResponseExtensions
    {
        public static async Task<HttpResponse> WriteJson<T>(this HttpResponse response, int statusCode,T content)
        {
            var strContent = JsonSerializerHelper.Serializer<T>(content);
            var contentBytes = UTF8Encoding.UTF8.GetBytes(strContent);
            response.StatusCode = statusCode;
            response.ContentType = "application/json; charset=utf-8";
            await response.Body.WriteAsync(contentBytes, 0, contentBytes.Length);
            return response;
        }

        public static async Task<HttpResponse> WriteJson(this HttpResponse response, int statusCode,Type contentType, object content)
        {
            var strContent = JsonSerializerHelper.Serializer(content,contentType);
            var contentBytes = UTF8Encoding.UTF8.GetBytes(strContent);
            response.StatusCode = statusCode;
            response.ContentType = "application/json; charset=utf-8";
            await response.Body.WriteAsync(contentBytes, 0, contentBytes.Length);
            return response;
        }



        public static async Task<HttpResponse> WriteXml(this HttpResponse response, int statusCode, XDocument doc)
        {
            var strContent = doc.ToString();
            var contentBytes = UTF8Encoding.UTF8.GetBytes(strContent);
            response.StatusCode = statusCode;
            response.ContentType = "text/xml; charset=utf-8";
            await response.Body.WriteAsync(contentBytes, 0, contentBytes.Length);
            return response;
        }

        public static async Task<HttpResponse> WriteJsonText<T>(this HttpResponse response, int statusCode, T content)
        {
            var strContent = JsonSerializerHelper.Serializer<T>(content);
            var str = JsonSerializerHelper.Serializer<string>(strContent);
            var contentBytes = UTF8Encoding.UTF8.GetBytes(str);
            response.StatusCode = statusCode;
            response.ContentType = "text/html; charset=utf-8";
            await response.Body.WriteAsync(contentBytes, 0, contentBytes.Length);
            return response;

        }

        public static async Task<HttpResponse> WriteJsonText(this HttpResponse response, int statusCode,Type contentType, object content)
        {
            var strContent = JsonSerializerHelper.Serializer(content,contentType);
            var str = JsonSerializerHelper.Serializer<string>(strContent);
            var contentBytes = UTF8Encoding.UTF8.GetBytes(str);
            response.StatusCode = statusCode;
            response.ContentType = "text/html; charset=utf-8";
            await response.Body.WriteAsync(contentBytes, 0, contentBytes.Length);
            return response;

        }

    }
}
