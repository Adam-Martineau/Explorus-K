using Explorus_K.Game;
using System;
using System.Collections.Generic;

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
        private MovementDirection movementDirection;
        private Dictionary<MovementDirection, List<ImageType>> animationDict;
        private int animationCount = 0;
        private string labyrinthName = "s";
        private Iterator iterator;
        private Guid id = Guid.NewGuid();

        public Slimus(int posX, int posY, ImageType imageType, int life, Iterator iterator)
        {
            this.posX = posX;
            this.posY = posY;
            this.imageType = imageType;
            this.lifeCount = life;
            bubbleCount = Constant.INITIAL_BUBBLE_COUNT;
            invincible = false;
            movementDirection = MovementDirection.none;
            fillAnimationDict();
            this.iterator = iterator;
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

        public MovementDirection getMovementDirection()
        {
            return movementDirection;
        }

        public void setMovementDirection(MovementDirection direction)
        {
            this.movementDirection = direction;
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
            return new Image2D(SpriteType.SLIMUS, imageType, posX, posY);
        }

        public ImageType getAnimationDictValue(MovementDirection key, int value)
        {
            return animationDict[key][value];
        }

        public int getAnimationCount()
        {
            return animationCount;
        }

        public void setAnimationCount(int count)
        {
            animationCount = count;
        }

        public void setLabyrinthName(string name)
        {
            labyrinthName = name;   
        }

        public string getLabyrinthName()
        {
            return labyrinthName;
        }

        public Iterator getIterator()
        {
            return iterator;
        }
        private void fillAnimationDict()
        {
            animationDict = new Dictionary<MovementDirection, List<ImageType>>();

            foreach (MovementDirection movementDirection in Enum.GetValues(typeof(MovementDirection)))
            {
                List<ImageType> tempImageList;

                switch (movementDirection)
                {
                    case MovementDirection.down:
                        tempImageList = new List<ImageType> { ImageType.SLIMUS_DOWN_ANIMATION_1 , ImageType.SLIMUS_DOWN_ANIMATION_2 , ImageType.SLIMUS_DOWN_ANIMATION_3 };
                        animationDict.Add(movementDirection, tempImageList);
                        break;
                    case MovementDirection.up:
                        tempImageList = new List<ImageType> { ImageType.SLIMUS_UP_ANIMATION_1, ImageType.SLIMUS_UP_ANIMATION_2, ImageType.SLIMUS_UP_ANIMATION_3 };
                        animationDict.Add(movementDirection, tempImageList);
                        break;
                    case MovementDirection.left:
                        tempImageList = new List<ImageType> { ImageType.SLIMUS_LEFT_ANIMATION_1, ImageType.SLIMUS_LEFT_ANIMATION_2, ImageType.SLIMUS_LEFT_ANIMATION_3 };
                        animationDict.Add(movementDirection, tempImageList);
                        break;
                    case MovementDirection.right:
                        tempImageList = new List<ImageType> { ImageType.SLIMUS_RIGHT_ANIMATION_1, ImageType.SLIMUS_RIGHT_ANIMATION_2, ImageType.SLIMUS_RIGHT_ANIMATION_3 };
                        animationDict.Add(movementDirection, tempImageList);
                        break;
                    default:
                        break;
                }
            }
        }

        public Guid GetGuid()
        {
            return this.id;
        }
    }
}
