using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Transactions;
using MSLibrary.DI;
using MSLibrary.Transaction.DbConnections;
using MSLibrary.Transaction.DBTransactions;

namespace MSLibrary.Transaction
{
    /*public static class DBTransactionHelper
    {
        /// <summary>
        /// 连接生成器列表
        /// </summary>
        private static Dictionary<string,IDBConnGenerate> _dbConnGenerates = new Dictionary<string, IDBConnGenerate>();

        /// <summary>
        /// 默认添加SqlServer的连接生成器
        /// </summary>
        static DBTransactionHelper()
        {
            _dbConnGenerates.Add(DBTypes.SqlServer, new DBConnGenerateForSql());
        }
        /// <summary>
        /// 提供静态属性可以替换连接生成器
        /// </summary>
        public static Dictionary<string, IDBConnGenerate> DBConnGenerates
        {
            get
            {
                return _dbConnGenerates;
            }
        }
        /// <summary>
        /// SQL事务处理封装
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="isOnlyRead">是否是只读</param>
        /// <param name="isInnerTranaction">当不在事务环境中时，是否需要开启事务</param>
        /// <param name="strConn">连接字符串</param>
        /// <param name="callBack">处理回调函数</param>
        public static void SqlTransactionWork(string dbType,bool isOnlyRead, bool isInnerTranaction, string strConn, Action<DbConnection> callBack)
        {
            if (!_dbConnGenerates.TryGetValue(dbType,out IDBConnGenerate dbConnGenerate))
            {
                throw new Exception($"not found {dbType} in DBTransactionHelper.DBConnGenerates");
            }

            DbConnection conn = null;
            //如果当前处于事务环境中
            if (DBTransactionScope.InScope())
            {
                //检查是否已经在事务中创建过连接
                if (DBTransactionScope.CurrentConnections.ContainsKey(strConn) && (DBTransactionScope.CurrentConnections[strConn].State == ConnectionState.Connecting || DBTransactionScope.CurrentConnections[strConn].State == ConnectionState.Open))
                {
                    conn = DBTransactionScope.CurrentConnections[strConn];
                    callBack(conn);
                }
                else
                {
                    //只读连接在事务中的时候需要做隔离，防止提升到分布式事务
                    if (isOnlyRead)
                    {
                        using (var transactionScope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
                        {
                            using (conn = dbConnGenerate.Generate(strConn))
                            {
                                conn.Open();
                                callBack(conn);
                                conn.Close();
                            }

                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        //如果是写连接，需要加入到事务连接列表中
                        conn = dbConnGenerate.Generate(strConn);
                        conn.Open();
                        DBTransactionScope.CurrentConnections[strConn] = conn;
                        callBack(conn);
                    }
                }
            }
            else
            {
                //不在事务中，需要创建连接，由isInnerTranaction确定是否需要创建事务
                if (isInnerTranaction)
                {
                    using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                    //using (var transactionScope = new TransactionScope())
                    {
                        using (conn = dbConnGenerate.Generate(strConn))
                        {
                            conn.Open();
                            callBack(conn);
                            conn.Close();
                        }

                        transactionScope.Complete();
                    }
                }
                else
                {
                    using (conn = dbConnGenerate.Generate(strConn))
                    {
                        conn.Open();
                        callBack(conn);
                        conn.Close();
                    }
                }

            }
        }


        /// <summary>
        /// SQL事务处理封装
        /// </summary>
        /// <param name="isOnlyRead">是否是只读</param>
        /// <param name="isInnerTranaction">当不在事务环境中时，是否需要开启事务</param>
        /// <param name="strConn">连接字符串</param>
        /// <param name="callBack">处理回调函数</param>
        public static async Task SqlTransactionWorkAsync(string dbType,bool isOnlyRead, bool isInnerTranaction, string strConn, Func<DbConnection, Task> callBack)
        {
            if (!_dbConnGenerates.TryGetValue(dbType, out IDBConnGenerate dbConnGenerate))
            {
                throw new Exception($"not found {dbType} in DBTransactionHelper.DBConnGenerates");
            }

            DbConnection conn = null;
            //如果当前处于事务环境中
            if (DBTransactionScope.InScope())
            {
                //检查是否已经在事务中创建过连接
                if (DBTransactionScope.CurrentConnections.ContainsKey(strConn) && (DBTransactionScope.CurrentConnections[strConn].State == ConnectionState.Connecting || DBTransactionScope.CurrentConnections[strConn].State == ConnectionState.Open))
                {
                    conn = DBTransactionScope.CurrentConnections[strConn];
                    await callBack(conn);
                }
                else
                {
                    //只读连接在事务中的时候需要做隔离，防止提升到分布式事务
                    if (isOnlyRead)
                    {
                        using (var transactionScope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
                        {
                            using (conn = dbConnGenerate.Generate(strConn))
                            {
                                await conn.OpenAsync();
                                await callBack(conn);
                                conn.Close();
                            }

                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        //如果是写连接，需要加入到事务连接列表中
                        conn = dbConnGenerate.Generate(strConn);
                        await conn.OpenAsync();
                        DBTransactionScope.CurrentConnections[strConn] = conn;
                        await callBack(conn);
                    }
                }
            }
            else
            {
                //不在事务中，需要创建连接，由isInnerTranaction确定是否需要创建事务
                if (isInnerTranaction)
                {
                    using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                    {
                        using (conn = dbConnGenerate.Generate(strConn))
                        {
                            await conn.OpenAsync();
                            await callBack(conn);
                            conn.Close();
                        }

                        transactionScope.Complete();
                    }
                }
                else
                {
                    using (conn = dbConnGenerate.Generate(strConn))
                    {
                        await conn.OpenAsync();
                        await callBack(conn);
                        conn.Close();
                    }
                }

            }
        }
    }
    */


    public static class DBTransactionHelper
    {
        /// <summary>
        /// 连接生成器列表
        /// </summary>
        private static Dictionary<string, IDBConnGenerate> _dbConnGenerates = new Dictionary<string, IDBConnGenerate>();

        /// <summary>
        /// 默认添加SqlServer的连接生成器
        /// </summary>
        static DBTransactionHelper()
        {
            _dbConnGenerates.Add(DBTypes.SqlServer, new DBConnGenerateForSql());
            _dbConnGenerates.Add(DBTypes.MongoDB, new DBConnGenerateForMongoDB());
        }
        /// <summary>
        /// 提供静态属性可以替换连接生成器
        /// </summary>
        public static Dictionary<string, IDBConnGenerate> DBConnGenerates
        {
            get
            {
                return _dbConnGenerates;
            }
        }
        /// <summary>
        /// SQL事务处理封装
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="isOnlyRead">是否是只读</param>
        /// <param name="isInnerTranaction">当不在事务环境中时，是否需要开启事务</param>
        /// <param name="strConn">连接字符串</param>
        /// <param name="callBack">处理回调函数</param>
        public static void SqlTransactionWork(string dbType, bool isOnlyRead, bool isInnerTranaction, string strConn, Action<DbConnection,DbTransaction> callBack,System.Transactions.IsolationLevel innerTransactionIsolationLevel= System.Transactions.IsolationLevel.ReadCommitted)
        {
            if (!_dbConnGenerates.TryGetValue(dbType, out IDBConnGenerate dbConnGenerate))
            {
                throw new Exception($"not found {dbType} in DBTransactionHelper.DBConnGenerates");
            }

            DbConnection conn = null;
            DbTransaction transaction = null;
            //如果当前处于事务环境中
            if (DBTransactionScope.InScope())
            {


                //检查是否已经在事务中创建过连接
                if (DBTransactionScope.CurrentConnections.ContainsKey(strConn) && (DBTransactionScope.CurrentConnections[strConn].Connection.State == ConnectionState.Connecting || DBTransactionScope.CurrentConnections[strConn].Connection.State == ConnectionState.Open))
                {
                    conn = DBTransactionScope.CurrentConnections[strConn].Connection;

                    transaction= DBTransactionScope.CurrentConnections[strConn].CreateTransaction((connection, options) =>
                    {
                        return CreateTransaction(dbConnGenerate, connection, options);
                    });

                    callBack(conn, transaction);
                }
                else
                {
                    //只读连接在事务中的时候需要做隔离，防止提升到分布式事务
                    if (isOnlyRead)
                    {
                        //using (var transactionScope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
                        //{
                        using (conn = dbConnGenerate.Generate(strConn))
                        {
                            conn.Open();
                           
                            //判断IsolationLevel是否是ReadUncommitted,如果是，则需要单独创建事务
                            if (DBTransactionScope.CurrentTransactionInfo.TransactionOptions.IsolationLevel== System.Transactions.IsolationLevel.ReadUncommitted)
                            {
                                transaction = dbConnGenerate.GenerateTransaction(conn, ConvertIsolationLevel(DBTransactionScope.CurrentTransactionInfo.TransactionOptions.IsolationLevel));
                            }
                            callBack(conn, transaction);
                            conn.Close();
                        }

                        //transactionScope.Complete();
                        //}
                    }
                    else
                    {
                        //如果是写连接，需要加入到事务连接列表中
                        conn = dbConnGenerate.Generate(strConn);

                        conn.Open();
  

                        DBTransactionScope.CurrentConnections[strConn] = new DBTransactionScope.DBConnectionContainer() { Connection = conn, Error = true };

                        transaction = DBTransactionScope.CurrentConnections[strConn].CreateTransaction((connection, options) =>
                        {
                            return CreateTransaction(dbConnGenerate, connection, options);
                        });

                        callBack(conn, transaction);
                    }
                }
            }
            else
            {
                //不在事务中，需要创建连接，由isInnerTranaction确定是否需要创建事务
                if (isInnerTranaction)
                {
                    //using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))

                    //{
                    using (conn = dbConnGenerate.Generate(strConn))
                    {
                        conn.Open();
                        transaction = dbConnGenerate.GenerateTransaction(conn, ConvertIsolationLevel(innerTransactionIsolationLevel));

                        callBack(conn, transaction);
                        conn.Close();
                    }

                    //    transactionScope.Complete();
                    //}
                }
                else
                {
                    using (conn = dbConnGenerate.Generate(strConn))
                    {
                        conn.Open();
                        callBack(conn,null);
                        conn.Close();
                    }
                }

            }
        }


        /// <summary>
        /// SQL事务处理封装
        /// </summary>
        /// <param name="isOnlyRead">是否是只读</param>
        /// <param name="isInnerTranaction">当不在事务环境中时，是否需要开启事务</param>
        /// <param name="strConn">连接字符串</param>
        /// <param name="callBack">处理回调函数</param>
        public static async Task SqlTransactionWorkAsync(string dbType, bool isOnlyRead, bool isInnerTranaction, string strConn, Func<DbConnection,DbTransaction, Task> callBack, System.Transactions.IsolationLevel innerTransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted)
        {
            if (!_dbConnGenerates.TryGetValue(dbType, out IDBConnGenerate dbConnGenerate))
            {
                throw new Exception($"not found {dbType} in DBTransactionHelper.DBConnGenerates");
            }

            DbConnection conn = null;
            DbTransaction transaction = null;
            //如果当前处于事务环境中
            if (DBTransactionScope.InScope())
            {

                //检查是否已经在事务中创建过连接
                if (DBTransactionScope.CurrentConnections.ContainsKey(strConn.ToLower()) && (DBTransactionScope.CurrentConnections[strConn.ToLower()].Connection.State == ConnectionState.Connecting || DBTransactionScope.CurrentConnections[strConn.ToLower()].Connection.State == ConnectionState.Open))
                {
                    conn = DBTransactionScope.CurrentConnections[strConn.ToLower()].Connection;
                    transaction = await DBTransactionScope.CurrentConnections[strConn.ToLower()].CreateTransactionAsync(async (connection, options) =>
                    {
                        return await CreateTransactionAsync(dbConnGenerate, connection, options);
                    });
                    
                    await callBack(conn, transaction);
                }
                else
                {
                    //只读连接在事务中的时候需要做隔离，防止提升到分布式事务
                    if (isOnlyRead)
                    {
                        //using (var transactionScope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
                        //{
                        await using (conn = dbConnGenerate.Generate(strConn))
                        {
                            await conn.OpenAsync();
                            //判断IsolationLevel是否是ReadUncommitted,如果是，则需要单独创建事务
                            if (DBTransactionScope.CurrentTransactionInfo.TransactionOptions.IsolationLevel == System.Transactions.IsolationLevel.ReadUncommitted)
                            {
                                transaction =await dbConnGenerate.GenerateTransactionAsync(conn, ConvertIsolationLevel(DBTransactionScope.CurrentTransactionInfo.TransactionOptions.IsolationLevel));
                            }
                            else
                            {
                                transaction = null;
                            }
                            await callBack(conn, transaction);
                            if (transaction != null)
                            {
                                await transaction.CommitAsync();
                            }
                            await conn.CloseAsync();
                        }

                        //    transactionScope.Complete();
                        //}
                    }
                    else
                    {
                        //如果是写连接，需要加入到事务连接列表中
                        conn = dbConnGenerate.Generate(strConn);
                        await conn.OpenAsync();

                        DBTransactionScope.CurrentConnections[strConn.ToLower()] = new DBTransactionScope.DBConnectionContainer() { Connection = conn, Error = true };
                        transaction =await DBTransactionScope.CurrentConnections[strConn.ToLower()].CreateTransactionAsync(async (connection, options) =>
                        {
                            return await CreateTransactionAsync(dbConnGenerate, connection, options);
                        });

                        await callBack(conn, transaction);
                    }
                }
            }
            else
            {
                //不在事务中，需要创建连接，由isInnerTranaction确定是否需要创建事务
                if (isInnerTranaction)
                {
                    //using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                    //{
                    await using (conn = dbConnGenerate.Generate(strConn))
                    {
                        await conn.OpenAsync();
                        transaction =await dbConnGenerate.GenerateTransactionAsync(conn, ConvertIsolationLevel(innerTransactionIsolationLevel));
                        await callBack(conn, transaction);

                        await transaction.CommitAsync();
                        await conn.CloseAsync();
                    }

                    //    transactionScope.Complete();
                    //}
                }
                else
                {
                    await using (conn = dbConnGenerate.Generate(strConn))
                    {
                        await conn.OpenAsync();
                        await callBack(conn,null);
                        await conn.CloseAsync ();
                    }
                }

            }
        }





        public static System.Data.IsolationLevel ConvertIsolationLevel(System.Transactions.IsolationLevel level)
        {
            System.Data.IsolationLevel result;
            switch (level)
            {
                case System.Transactions.IsolationLevel.ReadCommitted:
                    result = System.Data.IsolationLevel.ReadCommitted;
                    break;
                case System.Transactions.IsolationLevel.Chaos:
                    result = System.Data.IsolationLevel.Chaos;
                    break;
                case System.Transactions.IsolationLevel.ReadUncommitted:
                    result = System.Data.IsolationLevel.ReadUncommitted;
                    break;
                case System.Transactions.IsolationLevel.RepeatableRead:
                    result = System.Data.IsolationLevel.RepeatableRead;
                    break;
                case System.Transactions.IsolationLevel.Serializable:
                    result = System.Data.IsolationLevel.Serializable;
                    break;
                case System.Transactions.IsolationLevel.Snapshot:
                    result = System.Data.IsolationLevel.Snapshot;
                    break;
                case System.Transactions.IsolationLevel.Unspecified:
                    result = System.Data.IsolationLevel.Unspecified;
                    break;
                default:
                    result = System.Data.IsolationLevel.ReadCommitted;
                    break;
            }

            return result;
        }

        public static DbTransaction CreateTransaction(IDBConnGenerate dbConnGenerate, DbConnection connection,TransactionOptions options)
        {
            return dbConnGenerate.GenerateTransaction(connection, ConvertIsolationLevel(options.IsolationLevel));
        }

        public static async Task<DbTransaction> CreateTransactionAsync(IDBConnGenerate dbConnGenerate, DbConnection connection, TransactionOptions options)
        {
            return await dbConnGenerate.GenerateTransactionAsync(connection, ConvertIsolationLevel(options.IsolationLevel));
        }

    }

    /// <summary>
    /// 连接生成接口
    /// </summary>
    public interface IDBConnGenerate
    {
        /// <summary>
        /// 根据连接字符串创建连接
        /// </summary>
        /// <param name="strConn">连接字符串</param>
        /// <returns></returns>
        DbConnection Generate(string strConn);
        /// <summary>
        /// 根据连接创建事务
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        DbTransaction GenerateTransaction(DbConnection conn,System.Data.IsolationLevel isolationLevel);
        /// <summary>
        /// 根据连接创建事务(异步)
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        Task<DbTransaction> GenerateTransactionAsync(DbConnection conn, System.Data.IsolationLevel isolationLevel);
    }

    /// <summary>
    /// 实现针对SqlServer的连接创建
    /// </summary>
    [Injection(InterfaceType = typeof(DBConnGenerateForSql), Scope = InjectionScope.Singleton)]
    public class DBConnGenerateForSql : IDBConnGenerate
    {
        public DbConnection Generate(string strConn)
        {
            return new SqlConnection(strConn);
        }

        public DbTransaction GenerateTransaction(DbConnection conn, System.Data.IsolationLevel isolationLevel)
        {
            return ((SqlConnection)conn).BeginTransaction(isolationLevel);
        }

        public async Task<DbTransaction> GenerateTransactionAsync(DbConnection conn, System.Data.IsolationLevel isolationLevel)
        {
            return await ((SqlConnection)conn).BeginTransactionAsync(isolationLevel);
        }
    }

    /// <summary>
    /// 实现针对MongoDB的连接创建
    /// </summary>
    [Injection(InterfaceType = typeof(DBConnGenerateForMongoDB), Scope = InjectionScope.Singleton)]
    public class DBConnGenerateForMongoDB : IDBConnGenerate
    {
        public DbConnection Generate(string strConn)
        {
            return new MongoDBConnection(strConn);
        }

        public DbTransaction GenerateTransaction(DbConnection conn, System.Data.IsolationLevel isolationLevel)
        {
            return conn.BeginTransaction();
        }

        public async Task<DbTransaction> GenerateTransactionAsync(DbConnection conn, System.Data.IsolationLevel isolationLevel)
        {
            return await conn.BeginTransactionAsync();
        }
    }
}
