using System.Threading.Tasks;

namespace PUSG.Web.Events.Api
{
    public interface IEventHandler
    {
        Task HandleEvent(string eventName, object eventArgs);

        bool IsAvailable();
    }
}
