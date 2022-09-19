using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    internal class Bubble
    {
        int posX;
        int posY;
        ImageType imageType;

        Bubble(int posX, int posY, ImageType imageType)
        {
            this.posX = posX;
            this.posY = posY;
            this.imageType = imageType;
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
            return new Image2D(SpriteType.BUBBLE, imageType, posX, posY);
        }
    }
}
