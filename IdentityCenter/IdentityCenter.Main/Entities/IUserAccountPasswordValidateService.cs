using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Entities
{
    /// <summary>
    /// 用户账号密码验证
    /// </summary>
    public interface IUserAccountPasswordValidateService
    {
        Task<UserAccount> Validate(string name, string password, CancellationToken cancellationToken = default);
    }
}
