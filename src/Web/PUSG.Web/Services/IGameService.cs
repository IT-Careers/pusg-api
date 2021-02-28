using PUSG.Web.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUSG.Web.Services
{
    public interface IGameService
    {
        Task CreateGameSession(string blueClientId, string redClientId);

        Task DestroyGameSession(string clientId);

        GameSession this[string clientId] { get; set; }
        
        List<string> Waiting { get; }
        
        ConcurrentDictionary<string, string> PlayerUsernames { get; }

        ConcurrentDictionary<string, string> OtherPlayer { get; }

        ConcurrentDictionary<string, string> PlayerColor { get; }

        void Shoot(string clientId);

        void MoveUp(string clientId);

        void MoveRight(string clientId);

        void MoveDown(string clientId);

        void MoveLeft(string clientId);

        void RotateLeft(string clientId);

        void RotateRight(string clientId);
    }
}
