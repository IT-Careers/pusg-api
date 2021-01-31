using PUSG.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PUSG.Web.Static
{
    public class GameLoopService : IGameLoopService
    {
        private Timer timer;

        public GameSession GameSession { get; set; }

        private void Step(object state)
        {
            this.GameSession?.Update();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.timer = new Timer(Step, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(17));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }
    }
}
