using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Xrm.Token
{
    /// <summary>
    /// Crm服务令牌生成接口
    /// </summary>
    public interface ICrmServiceTokenGenerateService
    {
        Task<string> Genereate(Dictionary<string,object> parameters);
    }
}
