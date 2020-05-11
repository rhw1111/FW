using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Oauth.DAL
{
    public class OauthEndpointStore : IOauthEndpointStore
    {
        private IOauthConnectionFactory _oauthConnectionFactory;

        public OauthEndpointStore(IOauthConnectionFactory oauthConnectionFactory)
        {
            _oauthConnectionFactory = oauthConnectionFactory;
        }
        public async Task Add(OauthEndpoint endpoint)
        {
            var conn=_oauthConnectionFactory.CreateOauthDBALL();
            await Task.FromResult(0);
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<OauthEndpoint>> Query(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<OauthEndpoint> QueryByClientID(string clientID)
        {
            throw new NotImplementedException();
        }

        public Task Update(OauthEndpoint endpoint)
        {
            throw new NotImplementedException();
        }
    }
}
