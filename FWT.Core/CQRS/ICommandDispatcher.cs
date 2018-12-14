using System.Threading.Tasks;

namespace FWTL.Core.CQRS
{
    public interface ICommandDispatcher
    {
        Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand;

        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
