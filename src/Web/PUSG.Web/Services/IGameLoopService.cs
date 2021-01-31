using Microsoft.Extensions.Hosting;
using PUSG.Web.Models;
using System;

namespace PUSG.Web.Static
{
    public interface IGameLoopService : IHostedService, IDisposable
    {
        GameSession GameSession { get; set; }
    }
}
