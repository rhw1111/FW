using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityCenter.Main.DTOModel;

namespace IdentityCenter.Main.Application
{
    /// <summary>
    /// 应用层刷新令牌
    /// </summary>
    public interface IAppOpenIDRefreshToken
    {
        Task<RefreshTokenModel> Do(string bindingName,string refreshToken, CancellationToken cancellationToken = default);
    }
}
