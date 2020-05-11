using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken.DAL
{
    /// <summary>
    /// 第三方系统令牌记录数据操作
    /// </summary>
    public interface IThirdPartySystemTokenRecordStore
    {
        Task Add(ThirdPartySystemTokenRecord record);
        Task Update(ThirdPartySystemTokenRecord record);
        Task Delete(string userKey,Guid id);
        Task UpdateToken(string userKey, Guid id,string token);
        Task<ThirdPartySystemTokenRecord> QueryByID(string userKey, Guid id);
        Task<ThirdPartySystemTokenRecord> QueryByUserKey(string userKey,Guid loginEndpointId,Guid authEndpointId);
        Task<QueryResult<ThirdPartySystemTokenRecord>> QueryByUserKeyPage(string userKey,int page,int pageSize);

    }
}
