using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSLibrary.Compression
{
    /// <summary>
    /// 压缩服务
    /// </summary>
    public interface ICompressionService
    {
        /// <summary>
        /// 压缩文本数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        Task GetCompressionStream(Func<Stream, Task> action, params CompressionTextItemFileInfo[] items);
        /// <summary>
        /// 解压文本数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<CompressionTextItemFileInfo[]> GetUnCompressionFileInfo(Stream stream);
        /// <summary>
        /// 压缩文本数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        Task<string> GetCompressionText(string text);
        /// <summary>
        /// 解压文本数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<string> GetUnCompressionText(string compressionText);
    }

    /// <summary>
    /// 要压缩的文本数据项
    /// </summary>
    public class CompressionTextItemFileInfo
    {
        public string FileName { get; set; }
        public string Text { get; set; }
    }
}
