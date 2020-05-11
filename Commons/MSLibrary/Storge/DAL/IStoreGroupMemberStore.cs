using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Storge.DAL
{
    public interface IStoreGroupMemberStore
    {
        Task<StoreGroupMember> QueryByName(Guid groupID,string memberName);
    }
}
