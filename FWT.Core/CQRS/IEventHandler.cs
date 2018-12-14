using System.Threading.Tasks;

namespace FWTL.Core.CQRS
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task ExecuteAsync(TEvent @event);
    }
}
