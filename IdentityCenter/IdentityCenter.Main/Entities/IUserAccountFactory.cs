using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Entities
{
    public interface IUserAccountFactory
    {
        Task<UserAccount> Create(string serializeData);
    }
}
