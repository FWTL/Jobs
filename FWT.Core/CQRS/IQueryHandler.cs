using System.Threading.Tasks;

namespace FWTL.Core.CQRS
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
