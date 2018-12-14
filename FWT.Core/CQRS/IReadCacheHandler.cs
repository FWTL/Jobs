using System.Threading.Tasks;

namespace FWTL.Core.CQRS
{
    public interface IReadCacheHandler<TQuery, TResult> : ICacheKey<TQuery>
    {
        Task<TResult> ReadAsync(TQuery query);
    }
}
