using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using MSLibrary.Serializer;

namespace MSLibrary.AspNet
{
    /// <summary>
    /// 指定类型数据Json序列化后的请求响应
    /// </summary>
    public class JsonContentResult<T> : IActionResult
    {
        private T _content;
        private int _statusCode;

        public JsonContentResult(int statusCode, T content)
        {
            _statusCode = statusCode;
            _content = content;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            await context.HttpContext.Response.WriteJson(_statusCode, _content);
        }
    }

    public class JsonContentResult : IActionResult
    {
        private Type _contentType;
        private object _content;
        private int _statusCode;

        public JsonContentResult(int statusCode, Type contentType,object content)
        {
            _contentType = contentType;
            _statusCode = statusCode;
            _content = content;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            await context.HttpContext.Response.WriteJson(_statusCode,_contentType, _content);
        }
    }
}
