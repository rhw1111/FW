using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.CommandLine.SSH.DAL;
using MSLibrary.DI;

namespace MSLibrary.CommandLine.DAL
{
    [Injection(InterfaceType = typeof(ICommandLineConnectionFactory), Scope = InjectionScope.Singleton)]
    public class CommandLineConnectionFactory : ICommandLineConnectionFactory
    {
        public string CreateAllForCommandLine()
        {
            throw new NotImplementedException();
        }

        public string CreateReadForCommandLine()
        {
            throw new NotImplementedException();
        }
    }
}
