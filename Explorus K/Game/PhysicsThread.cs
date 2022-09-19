using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game
{
    internal class PhysicsThread
    {
        public List<Sprite> sprites { get; set; } = new List<Sprite>();

        public void startThread()
        {
            Sprite slimus;

            while (true)
            {
                foreach (Sprite sprite in sprites)
                    if (sprite.spriteId == SpriteType.SLIMUS)
                        slimus = sprite;
            }
        }

        public bool areSpriteColliding(Sprite sprite_A, Sprite sprite_B)
        {
            if (sprite_A == null || sprite_B == null)
                return false;

            double max = sprite_A.radius + sprite_B.radius;
            double diff = getDiff(sprite_A, sprite_B);

            if (diff <= max)
                return true;
            else
                return false;
        }

        private double getDiff(Sprite sprite_A, Sprite sprite_B)
        {
            int x = Math.Abs(sprite_A.x_pos - sprite_B.x_pos);
            int y = Math.Abs(sprite_A.y_pos - sprite_B.y_pos);

            return Math.Sqrt(y * y + x * x);
        }
    }
}
