using Explorus_K.Game;
using System;
using System.Collections.Generic;

namespace Explorus_K.Models
{
    internal class ToxicSlime : Player
    {
        public ToxicSlime(int posX, int posY, ImageType imageType, int life, Iterator iterator) : base(posX, posY, imageType, life, iterator)
        {
            fillAnimationDict();
        }

        public override Image2D refreshPlayer()
        {
            return new Image2D(SpriteType.TOXIC_SLIME, imageType, posX, posY, id);
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
    }
}
