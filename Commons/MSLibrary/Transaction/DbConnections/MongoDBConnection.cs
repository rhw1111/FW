using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using MongoDB;
using MongoDB.Driver;
using System.Data;
using MSLibrary.Transaction.DBTransactions;
using System.Threading.Tasks;
using System.Threading;

namespace MSLibrary.Transaction.DbConnections
{
    /// <summary>
    /// 针对MongoDB的数据连接
    /// </summary>
    public class MongoDBConnection : DbConnection
    {
        private MongoClient _mongoClient;
        private string _conn;

        public MongoDBConnection(string conn)
        {
            _conn = conn;
            _mongoClient = new MongoClient(conn);
        }
        public override string ConnectionString
        {
            get
            {
                return _conn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string Database => throw new NotImplementedException();

        public override string DataSource => throw new NotImplementedException();

        public override string ServerVersion => throw new NotImplementedException();

        public override ConnectionState State
        {
            get
            {
                return ConnectionState.Open;
            }
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
        }

        public override void Open()
        {
        }

        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public override async Task CloseAsync()
        {
            await Task.CompletedTask;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            var session = _mongoClient.StartSession(new ClientSessionOptions() { DefaultTransactionOptions = new TransactionOptions(readConcern: ReadConcern.Snapshot, writeConcern: WriteConcern.WMajority) });
            session.StartTransaction();
            MongoDBTransaction mongoDBTransaction = new MongoDBTransaction(session, this);
            return mongoDBTransaction;
        }

        protected override DbCommand CreateDbCommand()
        {
            throw new NotImplementedException();
        }

        public MongoClient Client
        {
            get
            {
                return _mongoClient;
            }
        }
    }
}
