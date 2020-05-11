using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace MSLibrary
{
    /// <summary>
    /// Claim的扩展方法
    /// </summary>
    public static class ClaimExtension
    {
        /// <summary>
        /// 获取Claim对象的二进制数据字符串
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static async Task<string> GetBinaryData(this Claim claim)
        {
            string strBase64;
            await using (var st = new MemoryStream())
            {
                await using (BinaryWriter w = new BinaryWriter(st))
                {
                    claim.WriteTo(w);
                    strBase64 = Convert.ToBase64String(st.ToArray());
                }
            }

            return strBase64;
        }
        /// <summary>
        /// 从二进制数据创建Claim
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<Claim> CreateFromBinaryData(string data)
        {
            Claim claim;
            await using (var st = new MemoryStream(Convert.FromBase64String(data)))
            {
                using (BinaryReader w = new BinaryReader(st))
                {
                    claim = new Claim(w);
                }
            }

            return claim;
        }
    }
}
