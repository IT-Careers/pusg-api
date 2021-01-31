using System.Drawing;

namespace PUSG.Web.Models
{
    public class CollisionEngine
    {
        public static bool CollidesTankWithShell(BaseGameObject tank, BaseGameObject shell)
        {
            Rectangle targetRectangle = new Rectangle((int)tank.X, (int)tank.Y, 50, 40);

            Rectangle colliderRectangle = new Rectangle((int)shell.X, (int)shell.Y, 25, 25);

            return colliderRectangle.IntersectsWith(targetRectangle);
        }
    }
}
