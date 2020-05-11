using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DAL;

namespace MSLibrary.Oauth.DAL
{
    public interface IOauthConnectionFactory:IDBConnectionFactory
    {
        string CreateOauthDBALL();
        string CreateOauthDBRead();
    }
}
