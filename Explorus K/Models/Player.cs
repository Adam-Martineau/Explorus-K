using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K
{
    public class Player
    {
        private int posX;
        private int posY;
        private ImageType imageType;
        private bool invincible;

        public Player(int posX, int posY, ImageType imageType)
        {
            this.posX = posX;
            this.posY = posY;
            this.imageType = imageType;
            invincible = false;
        }

        public ImageType getImageType()
        {
            return imageType;
        }

        public bool getInvincible()
        {
            return invincible;
        }

        public void setInvincible(bool invincible)
        {
            this.invincible = invincible;
        }

        public int getPosX()
        {
            return posX;
        }

        public int getPosY()
        {
            return posY;
        }

        public void setImageType(ImageType imageType)
        {
            this.imageType = imageType;
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

        public Image2D refreshPlayer()
        {
            return new Image2D(0, imageType, posX, posY);  
        }
    }
}
