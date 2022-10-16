using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
