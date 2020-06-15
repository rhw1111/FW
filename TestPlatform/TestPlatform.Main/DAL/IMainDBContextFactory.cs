using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace FW.TestPlatform.Main.DAL
{
    public interface IMainDBContextFactory
    {
        MainDBContext CreateMainDBContext(DbConnection conn);
    }
}
