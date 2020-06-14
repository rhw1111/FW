using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Buffers;
using MSLibrary.DI;
using MSLibrary.AspNet.Filter.Application;
using FW.TestPlatform.Main.Logger;

namespace FW.TestPlatform.Main.AspNet.Filter.Application
{
    /// <summary>
    /// 将Exception过滤器上下文转换为日志
    /// 本系统中转换为LoggerContent
    /// </summary>
    [Injection(InterfaceType = typeof(IAppExceptionContextLogConvert), Scope = InjectionScope.Singleton)]
    public class AppExceptionContextLogConvert : IAppExceptionContextLogConvert
    {
        /// <summary>
        /// 记录的最大请求内容长度
        /// 超过长度的，将截取请求内容
        /// </summary>
        private const long _maxRequestLength = 102400;

        public async Task<object> Do(ExceptionContext context)
        {

            string? strRequestBody = null;
            //尝试获取请求内容和响应内容
            if (context.HttpContext.Request != null && context.HttpContext.Request.Body != null && context.HttpContext.Request.Body.CanRead)
            {
                await using (MemoryStream requestStream = new MemoryStream())
                {
                    await context.HttpContext.Request.Body.CopyToAsync(requestStream);
                    requestStream.Position = 0;

                    var length = requestStream.Length;
                    if (length > _maxRequestLength)
                    {
                        length = _maxRequestLength;
                    }

                    var bytes = ArrayPool<byte>.Shared.Rent((int)length);
                    try
                    {
                        await requestStream.ReadAsync(bytes, 0, (int)length);

                        strRequestBody = UTF8Encoding.UTF8.GetString(bytes.AsSpan(0, (int)length));
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(bytes, true);
                    }

                }
            }


            LoggerContent content = new LoggerContent() { RequestUri = context.HttpContext.Request!=null?context.HttpContext.Request.Path.Value:string.Empty, ActionName = context.ActionDescriptor.DisplayName.Split("(")[0].Trim(), Message = $"Unhandle Error,\nmessage:{context.Exception.Message},\nstacktrace:{context.Exception.StackTrace}", RequestBody = strRequestBody??string.Empty, ResponseBody = "" };
            if (context.HttpContext.Request != null)
            {
                content.Tags.Add(context.HttpContext.Request.PathBase.Value);
            }

            return await Task.FromResult(content);
        }
    }
}
