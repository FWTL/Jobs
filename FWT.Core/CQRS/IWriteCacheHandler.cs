using System.Threading.Tasks;

namespace FWTL.Core.CQRS
{
    public interface IWriteCacheHandler<TQuery, TResult> : ICacheKey<TQuery>
    {
        Task WriteAsync(TQuery query, TResult result);
    }
}
