using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Configuration.DAL;
using MSLibrary.Collections.Hash.DAL;
using MSLibrary.Context.DAL;

namespace IdentityCenter.Main.DAL
{
    public interface IDBConnectionMainFactory:ISystemConfigurationConnectionFactory, IHashConnectionFactory,IContextConnectionFactory
    {
        string CreateAllForIdentityConfiguration();
        string CreateReadForIdentityConfiguration();
        string CreateAllForIdentityEntity();
        string CreateReadForIdentityEntity();
        string CreateAllForIdentityTemporary();
        string CreateReadForIdentityTemporary();
    }
}
