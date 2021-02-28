using PUSG.Web.Events.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUSG.Web.Events
{
    public class EventService : IEventService
    {
        private List<IEventHandler> eventHandlers;

        public EventService()
        {
            this.eventHandlers = new List<IEventHandler>();
        }
        
        private void DisposeUnavailable()
        {
            this.eventHandlers.RemoveAll(eventHandler => !eventHandler.IsAvailable());
        }

        public IEnumerable<IEventHandler> EventHandlers => this.eventHandlers;

        public async Task Publish(string eventName, params object[] args)
        {
            List<IEventHandler> availableHandlers = this.EventHandlers.Where(eventHandler => eventHandler.IsAvailable()).ToList();

            foreach (var eventHandler in availableHandlers)
            {
                await eventHandler.HandleEvent(eventName, args);
            }

            this.DisposeUnavailable();
        }

        public void Subscribe(IEventHandler eventHandler)
        {
            this.eventHandlers.Add(eventHandler);
        }
    }
}
