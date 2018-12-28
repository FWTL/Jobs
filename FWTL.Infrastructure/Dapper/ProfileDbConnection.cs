using System;
using System.Data;
using System.Data.Common;
using FWTL.Core.Extensions;
using Serilog;
using StackExchange.Profiling.Data;

namespace FWTL.Infrastructure.Dapper
{
    public class ProfileDbConnection : IDbProfiler
    {
        private readonly ILogger _logger;

        public ProfileDbConnection(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsActive => true;

        public void ExecuteFinish(IDbCommand profiledDbCommand, SqlExecuteType executeType, DbDataReader reader)
        {
        }

        public void ExecuteStart(IDbCommand profiledDbCommand, SqlExecuteType executeType)
        {
            _logger.Information(profiledDbCommand.ToSql());
        }

        public void OnError(IDbCommand profiledDbCommand, SqlExecuteType executeType, Exception exception)
        {
        }

        public void ReaderFinish(IDataReader reader)
        {
        }
    }
}