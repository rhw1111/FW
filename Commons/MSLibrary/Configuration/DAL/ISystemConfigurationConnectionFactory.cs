using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Configuration.DAL
{
    public interface ISystemConfigurationConnectionFactory
    {
        string CreateAllForSystemConfiguration();
        string CreateReadForSystemConfiguration();
    }
}
