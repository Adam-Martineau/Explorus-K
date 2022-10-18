using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Explorus_K.Game.Replay
{
    internal class Receiver
    {
        public Receiver()
        {
        }

        public void computePlayerCommand(Player player, MovementDirection movementDirection)
        {
            player.setMovementDirection(movementDirection);
        }

        public void computeBubbleCommand(Bubble bubble, BubbleManager bubbleManager)
        {
            Bubble bubbleCopy = new Bubble(bubble.getPosX(), bubble.getPosY(), ImageType.BUBBLE_BIG, bubble.getMovementDirection(), bubble.getIteratorPosition());

            bubbleManager.addBubble(bubbleCopy);
        }

        public void computeDecreaseLifeCommand(Player player, bool isDecreasing)
        {
            if(isDecreasing)
            {
                player.decreaseLife();
            }
            else
            {
                player.setLives(player.getLifes() + 1);
            }
        }

        public void computeGemCollectingCommand(GemBar gemBar, bool isIncreasing)
        {
            if(isIncreasing)
            {
                gemBar.Increase();
            }
            else
            {
                gemBar.Decrease();
            }
        }

        public void computeDecreaseHealthBarCommand(HealthBar healthBar, bool isDecreasing)
        {
            if (isDecreasing)
            {
                healthBar.Decrease();
            }
            else
            {
                healthBar.Increase();
            }
        }

        public void computeToxicSlimeDeadCommand(LabyrinthImage labyrinthImage, Player player, Image2D image2D, bool isUndo)
        {
            if (!isUndo)
            {
                labyrinthImage.getPlayerList().Remove(player);
                labyrinthImage.labyrinthImages.Remove(image2D);
                labyrinthImage.labyrinthImages.Add(new Image2D(SpriteType.GEM, ImageType.GEM, image2D.X, image2D.Y));
            }
            else
            {
                labyrinthImage.getPlayerList().Add(player);
                labyrinthImage.labyrinthImages.Add(image2D);
                labyrinthImage.labyrinthImages.Remove(new Image2D(SpriteType.GEM, ImageType.GEM, image2D.X, image2D.Y));
            }
        }
    }
}
