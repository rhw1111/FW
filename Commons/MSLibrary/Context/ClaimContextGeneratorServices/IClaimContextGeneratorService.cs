using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MSLibrary.Context.ClaimContextGeneratorServices
{
    /// <summary>
    /// 基于声明生成上下文的服务接口
    /// </summary>
    public interface IClaimContextGeneratorService
    {
        /// <summary>
        /// 根据声明初始化上下文
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        void Do(IEnumerable<Claim> claims);
    }
}
