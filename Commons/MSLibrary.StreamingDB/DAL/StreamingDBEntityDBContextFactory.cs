using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.DI;

namespace MSLibrary.StreamingDB.DAL
{
    [Injection(InterfaceType = typeof(IStreamingDBEntityDBContextFactory), Scope = InjectionScope.Singleton)]
    public class StreamingDBEntityDBContextFactory : IStreamingDBEntityDBContextFactory
    {
        public StreamingDBDBContext CreateStreamingDBDBContext(DbConnection conn)
        {
            DbContextOptions<StreamingDBDBContext> option = new DbContextOptions<StreamingDBDBContext>();
            DbContextOptionsBuilder<StreamingDBDBContext> optionBuilder = new DbContextOptionsBuilder<StreamingDBDBContext>(option);
            StreamingDBDBContext context = new StreamingDBDBContext(optionBuilder.UseSqlServer(conn).Options);

            return context;
        }
    }
}
