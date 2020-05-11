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
    /// 将XDocument数据加入到请求响应
    /// </summary>
    public class XmlContentResult : IActionResult
    {
        private XDocument _document;
        private int _statusCode;

        public XmlContentResult(int statusCode, XDocument document)
        {
            _statusCode = statusCode;
            _document = document;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            await context.HttpContext.Response.WriteXml(_statusCode, _document);
        }
    }
}
