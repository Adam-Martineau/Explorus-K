using Explorus_K.Game;
using Explorus_K.NewFolder1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    public class Bubble
    {
        private int posX;
        private int posY;
        private ImageType imageType;
        private bool popped = false;
        private MovementDirection movementDirection;
        private Point iteratorPos;
        private int animationCount = 0;


        public Bubble(int posX, int posY, ImageType imageType, MovementDirection movementDirection, Point iteratorPos)
        {
            this.posX = posX;
            this.posY = posY;
            this.imageType = imageType;
            this.movementDirection = movementDirection;
            this.iteratorPos = iteratorPos;
        }

        public int getPosX()
        { 
            return posX; 
        }

        public int getPosY()
        {
            return posY;
        }

        public ImageType getImageType()
        {
            return imageType;
        }

        public void setImageType(ImageType imageType)
        {
            this.imageType=imageType;   
        }

        public bool isPopped()
        {
            return popped;
        }

        public void popBubble()
        {
            popped = true;
            animationCount = 0;
            imageType = ImageType.BUBBLE_EXPLODED;
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

        public Image2D refreshBubble()
        {
            return new Image2D(SpriteId.BUBBLE, imageType, posX, posY);
        }

        public MovementDirection getMovementDirection()
        {
            return movementDirection;
        }

        public Point getIteratorPosition()
        {
            return iteratorPos;
        }

        public void setIteratorPosition(Point iteratorPos)
        {
            this.iteratorPos = iteratorPos;
        }

        public int getAnimationCount()
        {
            return animationCount;
        }

        public void setAnimationCount(int animationCount)
        {
            this.animationCount = animationCount;
        }
    }
}
