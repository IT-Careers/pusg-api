using Microsoft.AspNetCore.SignalR;
using PUSG.Web.Models;
using PUSG.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUSG.Web.Hubs
{
    public class GameHub : Hub
    {
        private const string ServerMessageUser = "PUSG";

        private ISessionService sessionService;

        private IGameService gameService;

        private IHubContext<HomeHub> homeHubContext;

        public GameHub(ISessionService sessionService, IGameService gameService, IHubContext<HomeHub> homeHubContext)
        {
            this.sessionService = sessionService;
            this.gameService = gameService;
            this.homeHubContext = homeHubContext;
        }

        public override async Task OnConnectedAsync()
        {
            this.gameService.Waiting.Add(this.Context.ConnectionId);

            if(this.gameService.Waiting.Count >= 2)
            {
                await this.gameService.CreateGameSession(
                    this.gameService.Waiting[0],
                    this.gameService.Waiting[1]);

                await this.Clients.Client(
                    this.gameService.Waiting[0]).SendAsync("StartGame", new []{ "blue" });
                await this.Clients.Client(
                        this.gameService.Waiting[1]).SendAsync("StartGame", new[]{ "red" });

                this.gameService.Waiting.RemoveAt(0);
                this.gameService.Waiting.RemoveAt(0);
            }

            await base.OnConnectedAsync();
        }

        public async Task Login(string username)
        {
            this.gameService.PlayerUsernames[this.Context.ConnectionId] = username;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string id = this.Context.ConnectionId;
            string otherId = this.gameService.OtherPlayer.ContainsKey(id) 
                ? this.gameService.OtherPlayer[id]
                : null;

            await this.gameService.DestroyGameSession(id);

            await this.Clients.Client(otherId).SendAsync("GameEnd");
                
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Update(string command)
        {
            switch(command)
            {
                case "up":
                    {
                        this.gameService.MoveUp(this.Context.ConnectionId);
                        await this.Clients.Client(this.gameService.OtherPlayer[this.Context.ConnectionId])
                            .SendAsync("OtherUp");
                        break;
                    }
                case "right":
                    {
                        this.gameService.MoveRight(this.Context.ConnectionId);
                        await this.Clients.Client(this.gameService.OtherPlayer[this.Context.ConnectionId])
                            .SendAsync("OtherRight");
                        break;
                    }
                case "down":
                    {
                        this.gameService.MoveDown(this.Context.ConnectionId);
                        await this.Clients.Client(this.gameService.OtherPlayer[this.Context.ConnectionId])
                            .SendAsync("OtherDown");
                        break;
                    }
                case "left":
                    {
                        this.gameService.MoveLeft(this.Context.ConnectionId);
                        await this.Clients.Client(this.gameService.OtherPlayer[this.Context.ConnectionId])
                            .SendAsync("OtherLeft");
                        break;
                    }
                case "rotate-left":
                    {
                        this.gameService.RotateLeft(this.Context.ConnectionId);
                        await this.Clients.Client(this.gameService.OtherPlayer[this.Context.ConnectionId])
                            .SendAsync("OtherRotateLeft");
                        break;
                    }
                case "rotate-right":
                    {
                        this.gameService.RotateRight(this.Context.ConnectionId);
                        await this.Clients.Client(this.gameService.OtherPlayer[this.Context.ConnectionId])
                            .SendAsync("OtherRotateRight");
                        break;
                    }
                case "pew":
                    {
                        this.gameService.Shoot(this.Context.ConnectionId);
                        await this.Clients.Client(this.gameService.OtherPlayer[this.Context.ConnectionId])
                            .SendAsync("OtherShoot");
                        break;
                    }
            }
        }

        public async Task GetState()
        {
            GameSession gameSession = this.gameService[this.Context.ConnectionId];

            if (gameSession.IsDone)
            {
                string id = this.Context.ConnectionId;
                string otherId = this.gameService.OtherPlayer.ContainsKey(id)
                ? this.gameService.OtherPlayer[id]
                : null;

                string clientUsername = this.gameService.PlayerUsernames[id];
                string otherClientUsername = this.gameService.PlayerUsernames[otherId];

                List<string> clientSessionIds = this.sessionService.Users.Where(user => user.Value == clientUsername).Select(user => user.Key).ToList();

                List<string> otherClientSessionIds = this.sessionService.Users.Where(user => user.Value == otherClientUsername).Select(user => user.Key).ToList();

                List<string> totalExcludedIds = clientSessionIds.Concat(otherClientSessionIds).ToList();

                string winningMessage =
                    this.gameService.PlayerColor[id] == gameSession.Winner
                    ? $"{clientUsername} won against {otherClientUsername}!"
                    : $"{otherClientUsername} won against {clientUsername}!";

                await this.homeHubContext.Clients.AllExcept(totalExcludedIds).SendAsync("ReceiveMessage", new[] { ServerMessageUser, winningMessage });
                await this.Clients.Clients(id, otherId).SendAsync("GameEnd", new { Result = winningMessage });
            }
            else
            {
                GameState gameState = new GameState
                {
                    Blue = gameSession.BluePlayer,
                    Red = gameSession.RedPlayer,
                    Shells = gameSession.Shells.Select(shell => shell.Value).ToList(),
                };

                await this.Clients
                    .Client(this.Context.ConnectionId)
                    .SendAsync("GameState", gameState);
            }
        }
    }
}
