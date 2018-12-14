using System.Threading.Tasks;

namespace FWTL.Core.CQRS
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
