using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FWTL.Core.Services.Dapper;
using FWTL.Core.Sql;

namespace FWTL.Infrastructure.Dapper
{
    public class DapperConnector<TCredentials> : IDatabaseConnector<TCredentials> where TCredentials : IDatabaseCredentials
    {
        private readonly IDatabaseCredentials _databaseConnection;

        public DapperConnector(TCredentials databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public void Execute(Action<IDbConnection> data)
        {
            using (var connection = new SqlConnection(_databaseConnection.ConnectionString))
            {
                connection.Open();
                data(connection);
            }
        }

        public T Execute<T>(Func<IDbConnection, T> data)
        {
            using (var connection = new SqlConnection(_databaseConnection.ConnectionString))
            {
                connection.Open();
                return data(connection);
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> data)
        {
            using (var connection = new SqlConnection(_databaseConnection.ConnectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                return await data(connection).ConfigureAwait(false);
            }
        }

        public async Task ExecuteAsync(Func<IDbConnection, Task> data)
        {
            using (var connection = new SqlConnection(_databaseConnection.ConnectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                await data(connection).ConfigureAwait(false);
            }
        }
    }
}
