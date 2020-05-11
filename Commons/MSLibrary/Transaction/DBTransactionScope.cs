using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace MSLibrary.Transaction
{
    /*
    public class DBTransactionScope : IDisposable
    {
        
        private TransactionScope _scope;

        private static AsyncLocal<Dictionary<string, DbConnection>> _connections = new AsyncLocal<Dictionary<string, DbConnection>>();

        private Dictionary<string, DbConnection> _preConnections;

        private bool _needClose;

        public DBTransactionScope(TransactionScopeOption scopeOption,TransactionOptions transactionOptions)
        {
            _scope = new TransactionScope(scopeOption, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

            _preConnections = _connections.Value;

            //如果是范围是新事务或者忽略，则_needClose=true
            if (scopeOption == TransactionScopeOption.RequiresNew || scopeOption == TransactionScopeOption.Suppress)
            {
                _needClose = true;
                _connections.Value = new Dictionary<string, DbConnection>();
            }
            else if (scopeOption == TransactionScopeOption.Required && _preConnections == null)
            {
                //如果是Required，并且_preConnections为空，则_needClose=true
                _connections.Value = new Dictionary<string, DbConnection>();
                _needClose = true;
            }
            else
            {
                if (_connections.Value == null)
                {
                    _connections.Value = new Dictionary<string, DbConnection>();
                }
                _needClose = false;
            }
        }

        /// <summary>
        /// 获取当前连接集合
        /// </summary>
        public static Dictionary<string, DbConnection> CurrentConnections
        {
            get { return _connections.Value; }
        }

        /// <summary>
        /// 清除当前环境事务
        /// </summary>
        public static void ClearTransactionScope()
        {
            System.Transactions.Transaction.Current = null;
        }

        /// <summary>
        /// 检查当前环境是否处于事务中
        /// </summary>
        /// <returns></returns>
        public static bool InScope()
        {
            return !(System.Transactions.Transaction.Current == null);
        }

        public void Complete()
        {
            _scope.Complete();

            if (_needClose)
            {
                if (_connections.Value != null)
                {
                    foreach (var item in _connections.Value)
                    {
                     
                        item.Value.Close();
                    }
                }
            }
        }
        public void Dispose()
        {
            if (_needClose)
            {
                if (_connections.Value != null)
                {
                    foreach (var item in _connections.Value)
                    {
                        
                        try
                        {
                           
                            if (item.Value.State != ConnectionState.Closed)
                            {
                                item.Value.Close();
                            }
                        }
                        catch
                        {

                        }
                    }

                }

                _connections.Value = null;
            }

            _connections.Value = _preConnections;

            _scope.Dispose();
        }
    }
    */


    public class DBTransactionScope : IDisposable,IAsyncDisposable
    {

        private static AsyncLocal<bool> _inTransaction = new AsyncLocal<bool>();

        private static AsyncLocal<Dictionary<string, DBConnectionContainer>> _connections = new AsyncLocal<Dictionary<string, DBConnectionContainer>>();

        private static AsyncLocal<TransactionInfo> _transactionInfo = new AsyncLocal<TransactionInfo>();

        private static AsyncLocal<DBTransactionScope> _transactionScope = new AsyncLocal<DBTransactionScope>();

        private bool _needCreateNewTransaction;

        private Dictionary<string, DBConnectionContainer> _preConnections;

        private TransactionInfo _preTransactionInfo;



        private DBTransactionScope _preTransactionScope;

        private bool _needClose;

        private bool _needClearTransactionScope = false;

        private Guid _id;
        private TransactionScopeOption _scopeOption;
        private TransactionOptions _transactionOptions;

        private Action _rollbackAction;

        public DBTransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions, Action rollbackAction = null)
        {

            _preTransactionScope = _transactionScope.Value;
            _transactionScope.Value = this;

            _rollbackAction = rollbackAction;

            _preConnections = _connections.Value;
            _preTransactionInfo = _transactionInfo.Value;

            _transactionInfo.Value = new TransactionInfo() { ID = Guid.NewGuid(), ScopeOption = scopeOption, TransactionOptions = transactionOptions };
            _id = _transactionInfo.Value.ID;
            _scopeOption = _transactionInfo.Value.ScopeOption;
            _transactionOptions = _transactionInfo.Value.TransactionOptions;



            if (_preTransactionInfo == null)
            {
                _needCreateNewTransaction = true;
            }
            else
            {
                if (_preTransactionInfo.TransactionOptions.IsolationLevel != transactionOptions.IsolationLevel)
                {
                    _needCreateNewTransaction = true;
                }
                else
                {
                    _needCreateNewTransaction = false;
                }
            }

            if (!_inTransaction.Value)
            {
                _needClearTransactionScope = true;
            }

            _inTransaction.Value = true;

            //如果是范围是新事务或者忽略，则_needClose=true
            if (scopeOption == TransactionScopeOption.RequiresNew || scopeOption == TransactionScopeOption.Suppress)
            {
                _needClose = true;
                _connections.Value = new Dictionary<string, DBConnectionContainer>();
                if (scopeOption == TransactionScopeOption.Suppress)
                {
                    _inTransaction.Value = false;
                }

            }
            else if (scopeOption == TransactionScopeOption.Required && _preConnections == null)
            {
                //如果是Required，并且_preConnections为空，则_needClose=true
                _connections.Value = new Dictionary<string, DBConnectionContainer>();
                _needClose = true;
            }
            else
            {
                if (_connections.Value == null)
                {
                    _connections.Value = new Dictionary<string, DBConnectionContainer>();
                }
                _needClose = false;

            }
        }

        /// <summary>
        /// 获取当前连接集合
        /// </summary>
        public static Dictionary<string, DBConnectionContainer> CurrentConnections
        {
            get { return _connections.Value; }
        }


        /// <summary>
        /// 获取当前事务信息
        /// </summary>
        public static TransactionInfo CurrentTransactionInfo
        {
            get { return _transactionInfo.Value; }
        }


        /// <summary>
        /// 获取当前的事务范围
        /// </summary>
        public static DBTransactionScope CurrentScope
        {
            get { return _transactionScope.Value; }
        }

        /// <summary>
        /// 清除当前环境事务
        /// </summary>
        public static void ClearTransactionScope()
        {
            _inTransaction.Value = false;
        }

        /// <summary>
        /// 检查当前环境是否处于事务中
        /// </summary>
        /// <returns></returns>
        public static bool InScope()
        {
            return _inTransaction.Value;
        }


        public bool NeedClose
        {
            get
            {
                return _needClose;
            }
        }

        public DBTransactionScope PreScope
        {
            get
            {
                return _preTransactionScope;
            }
        }



        public Guid ID
        {
            get
            {
                return _id;
            }
        }


        public TransactionScopeOption ScopeOption
        {
            get
            {
                return _scopeOption;
            }
        }

        public TransactionOptions TransactionOptions
        {
            get
            {
                return _transactionOptions;
            }
        }

        /// <summary>
        /// 是否需要在连接上创建新的事务
        /// </summary>
        public bool NeedCreateNewTransaction
        {
            get { return _needCreateNewTransaction; }
        }


        public void Complete()
        {
            if (_connections.Value != null)
            {
                foreach (var item in _connections.Value)
                {
                    item.Value.Error = false;
                }

            }
        }
        public void Dispose()
        {
            try
            {
                bool partSubmit = false;
                try
                {
                    //提交或回滚属于该事务范围的事务
                    if (_connections.Value != null)
                    {
                        foreach (var item in _connections.Value)
                        {
                            if (item.Value.Transactions.TryGetValue(_transactionInfo.Value.ID, out DbTransaction transaction))
                            {
                                if (item.Value.Error)
                                {
                                    transaction.Rollback();
                                }
                                else
                                {
                                    transaction.Commit();
                                    partSubmit = true;
                                }

                            }

                        }

                    }
                }
                catch
                {
                    ///如果有部分被提交，则需要调用回滚函数
                    if (partSubmit)
                    {
                        if (_rollbackAction != null)
                        {
                            _rollbackAction();
                        }
                    }

                    throw;
                }
                finally
                {
                    //关闭连接
                    if (_needClose)
                    {
                        if (_connections.Value != null)
                        {
                            foreach (var item in _connections.Value)
                            {
                                try
                                {

                                    if (item.Value.Connection.State != ConnectionState.Closed)
                                    {
                                        item.Value.Connection.Close();
                                        item.Value.Connection.Dispose();
                                    }
                                }
                                catch
                                {

                                }

                            }

                        }

                        _connections.Value = null;
                    }
                }

                if (_needClearTransactionScope)
                {
                    _inTransaction.Value = false;
                }


                if (_connections.Value != null)
                {
                    foreach (var item in _connections.Value)
                    {

                        item.Value.Error = true;
                    }
                }
            }
            finally
            {
                _connections.Value = _preConnections;
                _transactionInfo.Value = _preTransactionInfo;
                _transactionScope.Value = _preTransactionScope;
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                bool partSubmit = false;
                try
                {
                    //提交或回滚属于该事务范围的事务
                    if (_connections.Value != null)
                    {
                        foreach (var item in _connections.Value)
                        {
                            if (item.Value.Transactions.TryGetValue(_transactionInfo.Value.ID, out DbTransaction transaction))
                            {
                                if (item.Value.Error)
                                {
                                    await transaction.RollbackAsync();
                                }
                                else
                                {
                                    await transaction.CommitAsync();
                                    partSubmit = true;
                                }

                            }

                        }

                    }
                }
                catch
                {
                    ///如果有部分被提交，则需要调用回滚函数
                    if (partSubmit)
                    {
                        if (_rollbackAction != null)
                        {
                            _rollbackAction();
                        }
                    }

                    throw;
                }
                finally
                {
                    //关闭连接
                    if (_needClose)
                    {
                        if (_connections.Value != null)
                        {
                            foreach (var item in _connections.Value)
                            {
                                try
                                {

                                    if (item.Value.Connection.State != ConnectionState.Closed)
                                    {
                                        await item.Value.Connection.CloseAsync();
                                        await item.Value.Connection.DisposeAsync();
                                    }
                                }
                                catch
                                {

                                }

                            }

                        }

                        _connections.Value = null;
                    }
                }

                if (_needClearTransactionScope)
                {
                    _inTransaction.Value = false;
                }


                if (_connections.Value != null)
                {
                    foreach (var item in _connections.Value)
                    {

                        item.Value.Error = true;
                    }
                }
            }
            finally
            {
                _connections.Value = _preConnections;
                _transactionInfo.Value = _preTransactionInfo;
                _transactionScope.Value = _preTransactionScope;
            }
        }

        public class DBConnectionContainer
        {
            private Dictionary<Guid, DbTransaction> _transactions = new Dictionary<Guid, DbTransaction>();
            private bool _error = true;
            public DbConnection Connection { get; set; }


            public bool Error
            {
                get
                {
                    return _error;
                }
                set
                {
                    _error = value;
                }
            }

            public Dictionary<Guid, DbTransaction> Transactions
            {
                get
                {
                    return _transactions;
                }
            }

            public DbTransaction CreateTransaction(Func<DbConnection, TransactionOptions, DbTransaction> createTransactionAction)
            {
                DbTransaction result = null;
                var scope = DBTransactionScope.CurrentScope;
                List<DBTransactionScope> scopeList = new List<DBTransactionScope>();

                if (scope != null)
                {
                    while (true)
                    {
                        scopeList.Insert(0, scope);
                        if (scope.PreScope == null || scope.NeedClose)
                        {
                            break;
                        }
                        else
                        {
                            scope = scope.PreScope;
                        }

                    }
                }

                for (var index = 0; index <= scopeList.Count - 1; index++)
                {
                    if (!_transactions.ContainsKey(scopeList[index].ID) && scopeList[index].NeedCreateNewTransaction)
                    {
                        _transactions[scopeList[index].ID] = createTransactionAction(Connection, scopeList[index].TransactionOptions);
                    }

                    if (_transactions.ContainsKey(scopeList[index].ID))
                    {
                        result = _transactions[scopeList[index].ID];
                    }
                }

                return result;
            }

            public async Task<DbTransaction> CreateTransactionAsync(Func<DbConnection, TransactionOptions, Task<DbTransaction>> createTransactionAction)
            {
                DbTransaction result = null;
                var scope = DBTransactionScope.CurrentScope;
                List<DBTransactionScope> scopeList = new List<DBTransactionScope>();

                if (scope != null)
                {
                    while (true)
                    {
                        scopeList.Insert(0, scope);
                        if (scope.PreScope == null || scope.NeedClose)
                        {
                            break;
                        }
                        else
                        {
                            scope = scope.PreScope;
                        }

                    }
                }

                for (var index = 0; index <= scopeList.Count - 1; index++)
                {
                    if (!_transactions.ContainsKey(scopeList[index].ID) && scopeList[index].NeedCreateNewTransaction)
                    {
                        _transactions[scopeList[index].ID] = await createTransactionAction(Connection, scopeList[index].TransactionOptions);
                    }

                    if (_transactions.ContainsKey(scopeList[index].ID))
                    {
                        result = _transactions[scopeList[index].ID];
                    }
                }

                return result;
            }


        }


        public class TransactionInfo
        {
            public Guid ID
            {
                get; set;
            }
            public TransactionScopeOption ScopeOption
            {
                get; set;
            }

            public TransactionOptions TransactionOptions
            {
                get; set;
            }
        }


    }


    public class DBTransactionScopeConnectionInfo
    {
        public DbConnection Connection { get; set; }
        public DbTransaction Transaction { get; set; }
    }

}
