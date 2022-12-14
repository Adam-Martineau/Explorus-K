using Explorus_K.Game;
using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    public abstract class ProgressionBarCreator
    {
        public abstract IBar FactoryMethod();

        public IBar InitializeBar(int length, int current)
        {
            // Call the factory method to create a Product object.
            var bar = FactoryMethod();

            return bar.Initialize(length, current);
        }
    }

    class HealthBarCreator : ProgressionBarCreator
    {
        public override IBar FactoryMethod()
        {
            return new HealthBar();
        }
    }

    class BubbleBarCreator : ProgressionBarCreator
    {
        public override IBar FactoryMethod()
        {
            return new BubbleBar();
        }
    }

    class GemBarCreator : ProgressionBarCreator
    {
        public override IBar FactoryMethod()
        {
            return new GemBar();
        }
    }

    public interface IBar
    {
        int getCurrent();
        IBar Initialize(int length, int current);
        void Decrease();
        void Increase();
        int getLength();
    }

    public class HealthBar : IBar
    {
        public List<Image2D> healthBar = new List<Image2D>();
        private int current;
        private int length;

        public IBar Initialize(int length, int current)
        {
            this.current = current;
            this.length = length;

            healthBar.Add(new Image2D(SpriteType.BAR, ImageType.LEFT_SIDE_BAR, 0, 1));
            for (int i = 1; i < length + 1; i++)
            {
                if(i <= current)
                {
                    healthBar.Add(new Image2D(SpriteType.BAR, ImageType.HEALTH_BAR_FULL, i, 1));
                }
                else
                {
                    healthBar.Add(new Image2D(SpriteType.BAR, ImageType.EMPTY_BAR, i, 1));
                }
            }
            healthBar.Add(new Image2D(SpriteType.BAR, ImageType.RIGHT_SIDE_BAR, length + 1, 1));

            return this;
        }

        public void Decrease()
        {
            if (current > 0)
            {
                healthBar[current].setType(ImageType.EMPTY_BAR);
                current--;
            }
        }
        public void Increase()
        {
            if (current <= length)
            {
                current++;
                healthBar[current].setType(ImageType.HEALTH_BAR_FULL);
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

    public class BubbleBar : IBar
    {
        public List<Image2D> bubbleBar = new List<Image2D>();
        private int current;
        private int length;

        public IBar Initialize(int length, int current)
        {
            this.current = current;
            this.length = length;

            bubbleBar.Add(new Image2D(0, ImageType.LEFT_SIDE_BAR, 0, 1));
            for (int i = 1; i < length + 1; i++)
            {
                bubbleBar.Add(new Image2D(SpriteType.BAR, ImageType.BUBBLE_BAR_FULL, i, 1));
            }
            bubbleBar.Add(new Image2D(SpriteType.BAR, ImageType.RIGHT_SIDE_BAR, length + 1, 1));

            return this;
        }
        public void Decrease()
        {
            if (current > 0)
            {
                for (int i = 1; i < length + 1; i++)
                {
                    bubbleBar[i].setType(ImageType.EMPTY_BAR);
                }
                current-=length;
                //gemBar[current].setType(ImageType.GEM_BAR_HALF);
            }
        }
        public void Increase()
        {
            if (current <= length)
            {
                current++;
                bubbleBar[current].setType(ImageType.BUBBLE_BAR_FULL);
                
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

    public class GemBar : IBar
    {
        public List<Image2D> gemBar = new List<Image2D>();
        private int current;
        private int length;

        public IBar Initialize(int length, int current)
        {
            this.current = current;
            this.length = length;

            gemBar.Add(new Image2D(0, ImageType.LEFT_SIDE_BAR, 0, 1));
            for (int i = 1; i < length + 1; i++)
            {
                gemBar.Add(new Image2D(SpriteType.BAR, ImageType.EMPTY_BAR, i, 1));
            }
            gemBar.Add(new Image2D(SpriteType.BAR, ImageType.RIGHT_SIDE_BAR, length + 1, 1));

            return this;
        }

        public void Decrease()
        {
            if (current > 0)
            {
                gemBar[current].setType(ImageType.EMPTY_BAR);
                current--;
                //gemBar[current].setType(ImageType.GEM_BAR_HALF);
            }
        }
        public void Increase()
        {
            if (current <= length)
            {
                gemBar[current + 1].setType(ImageType.GEM_BAR_FULL);
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
