using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Azure.DAL
{
    /// <summary>
    /// 令牌凭据生成器数据操作
    /// </summary>
    public interface ITokenCredentialGeneratorStore
    {
        Task<TokenCredentialGenerator> QueryByName(string name);
    }
}
