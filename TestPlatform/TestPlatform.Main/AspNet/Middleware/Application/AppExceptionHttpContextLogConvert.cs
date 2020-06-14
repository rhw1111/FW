using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Buffers;
using Microsoft.AspNetCore.Http;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.DI;
using FW.TestPlatform.Main.Logger;

namespace FW.TestPlatform.Main.AspNet.Middleware.Application
{
    /// <summary>
    /// Http上下文转换成日志对象
    /// 本系统中转换为LoggerContent
    /// </summary>
    [Injection(InterfaceType = typeof(IAppExceptionHttpContextLogConvert), Scope = InjectionScope.Singleton)]
    public class AppExceptionHttpContextLogConvert : IAppExceptionHttpContextLogConvert
    {
        /// <summary>
        /// 记录的最大请求内容长度
        /// 超过长度的，将截取请求内容
        /// </summary>
        private const long _maxRequestLength = 102400;

        public async Task<object> Convert(HttpContext context)
        {
            string strRequestBody = null;
            //尝试获取请求内容和响应内容
            if (context.Request != null && context.Request.Body != null && context.Request.Body.CanRead)
            {
                await using (MemoryStream requestStream = new MemoryStream())
                {

                    await context.Request.Body.CopyToAsync(requestStream);
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



            //取出存储在上下文Item中的异常
            var ex = (Exception)context.Items["ExecuteException"];

            LoggerContent content = new LoggerContent() { RequestUri = context.Request!=null?context.Request.Path.Value:string.Empty, ActionName = "", Message = $"Unhandle Error,\nmessage:{ex.Message},\nstacktrace:{ex.StackTrace}", RequestBody = strRequestBody??string.Empty, ResponseBody = ""};

            if (context.Request != null)
            {
                content.Tags.Add(context.Request.PathBase.Value);
            }
            return await Task.FromResult(content);
        }
    }
}
