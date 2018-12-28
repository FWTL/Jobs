using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using FluentValidation;
using FWTL.Core.CQRS;
using FWTL.Core.Entities;
using FWTL.Core.Entities.Maps;
using FWTL.Core.Extensions;
using FWTL.Core.Services.Dapper;
using FWTL.Database;
using FWTL.Infrastructure.Validation;
using NodaTime;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Api.Controllers.Jobs
{
    public class CreateJob
    {
        public class Command : ICommand
        {
            public string UserId { get; set; }

            public int PeerId { get; set; }

            public PeerType PeerType { get; set; }
        }

        public class Handler : ICommandHandler<Command>
        {
            private readonly IDatabaseConnector<JobDatabaseCredentials> _database;
            private readonly IClock _clock;

            public Handler(IDatabaseConnector<JobDatabaseCredentials> database, IClock clock)
            {
                _database = database;
                _clock = clock;
            }

            public async Task ExecuteAsync(Command command)
            {
                await _database.ExecuteAsync(conn =>
                {
                    return conn.InsertAsync(new Job()
                    {
                        PeerId = command.PeerId,
                        PeerType = command.PeerType,
                        UserId = command.UserId,
                        State = JobState.Queued,
                        CreateDateUtc = _clock.UtcNow()
                    });
                });
            }
        }

        public class Validator : AppAbstractValidation<Command>
        {
            private readonly IDatabaseConnector<JobDatabaseCredentials> _database;

            public Validator(IDatabaseConnector<JobDatabaseCredentials> database)
            {
                _database = database;

                RuleFor(x => x.PeerType).IsInEnum();
                RuleFor(x => x).CustomAsync(async (command, context, token) =>
                {
                    int queuedJobs = await _database.ExecuteAsync(conn =>
                    {
                        return conn.ExecuteScalarAsync<int>($"SELECT COUNT(1) FROM {JobMap.Table} WHERE {JobMap.UserId} = @{JobMap.UserId} AND {JobMap.State} = {(int)JobState.Queued}", new { command.UserId });
                    });

                    if (queuedJobs >= 5)
                    {
                        context.AddFailure("command", "Unable to queue another job");
                    }

                    bool isAlreadyProccessed = await _database.ExecuteAsync(conn =>
                    {
                        return conn.ExecuteScalarAsync<bool>($@"
                        SELECT 1 FROM {JobMap.Table}
                        WHERE {JobMap.UserId} = @{JobMap.UserId}
                        AND {JobMap.PeerId} = @{JobMap.PeerId}
                        AND {JobMap.PeerType} = @{JobMap.PeerType}
                        AND {JobMap.State} IN ( {(int)JobState.Queued}, {(int)JobState.Fetching} )", new { command.UserId, command.PeerId, command.PeerType });
                    });

                    if (isAlreadyProccessed)
                    {
                        context.AddFailure("command", "This peer is in a queue already or is beeing proccessed");
                    }
                });
            }
        }
    }
}