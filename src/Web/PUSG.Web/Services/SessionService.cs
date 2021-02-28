using System.Collections.Generic;
using System.Linq;

namespace PUSG.Web.Services
{
    public class SessionService : ISessionService
    {
        private Dictionary<string, string> users;

        public SessionService()
        {
            this.users = new Dictionary<string, string>();
            this.LiveConnections = new List<string>();
        }

        public Dictionary<string, string> Users => this.users;

        public List<string> LiveConnections { get; }

        public void AddUser(string connectionId, string username)
        {
            this.users[connectionId] = username;
        }

        public KeyValuePair<string, string> GetUser(string connectionId)
        {
            return this.users.SingleOrDefault(user => user.Key == connectionId);
        }

        public bool IsLogged(string credential)
        {
            return this.users.ContainsKey(credential) || this.users.ContainsValue(credential);
        }

        public void RemoveUser(string connectionId)
        {
            this.users.Remove(connectionId);
        }
    }
}
