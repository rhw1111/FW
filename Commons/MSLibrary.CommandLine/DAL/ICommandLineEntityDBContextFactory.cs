using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.CommandLine.DAL
{
    public interface ICommandLineEntityDBContextFactory
    {
        CommandLineDBContext CreateCommandLineDBContext(DbConnection conn);
    }
}
