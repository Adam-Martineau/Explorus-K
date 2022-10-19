using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    public class Movement
    {
        protected int posX;
        protected int posY;
        protected MovementDirection movementDirection;
        protected int animationCount = 0;

        protected Movement(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
        }

        public int getPosX()
        {
            return posX;
        }

        public int getPosY()
        {
            return posY;
        }

        public MovementDirection getMovementDirection()
        {
            return movementDirection;
        }

        public void setMovementDirection(MovementDirection direction)
        {
            this.movementDirection = direction;
        }

        public void moveDown(int stepSize)
        {
            posY += stepSize;
        }

        public void moveUp(int stepSize)
        {
            posY -= stepSize;
        }

        public void moveLeft(int stepSize)
        {
            posX -= stepSize;
        }

        public void moveRight(int stepSize)
        {
            posX += stepSize;
        }

        public int getAnimationCount()
        {
            return animationCount;
        }

        public void setAnimationCount(int count)
        {
            animationCount = count;
        }
    }
}
