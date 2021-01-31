using System.Collections.Generic;

namespace PUSG.Web.Models
{
    public class Player : BaseGameObject
    {
        private Dictionary<string, string> directionsRight = new Dictionary<string, string>
        {
            ["up"] = "right",
            ["right"] = "down",
            ["down"] = "left",
            ["left"] = "up"
        };

        private Dictionary<string, string> directionsLeft = new Dictionary<string, string>
        {
            ["up"] = "left",
            ["left"] = "down",
            ["down"] = "right",
            ["right"] = "up"
        };

        public string Color { get; set; }

        public string Direction { get; set; }

        public string Type { get; set; }

        public bool IsDead { get; set; } = false;

        public void MoveUp()
        {
            if (this.Y - 20 >= 0)
            {
                this.Y -= 20;
            }
        }

        public void MoveRight()
        {
            if (this.X + 20 <= 1366)
            {
                this.X += 20;
            }
        }

        public void MoveDown()
        {
            if (this.Y + 20 <= 1366)
            {
                this.Y += 20;
            }
        }

        public void MoveLeft()
        {
            if (this.X - 20 >= 0)
            {
                this.X -= 20;
            }
        }

        public void RotateLeft()
        {
            this.Direction = this.directionsLeft[this.Direction];
        }

        public void RotateRight()
        {
            this.Direction = this.directionsRight[this.Direction];
        }
    }
}
