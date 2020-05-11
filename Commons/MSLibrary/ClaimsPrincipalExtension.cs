using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace MSLibrary
{
    public static class ClaimsPrincipalExtension
    {
        public static ClaimsPrincipal CreateMergeIdentity(this ClaimsPrincipal mainPrincipal, ClaimsPrincipal principal)
        {
            ClaimsIdentity identity = null;

            if (mainPrincipal.Identity != null)
            {
                identity = new ClaimsIdentity(mainPrincipal.Identity.AuthenticationType);
            }
            else
            {
                identity = new ClaimsIdentity(principal.Identity.AuthenticationType);
            }

            Dictionary<string, Claim> claims = new Dictionary<string, Claim>();

            foreach (var item in principal.Claims)
            {
                claims[item.Type] = item;
            }

            foreach (var item in mainPrincipal.Claims)
            {
                claims[item.Type] = item;
            }



            identity.AddClaims(claims.Values);

            return new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// 获取ClaimsPrincipal对象的二进制数据字符串
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static async Task<string> GetBinaryData(this ClaimsPrincipal principal)
        {
            string strBase64;
            await using (var st = new MemoryStream())
            {
                await using (BinaryWriter w = new BinaryWriter(st))
                {
                    principal.WriteTo(w);
                    strBase64 = Convert.ToBase64String(st.ToArray());
                }
            }

            return strBase64;
        }
        /// <summary>
        /// 从二进制数据创建ClaimsPrincipal
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<ClaimsPrincipal> CreateFromBinaryData(string data)
        {
            ClaimsPrincipal principal;
            await using (var st = new MemoryStream(Convert.FromBase64String(data)))
            {
                using (BinaryReader w = new BinaryReader(st))
                {
                    principal = new ClaimsPrincipal(w);
                }
            }

            return principal;
        }
    }
}
