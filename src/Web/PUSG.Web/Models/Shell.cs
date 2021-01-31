using System;

namespace PUSG.Web.Models
{
    public class Shell : BaseGameObject
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Direction { get; set; }

        public string Color { get; set; }

        public bool IsDestroyable { get; set; } = false;

        public void Update()
        {
            if(this.Direction == "up")
            {
                this.Y -= 7;
            } 
            else if(this.Direction == "right")
            {
                this.X += 7;
            } 
            else if(this.Direction == "down")
            {
                this.Y += 7;
            }
            else if(this.Direction == "left")
            {
                this.X -= 7;
            }

            if(this.Y < 0 || this.Y > 1366 || this.X < 0 || this.X > 1366)
            {
                this.IsDestroyable = true;
            }
        }
    }
}
