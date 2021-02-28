using System.Collections.Generic;

namespace PUSG.Web.Services
{
    public interface ISessionService
    {
        bool IsLogged(string username);

        void AddUser(string connectionId, string username);

        void RemoveUser(string connectionId);

        Dictionary<string, string> Users { get; }

        List<string> LiveConnections { get; }

        KeyValuePair<string, string> GetUser(string connectionId);
    }
}
