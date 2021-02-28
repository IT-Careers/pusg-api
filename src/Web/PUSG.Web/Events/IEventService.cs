using PUSG.Web.Events.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUSG.Web.Events
{
    public interface IEventService
    {
        IEnumerable<IEventHandler> EventHandlers { get; }

        void Subscribe(IEventHandler eventHandler);

        Task Publish(string eventName, params object[] args);
    }
}
