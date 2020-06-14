using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data;
using MySqlConnector;
using MySql.Data.MySqlClient;
using MSLibrary.Transaction;

namespace MSLibrary.MySqlStore.Transaction
{
    /// <summary>
    /// 针对MySql的数据库连接生成器
    /// </summary>
    public class DBConnGenerateForMySql : IDBConnGenerate
    {
        public DbConnection Generate(string strConn)
        {
            MySqlConnection conn = new MySqlConnection(strConn);
            return conn;
        }

        public DbTransaction GenerateTransaction(DbConnection conn, IsolationLevel isolationLevel)
        {
            return conn.BeginTransaction(isolationLevel);
        }

        public async Task<DbTransaction> GenerateTransactionAsync(DbConnection conn, IsolationLevel isolationLevel)
        {
            return await conn.BeginTransactionAsync(isolationLevel);
        }
    }
}
