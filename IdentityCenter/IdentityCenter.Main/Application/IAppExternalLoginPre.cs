using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityCenter.Main.DTOModel;

namespace IdentityCenter.Main.Application
{
    /// <summary>
    /// 第三方登录预处理
    /// </summary>
    public interface IAppExternalLoginPre
    {
        Task<ExternalLoginPreResult> Do(string schemeName, CancellationToken cancellationToken = default);
    }
}
