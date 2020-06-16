using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.CommandLine.DAL;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.DAL
{
    public class MainDBContextFactory : IMainDBContextFactory
    {
        public MainDBContext CreateMainDBContext(DbConnection conn)
        {
            DbContextOptions<MainDBContext> option = new DbContextOptions<MainDBContext>();
            DbContextOptionsBuilder<MainDBContext> optionBuilder = new DbContextOptionsBuilder<MainDBContext>(option);
            MainDBContext context = new MainDBContext(optionBuilder.UseMySql(conn).Options);

            return context;
        }
    }
}
