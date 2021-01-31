using System.Collections.Generic;

namespace PUSG.Web.Models
{
    public class GameState
    {
        public Player Blue { get; set; }

        public Player Red { get; set; }

        public List<Shell> Shells { get; set; }
    }
}
