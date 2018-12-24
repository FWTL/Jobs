using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FWTL.Core.Services.Dapper;
using FWTL.Core.Sql;

namespace FWTL.Infrastructure.Dapper
{
    public class ProfileDbConnection : DbConnection
    {
        private readonly DbConnection _connection;
        private DbCommand _command;

        public ProfileDbConnection(DbConnection connection)
        {
            _connection = connection;
        }

        public override string ConnectionString
        {
            get { return _connection.ConnectionString; }
            set { _connection.ConnectionString = value; }
        }

        public override string Database => _connection.Database;

        public override string DataSource => _connection.DataSource;

        public override string ServerVersion => _connection.ServerVersion;

        public override ConnectionState State => _connection.State;

        public override void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            _connection.Close();
        }

        public override void Open()
        {
            _connection.Open();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return _connection.BeginTransaction();
        }

        protected override DbCommand CreateDbCommand()
        {
            _command = _connection.CreateCommand();
            return _command;
        }
    }

    public class DapperConnector<TCredentials> : IDisposable, IDatabaseConnector<TCredentials> where TCredentials : IDatabaseCredentials
    {
        private readonly ProfileDbConnection _connection;

        public DapperConnector(TCredentials databaseConnection)
        {
            _connection = new ProfileDbConnection(new SqlConnection(databaseConnection.ConnectionString));
        }

        public void Execute(Action<IDbConnection> data)
        {
            Open(_connection);
            data(_connection);
        }

        public T Execute<T>(Func<IDbConnection, T> data)
        {
            Open(_connection);
            return data(_connection);
        }

        public async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> data)
        {
            await OpenAsync(_connection);
            try
            {
                return await data(_connection);
            }
            finally
            {

            }
        }

        private void Open(DbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        private async Task OpenAsync(DbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }
        }

        public async Task ExecuteAsync(Func<IDbConnection, Task> data)
        {
            await OpenAsync(_connection);
            await data(_connection);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}