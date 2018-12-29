using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FWTL.Core.CQRS;
using FWTL.Core.Entities;
using FWTL.Core.Entities.Maps;
using FWTL.Core.Extensions;
using FWTL.Core.Services.Dapper;
using FWTL.Database;
using FWTL.Infrastructure.Cache;
using FWTL.Infrastructure.Grid;
using FWTL.Infrastructure.Handlers;
using StackExchange.Redis;
using static Dapper.SqlMapper;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Api.Controllers.Jobs
{
    public class GetJobs
    {
        public class Cache : RedisJsonHandler<Query, PaginatedResults<Result>>
        {
            public Cache(IDatabase cache) : base(cache)
            {
                KeyFn = query =>
                {
                    return CacheKeyBuilder.Build<GetJobs, Query>(query, m => m.UserId, m => m.PaginationParams.Page, m => m.PaginationParams.PerPage);
                };
            }

            public override TimeSpan? Ttl(Query query)
            {
                return TimeSpan.FromMinutes(10);
            }
        }

        public class Handler : IQueryHandler<Query, PaginatedResults<Result>>
        {
            private readonly IDatabaseConnector<JobDatabaseCredentials> _database;
            private readonly IDatabase _cache;

            public Handler(IDatabaseConnector<JobDatabaseCredentials> database, IDatabase cache)
            {
                _database = database;
                _cache = cache;
            }

            public async Task<PaginatedResults<Result>> HandleAsync(Query query)
            {
                GridReader reader = await _database.ExecuteAsync(conn =>
                {
                    return conn.QueryMultipleAsync(
                        $"SELECT COUNT(1) FROM {JobMap.Table} " +
                        $"WHERE {JobMap.UserId} = @{JobMap.UserId}; " +
                        $"SELECT * FROM {JobMap.Table} " +
                        $"WHERE {JobMap.UserId} = @{JobMap.UserId} " +
                        $"ORDER BY {JobMap.Id} DESC {query.PaginationParams.ToSql()}", new { query.UserId });
                });
                int total = await reader.ReadFirstAsync<int>();
                IEnumerable<Job> jobs = await reader.ReadAsync<Job>();

                IEnumerable<Result> result = jobs.Select(job => new Result(job));

                result.Where(r => r.State == JobState.Fetching).ForEach(r =>
                {
                    RedisValue value = _cache.StringGet($"Messages.{nameof(Job)}.{nameof(Job.Id)}.{nameof(JobState.Fetching)}");
                    r.Message = value.HasValue ? value.ToString() : "Processing";
                });

                return new PaginatedResults<Result>(total, query.PaginationParams, null, result);
            }
        }

        public class Query : IQuery
        {
            public string UserId { get; set; }

            public PaginationParams PaginationParams { get; set; }
        }

        public class Result
        {
            public Result()
            {

            }

            public Result(Job job)
            {
                Id = job.Id;
                State = job.State;
                PeerId = job.PeerId;
                PeerType = job.PeerType;
            }

            public long Id { get; set; }

            public JobState State { get; set; }

            public int PeerId { get; set; }

            public PeerType PeerType { get; set; }

            public string Message { get; set; }
        }
    }
}