using Explorus_K.Game;
using System;
using System.Collections.Generic;

namespace Explorus_K.Models
{
    public class Slimus : Player
    {
        private bool invincible;

        public Slimus(int posX, int posY, ImageType imageType, int life, Iterator iterator) : base(posX, posY, imageType, life, iterator)
        {
            this.imageType = imageType;
            this.lifeCount = life;
            invincible = false;
            fillAnimationDict();
            labyrinthName = "s";
            this.iterator = iterator;
        }

        public bool getInvincible()
        {
            return invincible;
        }

        public void setInvincible(bool invincible)
        {
            this.invincible = invincible;
        }

        public override Image2D refreshPlayer()
        {
            return new Image2D(SpriteType.SLIMUS, imageType, posX, posY);
        }

        protected override void fillAnimationDict()
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
    }
}
