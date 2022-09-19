using Explorus_K.Controllers;
using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Explorus_K.Game
{
    internal class Physics
    {
        GameEngine gameEngine;
        public List<Image2D> sprites { get; set; } = new List<Image2D>();

        public Physics(GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
        }

        public void startThread()
        {
            sprites = gameEngine.gameView.labyrinthImage.labyrinthImages;
            Image2D slimus = null;

            while (true)
            {
                GameEngine.physicsWaitHandle.WaitOne();

                foreach (Image2D sprite in sprites.ToList())
                {
                    if (sprite.getId() == SpriteType.SLIMUS)
                        slimus = sprite;
                }

                foreach (Image2D sprite in sprites.ToList())
                {
                    if (sprite.getId() != SpriteType.WALL || sprite.getId() != SpriteType.BAR)
                        if (areSpriteColliding(slimus, sprite))
                            newCollision(slimus, sprite);

                }

                Thread.Sleep(10);
            }
        }

        // Checking if two sprite are colliding using circular hitbox
        public bool areSpriteColliding(Image2D sprite_A, Image2D sprite_B)
        {
            if (sprite_A == null || sprite_B == null || sprite_A == sprite_B)
                return false;

            double max = sprite_A.radius + sprite_B.radius;
            double diff = getDiff(sprite_A, sprite_B);

            if (diff <= max)
                return true;
            else
                return false;
        }

        private void newCollision(Image2D sprite_A, Image2D sprite_B)
        {
            // If the collision is with the slimus
            if (sprite_A.getId() == SpriteType.SLIMUS || sprite_B.getId() == SpriteType.SLIMUS)
            {
                // Finding the slimus is colliding with what
                Image2D collidingSprite = null;

                if (sprite_A.getId() == SpriteType.SLIMUS)
                    collidingSprite = sprite_B;
                else
                    collidingSprite = sprite_A;

                gameEngine.State = collidingSprite.collisionStrategy.executeStrategy(
                    gameEngine.gameView.labyrinthImage,
                    gameEngine.gameView.labyrinthImage.labyrinthImages.IndexOf(collidingSprite),
                    collidingSprite.getId());
            }
        }

        // Return the distance between the center of two sprites
        private double getDiff(Image2D sprite_A, Image2D sprite_B)
        {
            int x = Math.Abs(sprite_A.X - sprite_B.X);
            int y = Math.Abs(sprite_A.Y - sprite_B.Y);

            return Math.Sqrt(y * y + x * x);
        }
    }
}
