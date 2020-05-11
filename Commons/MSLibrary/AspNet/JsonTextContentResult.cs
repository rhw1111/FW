using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using MSLibrary.Serializer;

namespace MSLibrary.AspNet
{
    /// <summary>
    /// 将数据转成Json格式后再以Text格式返回
    /// </summary>
    public class JsonTextContentResult<T> : IActionResult
    {
        private T _content;
        private int _statusCode;

        public JsonTextContentResult(int statusCode, T content)
        {
            _statusCode = statusCode;
            _content = content;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            await context.HttpContext.Response.WriteJsonText(_statusCode, _content);
        }
    }
}
