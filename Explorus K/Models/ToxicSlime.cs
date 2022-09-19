using Explorus_K.Game;
using Explorus_K.Game;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    internal class ToxicSlime : Player
    {
        private int posX;
        private int posY;
        private ImageType imageType;
        private int lifeCount;

        public ToxicSlime(int posX, int posY, ImageType imageType, int life)
        {
            this.posX = posX;
            this.posY = posY;
            this.imageType = imageType;
            this.lifeCount = life;
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
            return new Image2D(SpriteType.SLIMUS, imageType, posX, posY);
        }
    }
}
