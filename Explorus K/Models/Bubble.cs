using Explorus_K.Game;
using System;
using System.Drawing;

namespace Explorus_K.Models
{
    public class Bubble : Movement
    {
        private ImageType imageType;
        private bool popped = false;
        private Point iteratorPos;
        public Guid id { get; } = Guid.NewGuid();

        public Bubble(int posX, int posY, ImageType imageType, MovementDirection movementDirection, Point iteratorPos) : base(posX, posY)
        {
            this.imageType = imageType;
            this.movementDirection = movementDirection;
            this.iteratorPos = iteratorPos;
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

        public Image2D refreshBubble()
        {
            return new Image2D(SpriteType.BUBBLE, imageType, posX, posY, id);
        }

        public Point getIteratorPosition()
        {
            return iteratorPos;
        }

        public void setIteratorPosition(Point iteratorPos)
        {
            this.iteratorPos = iteratorPos;
        }
    }
}
