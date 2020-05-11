using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using MongoDB.Driver;
using MSLibrary.Transaction.DbConnections;
using System.Threading.Tasks;
using System.Threading;

namespace MSLibrary.Transaction.DBTransactions
{
    public class MongoDBTransaction : DbTransaction
    {
        private IClientSessionHandle _clientSessionHandle;
        private MongoDBConnection _mongoDBConnection;

        public MongoDBTransaction(IClientSessionHandle clientSessionHandle, MongoDBConnection mongoDBConnection)
        {
            _clientSessionHandle = clientSessionHandle;
            _mongoDBConnection = mongoDBConnection;
        }
        public override IsolationLevel IsolationLevel
        {
            get
            {
                return IsolationLevel.ReadUncommitted;
            }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return _mongoDBConnection;
            }
        }

        public override void Commit()
        {
            _clientSessionHandle.CommitTransaction();
        }

        public override void Rollback()
        {
            _clientSessionHandle.AbortTransaction();
        }

        public override async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _clientSessionHandle.CommitTransactionAsync();
        }

        public override async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _clientSessionHandle.AbortTransactionAsync();
        }

        public IClientSessionHandle Transaction
        {
            get
            {
                return _clientSessionHandle;
            }
        }
    }
}
