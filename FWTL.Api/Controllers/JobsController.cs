using System.Threading.Tasks;
using FWTL.Api.Controllers.Jobs;
using FWTL.Core.CQRS;
using FWTL.Core.Services.User;
using FWTL.Infrastructure.Grid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICurrentUserProvider _userProvider;

        public JobsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        [HttpPost]
        [Authorize]
        public async Task Job(int peerId, PeerType peerType)
        {
            await _commandDispatcher.DispatchAsync(new CreateJob.Command()
            {
                UserId = _userProvider.UserId(User),
                PeerId = peerId,
                PeerType = peerType
            });
        }


        [HttpGet]
        [Authorize]
        public async Task<PaginatedResults<GetJobs.Result>> GetJobs([FromQuery] PaginationParams paginationParams)
        {
            return await _queryDispatcher.DispatchAsync<GetJobs.Query, PaginatedResults<GetJobs.Result>>(new GetJobs.Query()
            {
                UserId = _userProvider.UserId(User),
                PaginationParams = paginationParams
            });
        }
    }
}