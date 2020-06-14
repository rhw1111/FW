using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.DI;

namespace MSLibrary.CommandLine.DAL
{

    [Injection(InterfaceType = typeof(ICommandLineEntityDBContextFactory), Scope = InjectionScope.Singleton)]
    public class CommandLineEntityDBContextFactory : ICommandLineEntityDBContextFactory
    {
        public CommandLineDBContext CreateCommandLineDBContext(DbConnection conn)
        {
            DbContextOptions<CommandLineDBContext> option = new DbContextOptions<CommandLineDBContext>();
            DbContextOptionsBuilder<CommandLineDBContext> optionBuilder = new DbContextOptionsBuilder<CommandLineDBContext>(option);
            CommandLineDBContext context = new CommandLineDBContext(optionBuilder.UseSqlServer(conn).Options);

            return context;
        }
    }
}
