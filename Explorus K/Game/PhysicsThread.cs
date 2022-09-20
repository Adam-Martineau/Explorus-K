using Explorus_K.Controllers;
using Explorus_K.Models;
using Microsoft.VisualBasic.Devices;
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

			while (true)
			{
				GameEngine.physicsWaitHandle.WaitOne();

				foreach (Image2D sprite in sprites.ToList())
				{
					if (sprite.getId() == SpriteType.SLIMUS || sprite.getId() == SpriteType.BUBBLE)
						searchForCollisionWithSprite(sprite);
				}
			}
		}

		private void searchForCollisionWithSprite(Image2D spriteToLookFor)
		{
			foreach (Image2D sprite in sprites.ToList())
			{
				if (sprite.getId() != SpriteType.WALL || sprite.getId() != SpriteType.BAR)
					if (areSpriteColliding(spriteToLookFor, sprite))
						newCollision(spriteToLookFor, sprite);
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

		// Return the distance between the center of two sprites
		private double getDiff(Image2D sprite_A, Image2D sprite_B)
		{
			int x = Math.Abs(sprite_A.X - sprite_B.X);
			int y = Math.Abs(sprite_A.Y - sprite_B.Y);

			return Math.Sqrt(y * y + x * x);
		}

		// the logic for a collision
		private void newCollision(Image2D sprite_A, Image2D sprite_B)
		{
			if (sprite_A.getId() == SpriteType.BUBBLE || sprite_B.getId() == SpriteType.BUBBLE)
			{
				// Finding the slimus is colliding with what
				Image2D collidingSprite = null;
				Image2D bubble = null;

				if (sprite_A.getId() == SpriteType.BUBBLE)
				{
					collidingSprite = sprite_B;
					bubble = sprite_A;
				}
				else
				{
					collidingSprite = sprite_A;
					bubble = sprite_B;
				}

				if (collidingSprite.getId() == SpriteType.TOXIC_SLIME)
					executeToxicSlimeCollisionWithBubble(collidingSprite, bubble);
			}
			// If the collision is with the slimus
			else if (sprite_A.getId() == SpriteType.SLIMUS || sprite_B.getId() == SpriteType.SLIMUS)
			{
				// Finding the slimus is colliding with what
				Image2D collidingSprite = null;

				if (sprite_A.getId() == SpriteType.SLIMUS)
					collidingSprite = sprite_B;
				else
					collidingSprite = sprite_A;

				// executing the collision is there's someting to do
				if (collidingSprite.getId() != SpriteType.TOXIC_SLIME)
					executeCollision(collidingSprite);

				else if (!gameEngine.gameView.labyrinthImage.slimus.getInvincible())
					executeToxicSlimeCollision(collidingSprite);
			}
		}

		private void executeCollision(Image2D collidingSprite)
		{
			gameEngine.State = collidingSprite.collisionStrategy.executeStrategy(
				gameEngine.gameView.labyrinthImage,
				gameEngine.gameView.labyrinthImage.labyrinthImages.IndexOf(collidingSprite),
				SpriteType.SLIMUS);
		}

		private void executeToxicSlimeCollision(Image2D toxicSlime)
		{
			gameEngine.gameView.labyrinthImage.startInvincibilityTimer();

			if (gameEngine.State == GameState.STOP)
			{
				gameEngine.gameView.labyrinthImage.stopInvincibilityTimer();
			}
			else
			{
				gameEngine.State = toxicSlime.collisionStrategy.executeStrategy(
					gameEngine.gameView.labyrinthImage,
					gameEngine.gameView.labyrinthImage.labyrinthImages.IndexOf(toxicSlime),
					SpriteType.SLIMUS);
			}
		}

		private void executeToxicSlimeCollisionWithBubble(Image2D toxicSlime, Image2D bubbleImage)
		{
			foreach (Bubble bubbleBubble in gameEngine.gameView.labyrinthImage.bubbleManager.getBubbleList().ToList())
			{
				if (bubbleImage.id == bubbleBubble.id && !bubbleBubble.isPopped())
				{
                    bubbleBubble.popBubble();

                    gameEngine.State = toxicSlime.collisionStrategy.executeStrategy(
						gameEngine.gameView.labyrinthImage,
						gameEngine.gameView.labyrinthImage.labyrinthImages.IndexOf(toxicSlime),
						SpriteType.BUBBLE);
                }
			}
		}

        public void restartThread()
        {
            Console.WriteLine("yoo");
        }
    }
}
