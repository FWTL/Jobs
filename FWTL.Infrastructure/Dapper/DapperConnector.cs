using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using FWTL.Core.Services.Dapper;
using FWTL.Core.Sql;

namespace FWTL.Infrastructure.Dapper
{
    public class DapperConnector<TCredentials> : IDisposable, IDatabaseConnector<TCredentials> where TCredentials : IDatabaseCredentials
    {
        private readonly DbConnection _databaseConnection;

        public DapperConnector(DbConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public void Execute(Action<IDbConnection> data)
        {
            Open(_databaseConnection);
            data(_databaseConnection);
        }

        public T Execute<T>(Func<IDbConnection, T> data)
        {
            Open(_databaseConnection);
            return data(_databaseConnection);
        }

        public async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> data)
        {
            await OpenAsync(_databaseConnection);
            return await data(_databaseConnection);
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
            await OpenAsync(_databaseConnection);
            await data(_databaseConnection);
        }

        public void Dispose()
        {
            _databaseConnection.Dispose();
        }
    }
}