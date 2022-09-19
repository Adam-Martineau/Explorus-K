using Explorus_K.Game;
using Explorus_K.Game;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    public class Slimus : Player
    {
        private int posX;
        private int posY;
        private ImageType imageType;
        private int lifeCount;
        private int bubbleCount;
        private bool invincible;

        public Slimus(int posX, int posY, ImageType imageType, int life)
        {
            this.posX = posX;
            this.posY = posY;
            this.imageType = imageType;
            this.lifeCount = life;
            bubbleCount = Constant.INITIAL_BUBBLE_COUNT;
            invincible = false;
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

        public int getLifes()
        {
            return lifeCount;
        }

        public int decreaseLife()
        {
            return lifeCount--;
        }

        public int getBubbleCount()
        {
            return bubbleCount;
        }

        public int decreaseBubbleCount()
        {
            return bubbleCount--;
        }

        public void setImageType(ImageType imageType)
        {
            this.imageType = imageType;
        }

        public bool getInvincible()
        {
            return invincible;
        }

        public void setInvincible(bool invincible)
        {
            this.invincible = invincible;
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
            return new Image2D(SpriteType.TOXIC_SLIME, imageType, posX, posY);
        }
    }
}
