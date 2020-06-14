using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MSLibrary.AspNet.Middleware;
using MSLibrary.AspNet.Middleware.Application;
using Microsoft.AspNetCore.Http.Extensions;
using MSLibrary.DI;
using System.Buffers;
using FW.TestPlatform.Main.Logger;


namespace FW.TestPlatform.Main.AspNet.Middleware.Application
{
    /// <summary>
    /// Http上下文转换成日志对象
    /// 本系统中转换为LoggerContent
    /// </summary>
    [Injection(InterfaceType = typeof(IAppHttpContextLogConvert), Scope = InjectionScope.Singleton)]
    public class AppHttpContextLogConvert : IAppHttpContextLogConvert
    {
        /// <summary>
        /// 记录的最大请求内容长度
        /// 超过长度的，将截取请求内容
        /// </summary>
        private const long _maxRequestLength = 102400;

        public async Task<object> Convert(HttpContextData data)
        {
            LoggerContent content = new LoggerContent();          
            content.Tags.Add(data.RequestBasePath);
            content.ActionName = data.RequestPath;
            content.Message = string.Empty;

            string strRequestBody = string.Empty;
            string strResponseBody = string.Empty;
            //尝试获取请求内容和响应内容
            if (data.Request != null)
            {
                await using (data.Request)
                {
                    var length = data.Request.Length;
                    if (length > _maxRequestLength)
                    {
                        length = _maxRequestLength;
                    }

                    var bytes = ArrayPool<byte>.Shared.Rent((int)length);

                    try
                    {
                        await data.Request.ReadAsync(bytes, 0, (int)length);

                        strRequestBody = UTF8Encoding.UTF8.GetString(bytes.AsSpan(0, (int)length));
                        //context.Request.Body.Position = 0;
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(bytes, true);
                    }
                }
            }

            if (data.Response != null)
            {
                await using (data.Response)
                {

                    var bytes = ArrayPool<byte>.Shared.Rent((int)data.Response.Length);
                    try
                    {
                        await data.Response.ReadAsync(bytes, 0, (int)data.Response.Length);

                        strResponseBody = UTF8Encoding.UTF8.GetString(bytes.AsSpan(0, (int)data.Response.Length));
                        //context.Request.Body.Position = 0;
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(bytes, true);
                    }
                }
            }

            content.RequestBody = strRequestBody;
            content.ResponseBody = strResponseBody;
            content.RequestUri = data.RequestUri;
            content.Message = string.Empty;
            content.Duration = data.Duration;


            return await Task.FromResult(content);
        }
    }
}
