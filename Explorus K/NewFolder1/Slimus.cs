using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K
{
    internal class Slimus
    {
        private int posX;
        private int posY;
        private ImageType imageType;

        public Slimus(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
            imageType = ImageType.SLIMUS_DOWN_ANIMATION_1;
        }

        public ImageType getImageType()
        {
            return imageType;
        }

        public int getPosX()
        {
            return posX;
        }

        public int getPosY()
        {
            return posY;
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

        public Image2D refreshSlimus()
        {
            return new Image2D(0, imageType, posX, posY);  
        }
    }
}
