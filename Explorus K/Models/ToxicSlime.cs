using Explorus_K.Game;
using System;
using System.Collections.Generic;

namespace Explorus_K.Models
{
    internal class ToxicSlime : Player
    {
        private int posX;
        private int posY;
        private ImageType imageType;
        private int lifeCount;
        private MovementDirection movementDirection;
        private Dictionary<MovementDirection, List<ImageType>> animationDict;
        private int animationCount = 0;
        private string labyrinthName;
        private Iterator iterator;
        private Guid id = Guid.NewGuid();

        public ToxicSlime(int posX, int posY, ImageType imageType, int life, Iterator iterator)
        {
            this.posX = posX;
            this.posY = posY;
            this.imageType = imageType;
            this.lifeCount = life;
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
            return new Image2D(SpriteType.TOXIC_SLIME, imageType, posX, posY, id);
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
                        tempImageList = new List<ImageType> { ImageType.TOXIC_SLIME_DOWN_ANIMATION_1, ImageType.TOXIC_SLIME_DOWN_ANIMATION_2, ImageType.TOXIC_SLIME_DOWN_ANIMATION_3 };
                        animationDict.Add(movementDirection, tempImageList);
                        break;
                    case MovementDirection.up:
                        tempImageList = new List<ImageType> { ImageType.TOXIC_SLIME_UP_ANIMATION_1, ImageType.TOXIC_SLIME_UP_ANIMATION_2, ImageType.TOXIC_SLIME_UP_ANIMATION_3 };
                        animationDict.Add(movementDirection, tempImageList);
                        break;
                    case MovementDirection.left:
                        tempImageList = new List<ImageType> { ImageType.TOXIC_SLIME_LEFT_ANIMATION_1, ImageType.TOXIC_SLIME_LEFT_ANIMATION_2, ImageType.TOXIC_SLIME_LEFT_ANIMATION_3 };
                        animationDict.Add(movementDirection, tempImageList);
                        break;
                    case MovementDirection.right:
                        tempImageList = new List<ImageType> { ImageType.TOXIC_SLIME_RIGHT_ANIMATION_1, ImageType.TOXIC_SLIME_RIGHT_ANIMATION_2, ImageType.TOXIC_SLIME_RIGHT_ANIMATION_3 };
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

        public void setLives(int lives)
        {
            this.lifeCount = lives;
        }
    }
}
