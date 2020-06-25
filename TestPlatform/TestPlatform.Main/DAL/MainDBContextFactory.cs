using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.CommandLine.DAL;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.DAL
{
    [Injection(InterfaceType = typeof(IMainDBContextFactory), Scope = InjectionScope.Singleton)]
    public class MainDBContextFactory : IMainDBContextFactory
    {
        public ConfigurationDBContext CreateConfigurationDBContext(DbConnection conn)
        {
            DbContextOptions<ConfigurationDBContext> option = new DbContextOptions<ConfigurationDBContext>();
            DbContextOptionsBuilder<ConfigurationDBContext> optionBuilder = new DbContextOptionsBuilder<ConfigurationDBContext>(option);
            ConfigurationDBContext context = new ConfigurationDBContext(optionBuilder.UseMySql(conn).Options);

            return context;
        }

        public MainDBContext CreateMainDBContext(DbConnection conn)
        {
            DbContextOptions<MainDBContext> option = new DbContextOptions<MainDBContext>();
            DbContextOptionsBuilder<MainDBContext> optionBuilder = new DbContextOptionsBuilder<MainDBContext>(option);
            MainDBContext context = new MainDBContext(optionBuilder.UseMySql(conn).Options);

            return context;
        }
    }
}
