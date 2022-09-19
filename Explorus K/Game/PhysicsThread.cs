using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K.Game
{
    internal class PhysicsThread
    {
        public List<Sprite> sprites { get; set; } = new List<Sprite>();
        CollisionContext collisionStrategy = new CollisionContext();

        public void startThread()
        {
            Sprite slimus = null;

            while (true)
            {
                foreach (Sprite sprite in sprites)
                    if (sprite.spriteType == SpriteType.SLIMUS)
                        slimus = sprite;

                foreach (Sprite sprite in sprites)
                {
                    if (areSpriteColliding(slimus, sprite)) ;

                }
            }
        }

        // Checking if two sprite are colliding using circular hitbox
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

        private void newCollision(Sprite sprite_A, Sprite sprite_B)
        {
            // If the collision is with the slimus
            if (sprite_A.spriteType == SpriteType.SLIMUS || sprite_B.spriteType == SpriteType.SLIMUS)
            {
                // Finding the slimus is colliding with what
                Sprite collidingSprite = null;

                if (sprite_A.spriteType == SpriteType.SLIMUS)
                    collidingSprite = sprite_B;
                else
                    collidingSprite = sprite_A;

                collisionManager(collidingSprite.spriteType);
            }
        }

        private void collisionManager(SpriteType type)
        {
            // doing stuff depending on the collision
            if (type == SpriteType.GEM)
            {
                collisionStrategy.SetStrategy(new GemStrategy());
            }
            else if (type == SpriteType.DOOR)
            {
                collisionStrategy.SetStrategy(new DoorStrategy());
            }
            else if (type == SpriteType.MINI_SLIMUS)
            {
                collisionStrategy.SetStrategy(new MiniSlimeStrategy());
            }
            else if (type == SpriteType.TOXIC_SLIME)
            {
                collisionStrategy.SetStrategy(new ToxicSlimeStrategy());
            }
        }

        // Return the distance between the center of two sprites
        private double getDiff(Sprite sprite_A, Sprite sprite_B)
        {
            int x = Math.Abs(sprite_A.x_pos - sprite_B.x_pos);
            int y = Math.Abs(sprite_A.y_pos - sprite_B.y_pos);

            return Math.Sqrt(y * y + x * x);
        }
    }
}
