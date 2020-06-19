using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.StreamingDB.DAL
{
    public interface IStreamingDBEntityDBContextFactory
    {
        StreamingDBDBContext CreateStreamingDBDBContext(DbConnection conn);
    }
}
