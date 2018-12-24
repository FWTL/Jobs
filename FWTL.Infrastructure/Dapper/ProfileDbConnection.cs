using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Serilog;

namespace FWTL.Infrastructure.Dapper
{
    public class ProfileDbConnection : DbConnection
    {
        private readonly DbConnection _connection;
        private readonly ILogger _logger;
        private DbCommand _command;

        public ProfileDbConnection(DbConnection connection, ILogger logger)
        {
            _connection = connection;
            _logger = logger;
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
            _command.Disposed += _command_Disposed;
            return _command;
        }

        private void _command_Disposed(object sender, EventArgs e)
        {
            var dbCommand = sender as SqlCommand;
            _logger.Information(dbCommand.CommandText);
        }
    }
}