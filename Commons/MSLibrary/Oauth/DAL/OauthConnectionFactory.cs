using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Configuration;
using MSLibrary.Transaction;

namespace MSLibrary.Oauth.DAL
{
    public class OauthConnectionFactory : IOauthConnectionFactory
    {

        public string CreateOauthDBALL()
        {
            var coreConfiguration=ConfigurationContainer.Get<CoreConfiguration>();
            return coreConfiguration.Connections["oauthdball"];
        }

        public string CreateOauthDBRead()
        {
            var coreConfiguration = ConfigurationContainer.Get<CoreConfiguration>();

            if (DBAllScope.IsAll())
            {
                return coreConfiguration.Connections["oauthdball"];
            }
            else
            {
                return coreConfiguration.Connections["oauthdbread"];
            }
        }

    }
}
