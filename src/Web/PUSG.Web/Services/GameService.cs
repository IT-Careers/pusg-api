using Microsoft.Extensions.DependencyInjection;
using PUSG.Web.Extensions;
using PUSG.Web.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PUSG.Web.Static
{
    public class GameService : IGameService
    {
        private ConcurrentDictionary<string, GameSession> sessions;

        private ConcurrentDictionary<string, GameLoopService> loops;

        private ConcurrentDictionary<string, CancellationToken> loopCancellationTokens;

        private IServiceProvider serviceProvider;

        public GameService(IServiceProvider serviceProvider)
        {
            // Key - Socket Id (client), Value - Game Session
            this.sessions = new ConcurrentDictionary<string, GameSession>();
            this.loops = new ConcurrentDictionary<string, GameLoopService>();
            this.loopCancellationTokens = new ConcurrentDictionary<string, CancellationToken>();
            this.Waiting = new List<string>();
            this.PlayerUsernames = new ConcurrentDictionary<string, string>();
            this.OtherPlayer = new ConcurrentDictionary<string, string>();
            this.PlayerColor = new ConcurrentDictionary<string, string>();
            this.serviceProvider = serviceProvider;
        }

        public List<string> Waiting { get; }

        public ConcurrentDictionary<string, string> PlayerUsernames { get; }

        public ConcurrentDictionary<string, string> OtherPlayer { get; }

        public ConcurrentDictionary<string, string> PlayerColor { get; }

        public async Task CreateGameSession(string blueClientId, string redClientId)
        {
            GameSession gameSession = new GameSession();

            gameSession.BluePlayer = new Player
            {
                X = 50,
                Y = 384,
                Direction = "right",
                Color = "blue",
                Type = "basic"
            };

            gameSession.RedPlayer = new Player
            {
                X = 1266,
                Y = 384,
                Direction = "left",
                Color = "red",
                Type = "basic"
            };

            this.sessions[blueClientId] = gameSession;
            this.sessions[redClientId] = gameSession;
            this.OtherPlayer[blueClientId] = redClientId;
            this.OtherPlayer[redClientId] = blueClientId;
            this.PlayerColor[blueClientId] = "blue";
            this.PlayerColor[redClientId] = "red";

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            GameLoopService gameLoop = this.serviceProvider.CreateScope().ServiceProvider.GetRequiredService<GameLoopService>();

            this.loops[blueClientId] = gameLoop;
            this.loops[redClientId] = gameLoop;
            this.loopCancellationTokens[blueClientId] = token;
            this.loopCancellationTokens[redClientId] = token;

            gameLoop.GameSession = gameSession;
            await gameLoop.StartAsync(token);
        }

        public async Task DestroyGameSession(string clientId)
        {
            string otherClientId = this.OtherPlayer.ContainsKey(clientId) ? this.OtherPlayer[clientId] : null;

            await this.loops[clientId].StopAsync(this.loopCancellationTokens[clientId]);
            this.loops[clientId].Dispose();

            this.loops.Delete(clientId);
            this.sessions.Delete(clientId);
            this.PlayerUsernames.Delete(clientId);
            this.PlayerColor.Delete(clientId);
            this.loopCancellationTokens.Delete(clientId);
            this.OtherPlayer.Delete(clientId);

            if (otherClientId != null)
            {
                this.loops.Delete(otherClientId);
                this.sessions.Delete(otherClientId);
                this.PlayerUsernames.Delete(otherClientId);
                this.PlayerColor.Delete(otherClientId);
                this.loopCancellationTokens.Delete(otherClientId);
                this.OtherPlayer.Delete(otherClientId);
            }
        }

        public GameSession this[string clientId]
        {
            get
            {
                return this.sessions.ContainsKey(clientId) ? this.sessions[clientId] : null;
            }
            set
            {
                this.sessions[clientId] = value;
            }
        }

        public void Shoot(string clientId)
        {
            this.sessions[clientId].Shoot(this.PlayerColor[clientId]);
        }
        public void MoveUp(string clientId)
        {
            this.sessions[clientId][this.PlayerColor[clientId]].MoveUp();
        }

        public void MoveRight(string clientId)
        {
            this.sessions[clientId][this.PlayerColor[clientId]].MoveRight();
        }

        public void MoveDown(string clientId)
        {
            this.sessions[clientId][this.PlayerColor[clientId]].MoveDown();
        }

        public void MoveLeft(string clientId)
        {
            this.sessions[clientId][this.PlayerColor[clientId]].MoveLeft();
        }

        public void RotateLeft(string clientId)
        {
            this.sessions[clientId][this.PlayerColor[clientId]].RotateLeft();
        }

        public void RotateRight(string clientId)
        {
            this.sessions[clientId][this.PlayerColor[clientId]].RotateRight();
        }
    }
}
