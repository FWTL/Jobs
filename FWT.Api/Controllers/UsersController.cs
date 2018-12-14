using FWTL.Core.CQRS;
using FWTL.Core.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace FWTL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly IQueryDispatcher _queryDispatcher;

        private readonly ICurrentUserProvider _userProvider;

        public UsersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        //[HttpGet]
        //[Route("/Me")]
        //[Authorize]
        //public async Task<GetMe.Result> GetMe()
        //{
        //    return await _queryDispatcher.DispatchAsync<GetMe.Query, GetMe.Result>(new GetMe.Query()
        //    {
        //        PhoneHashId = _userProvider.PhoneHashId(User)
        //    });
        //}
    }
}