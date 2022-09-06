using Explorus_K.Models;
using Explorus_K.NewFolder1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game
{
    class BubbleBar : IBar
    {
        public List<Image2D> bubbleBar = new List<Image2D>();
        private int current;
        private int length;

        public IBar Initialize(int length)
        {
            this.current = length;
            this.length = length;

            bubbleBar.Add(new Image2D(0, ImageType.LEFT_SIDE_BAR, 0, 1));
            for (int i = 1; i < length + 1; i++)
            {
                bubbleBar.Add(new Image2D(SpriteId.BAR, ImageType.BUBBLE_BAR_FULL, i, 1));
            }
            bubbleBar.Add(new Image2D(SpriteId.BAR, ImageType.RIGHT_SIDE_BAR, length + 1, 1));

            return this;
        }
        public void Decrease()
        {
            if (current > 0)
            {
                bubbleBar[current].setType(ImageType.EMPTY_BAR);
                current--;
                //gemBar[current].setType(ImageType.GEM_BAR_HALF);
            }
        }
        public void Increase()
        {
            if (current <= length)
            {
                bubbleBar[current].setType(ImageType.BUBBLE_BAR_FULL);
                current++;
                //gemBar[current].setType(ImageType.GEM_BAR_HALF);
            }
        }
        public int getCurrent()
        {
            return this.current;
        }

        public int getLength()
        {
            return this.length;
        }
    }
}
