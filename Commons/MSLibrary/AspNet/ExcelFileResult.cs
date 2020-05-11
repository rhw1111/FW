using Microsoft.AspNetCore.Mvc;
using MSLibrary.MessageQueue;
using MSLibrary.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.AspNet
{
    
    public class ExcelFileReslut : IActionResult
    {
        private MemoryStream stream;
        private string format;
        private string fileName;
        private string userName;
        private string branchCode;
        //private 
        public ExcelFileReslut(MemoryStream stream,string format,string fileName,string userName,string branchCode)
        {
            this.stream = stream;
            this.format = format;
            this.fileName = fileName;
            this.userName = userName;
            this.branchCode = branchCode;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = format;
            context.HttpContext.Response.Headers.Add("Content-disposition", "attachment;filename="+WebUtility.UrlEncode(fileName));
            byte[] bytes = new byte[1024];
            stream.Position = 0;
            while (true)
            {
                int count = await stream.ReadAsync(bytes, 0, bytes.Length);
                await context.HttpContext.Response.Body.WriteAsync(bytes, 0, count);
                if (count < bytes.Length)
                {
                    break;
                }
            }
            ///log
            Guid guid = Guid.NewGuid();
            var message = new SMessage()
            {
                Key = $"{"Common"}-{"ExcelExportLog"}-{guid}",
                Type = "ExcelExportLog",
                Data = JsonSerializerHelper.Serializer(new { Id = guid, CreateBy = userName, CreateOn = DateTime.UtcNow, FileName = fileName, FileSize = stream.Length,CreateBranch=branchCode }),
                ExpectationExecuteTime = DateTime.UtcNow,
                RetryNumber = 0,
                IsDead = false
            };
            await message.Add();
        }
    }
}
