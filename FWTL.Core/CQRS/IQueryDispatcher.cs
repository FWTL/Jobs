using System.Threading.Tasks;

namespace FWTL.Core.CQRS
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}
