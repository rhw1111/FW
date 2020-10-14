using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace MSLibrary
{
    public static class StreamExtension
    {
        public static async Task<byte[]> ReadAll(this Stream stream,int buffSize)
        {
            stream.Position = 0;
            List<byte> result = new List<byte>();
            Memory<byte> buff = new Memory<byte>(new byte[buffSize]);
            while(true)
            {
                var len=await stream.ReadAsync(buff);
                result.AddRange(buff.Slice(0, len).ToArray());
                if (len< buffSize)
                {
                    break;
                }
            }

            return result.ToArray();
        }
    }
}
