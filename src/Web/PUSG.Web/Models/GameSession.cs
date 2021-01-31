using PUSG.Web.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PUSG.Web.Models
{
    public class GameSession
    {
        public Player BluePlayer { get; set; }

        public Player RedPlayer { get; set; }

        public bool IsDone { get; private set;  } = false;

        public string Winner { get; private set; } = null;

        public ConcurrentDictionary<string, Shell> Shells { get; set; } = new ConcurrentDictionary<string, Shell>();

        public Player this[string color]
        {
            get
            {
                if (color == "blue") return this.BluePlayer;
                if (color == "red") return this.RedPlayer;
                return null;
            }
        }

        public void Update()
        {
            lock (this.Shells)
            {
                Stack<Shell> shellsToDelete = new Stack<Shell>();

                foreach (var shell in this.Shells)
                {
                    shell.Value.Update();

                    if(shell.Value.IsDestroyable)
                    {
                        shellsToDelete.Push(shell.Value);
                    }
                }

                foreach (var item in shellsToDelete)
                {
                    this.Shells.Delete(item.Id);
                }

                foreach (var shell in this.Shells.Values.Where(shell => shell.Color == "red"))
                {
                    if (CollisionEngine.CollidesTankWithShell(this.BluePlayer, shell))
                    {
                        this.IsDone = true; // RED WINS
                        this.Winner = "red";
                    }
                }

                foreach (var shell in this.Shells.Values.Where(shell => shell.Color == "blue"))
                {
                    if (CollisionEngine.CollidesTankWithShell(this.RedPlayer, shell))
                    {
                        this.IsDone = true; // BLUE WINS
                        this.Winner = "blue";
                    }
                }
            }
        }

        public void Shoot(string playerColor)
        {
            if(playerColor == "blue")
            {
                Shell shell = new Shell
                {
                    Direction = this[playerColor].Direction,
                    Color = playerColor,
                    X = this[playerColor].X,
                    Y = this[playerColor].Y
                };

                this.Shells[shell.Id] = shell;
            }
            else if(playerColor == "red")
            {
                Shell shell = new Shell
                {
                    Direction = this[playerColor].Direction,
                    Color = playerColor,
                    X = this[playerColor].X,
                    Y = this[playerColor].Y
                };
                
                this.Shells[shell.Id] = shell;
            }
        }
    }
}
