using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Oauth.DAL
{
    /// <summary>
    /// Oauth终结点的数据存储
    /// </summary>
    public interface IOauthEndpointStore
    {
        Task Add(OauthEndpoint endpoint);
        Task Update(OauthEndpoint endpoint);
        Task Delete(Guid id);
        Task<OauthEndpoint> QueryByClientID(string clientID);
        Task<QueryResult<OauthEndpoint>> Query(int page,int size);
    }
}
