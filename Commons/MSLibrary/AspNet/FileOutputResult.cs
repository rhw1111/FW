using Microsoft.AspNetCore.Mvc;
using MSLibrary.MessageQueue;
using MSLibrary.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.AspNet
{
    /// <summary>
    /// 文件输出结果
    /// </summary>
    public class FileOutputResult : IActionResult
    {
        /// <summary>
        /// 是否分步下载
        /// </summary>
        private bool _isPart;
        /// <summary>
        /// 要下载的流
        /// </summary>
        private Stream _stream;
        /// <summary>
        /// 总字节数
        /// </summary>
        private long _size;
        /// <summary>
        /// 字节起始位置
        /// </summary>
        private long? _start;
        /// <summary>
        /// 字节结束
        /// </summary>
        private long? _end;
        /// <summary>
        /// 下载的格式
        /// </summary>
        private string _contentType;
        /// <summary>
        /// 下载的文件名
        /// </summary>
        private string _fileName;


        public FileOutputResult(bool isPart, Stream stream, long size, long? start, long? end, string contentType, string fileName)
        {
            _isPart = isPart;
            _stream = stream;
            _size = size;
            _start = start;
            _end = end;
            _contentType = contentType;
            _fileName = fileName;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            //HttpResponseMessage response;

            if (_start == null)
            {
                _start = 0;
            }
            if (_end == null)
            {
                _end = _size - 1;
            }

            bool isOut = false;

            //计算位置是否超出

            if ((_start.Value < 0) || (_start > _end) || (_end > _size - 1))
            {
                isOut = true;
            }


            if (isOut)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status416RequestedRangeNotSatisfiable;
                //response = new HttpResponseMessage(HttpStatusCode.RequestedRangeNotSatisfiable);
            }
            else
            {

                if (_isPart)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status206PartialContent;
                    context.HttpContext.Response.Headers.Add("Accept-Ranges", "bytes");
                }
                else
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                }



                /*PartialContentStream partialStream = new PartialContentStream(_stream, _start.Value, _end.Value);
                StreamContent pushContent = new StreamContent(partialStream);
                */


                if (_isPart)
                {
                    context.HttpContext.Response.Headers.Add("Content-Range", string.Format(" bytes {0}-{1}/{2}", _start, _end, _size));
                }


                context.HttpContext.Response.ContentType = _contentType;

                if (_fileName != null)
                {
                    context.HttpContext.Response.Headers.Add("Content-disposition", $"attachment; filename = {WebUtility.UrlEncode(_fileName)}");
                }





                byte[] buffer = new byte[8192];

                while (true)
                {
                    var readResult = await _stream.ReadAsync(buffer, 0, 8192);

                    if (readResult == 0)
                    {
                        break;
                    }
                    else
                    {
                        await context.HttpContext.Response.Body.WriteAsync(buffer, 0, readResult);
                    }
                }



            }
            await Task.FromResult(0);
        }
    }
}
