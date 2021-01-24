using Microsoft.AspNetCore.SignalR;
using PUSG.Web.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUSG.Web.Hubs
{
    public class HomeHub : Hub
    {
        private ISessionService sessionService;

        public HomeHub(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        private async Task BroadcastToLoggedIn(string message, params object[] contents)
        {
            List<string> excludedConnections = this.sessionService
                .LiveConnections
                .Where(liveConnection => !this.sessionService.IsLogged(liveConnection))
                .ToList();

            await this.Clients.AllExcept(excludedConnections).SendAsync(message, contents);
        }

        public async Task SendMessage(string username, string messageContent)
        {
            if (this.sessionService.IsLogged(username))
            {
                await this.BroadcastToLoggedIn("ReceiveMessage", username, messageContent);
            }
        }

        public async Task Login(string username)
        {
            if(!this.sessionService.IsLogged(username))
            {
                this.sessionService.AddUser(this.Context.ConnectionId, username);
            }
        }

        public override async Task OnConnectedAsync()
        {
            this.sessionService.LiveConnections.Add(this.Context.ConnectionId);

            await this.BroadcastToLoggedIn("OnConnected", "A new user has connected!");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            this.sessionService.LiveConnections.Remove(this.Context.ConnectionId);
            this.sessionService.RemoveUser(this.Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
