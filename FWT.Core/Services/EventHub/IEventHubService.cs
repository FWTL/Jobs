using System.Collections.Generic;
using System.Threading.Tasks;

namespace FWTL.Core.Services.EventHub
{
    public interface IEventHubService
    {
        Task SendAsync<TMessage>(List<TMessage> messages);
    }
}
