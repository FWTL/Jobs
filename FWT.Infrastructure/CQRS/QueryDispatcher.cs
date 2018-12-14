using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using FWTL.Core.CQRS;
using FWTL.Infrastructure.Validation;

namespace FWTL.Infrastructure.CQRS
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IComponentContext _context;

        public QueryDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery
        {
            IReadCacheHandler<TQuery, TResult> cache;
            if (_context.TryResolve(out cache))
            {
                TResult result = await cache.ReadAsync(query).ConfigureAwait(false);
                if (result != null)
                {
                    return result;
                }
            }

            AppAbstractValidation<TQuery> validator;
            if (_context.TryResolve(out validator))
            {
                var validationResult = validator.Validate(query);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var handler = _context.Resolve<IQueryHandler<TQuery, TResult>>();
            var queryResult = await handler.HandleAsync(query).ConfigureAwait(false);

            IWriteCacheHandler<TQuery, TResult> cache2;
            if (_context.TryResolve(out cache2))
            {
                await cache2.WriteAsync(query, queryResult).ConfigureAwait(false);
            }

            return queryResult;
        }
    }
}
