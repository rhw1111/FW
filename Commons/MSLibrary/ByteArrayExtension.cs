using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MSLibrary
{
    /// <summary>
    /// 字节数组的扩展方法
    /// </summary>
    public static class ByteArrayExtension
    {
        public static byte[] ToEnflate(this byte[] bytes)
        {
            byte[] result = null;
            using (var ms = new MemoryStream(bytes) { Position = 0 })
            {
                using (var outms = new MemoryStream())
                {
                    using (var deflateStream = new DeflateStream(outms, CompressionMode.Compress, true))
                    {
                        var buf = new byte[1024];
                        int len;
                        while ((len = ms.Read(buf, 0, buf.Length)) > 0)
                            deflateStream.Write(buf, 0, len);
                    }
                    result= outms.ToArray();
                }
            }

            return result;
               
        }


        public static string ToTimeSpanString(this byte[] bytes)
        {
            return $"0x{BitConverter.ToString(bytes).Replace("-", "")}";
        }
    }
}
