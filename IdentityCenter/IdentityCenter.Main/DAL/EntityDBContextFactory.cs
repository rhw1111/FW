using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace IdentityCenter.Main.DAL
{
    /// <summary>
    ///实体数据上下文静态工厂
    ///用来生成实体数据上下文
    /// </summary>
    public static class EntityDBContextFactory
    {
        public static MainDBContext CreateMainDBContext(DbConnection conn)
        {
            DbContextOptions<MainDBContext> option = new DbContextOptions<MainDBContext>();
            DbContextOptionsBuilder<MainDBContext> optionBuilder = new DbContextOptionsBuilder<MainDBContext>(option);
            MainDBContext context = new MainDBContext(optionBuilder.UseSqlServer(conn).Options);

            return context;
        }

        public static ConfigurationDBContext CreateConfigurationDBContext(DbConnection conn)
        {
            DbContextOptions<ConfigurationDBContext> option = new DbContextOptions<ConfigurationDBContext>();
            DbContextOptionsBuilder<ConfigurationDBContext> optionBuilder = new DbContextOptionsBuilder<ConfigurationDBContext>(option);
            ConfigurationDBContext context = new ConfigurationDBContext(optionBuilder.UseSqlServer(conn).Options);

            return context;
        }

        public static TemporaryDBContext CreateTemporaryDBContext(DbConnection conn)
        {
            DbContextOptions<TemporaryDBContext> option = new DbContextOptions<TemporaryDBContext>();
            DbContextOptionsBuilder<TemporaryDBContext> optionBuilder = new DbContextOptionsBuilder<TemporaryDBContext>(option);
            TemporaryDBContext context = new TemporaryDBContext(optionBuilder.UseSqlServer(conn).Options);

            return context;
        }

    }
}
