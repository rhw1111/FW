using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.CommandLine.DAL;
using MSLibrary.DI;

namespace MSLibrary.MySqlStore.DAL
{
    [Injection(InterfaceType = typeof(ICommandLineEntityDBContextFactory), Scope = InjectionScope.Singleton)]
    public class CommandLineEntityDBContextFactory : ICommandLineEntityDBContextFactory
    {
        public CommandLineDBContext CreateCommandLineDBContext(DbConnection conn)
        {
            DbContextOptions<CommandLineDBContext> option = new DbContextOptions<CommandLineDBContext>();
            DbContextOptionsBuilder<CommandLineDBContext> optionBuilder = new DbContextOptionsBuilder<CommandLineDBContext>(option);
            CommandLineDBContext context = new CommandLineDBContext(optionBuilder.UseMySql(conn).Options);

            return context;
        }
    }
}
