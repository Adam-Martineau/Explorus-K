using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    abstract class ProgressionBarCreator
    {
        public abstract IBar FactoryMethod();

        public List<Image2D> InitializeBar(int length)
        {
            // Call the factory method to create a Product object.
            var bar = FactoryMethod();

            return bar.Initialize(length);
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

    interface IBar
    {
        List<Image2D> Initialize(int length);
    }

    class HealthBar : IBar
    {
        public static List<Image2D> healthBar = new List<Image2D>();
        private int current;
        private int length;

        public List<Image2D> Initialize(int length)
        {
            this.current = 0;
            this.length = length;

            healthBar.Add(new Image2D(0, ImageType.LEFT_SIDE_BAR));
            for (int i = 1; i < length + 1; i++)
            {
                healthBar.Add(new Image2D(i, ImageType.EMPTY_BAR));
            }
            healthBar.Add(new Image2D(length + 1, ImageType.RIGHT_SIDE_BAR));

            return healthBar;
        }
    }

    class BubbleBar : IBar
    {
        public static List<Image2D> bubbleBar = new List<Image2D>();
        private int current;
        private int length;

        public List<Image2D> Initialize(int length)
        {
            this.current = 0;
            this.length = length;

            bubbleBar.Add(new Image2D(0, ImageType.LEFT_SIDE_BAR));
            for (int i = 1; i < length + 1; i++)
            {
                bubbleBar.Add(new Image2D(i, ImageType.EMPTY_BAR));
            }
            bubbleBar.Add(new Image2D(length + 1, ImageType.RIGHT_SIDE_BAR));

            return bubbleBar;
        }
    }

    class GemBar : IBar
    {
        public static List<Image2D> gemBar = new List<Image2D>();
        private int current;
        private int length;

        public List<Image2D> Initialize(int length)
        {
            this.current = 0;
            this.length = length;

            gemBar.Add(new Image2D(0, ImageType.LEFT_SIDE_BAR));
            for (int i = 1; i < length+1; i++)
            {
                gemBar.Add(new Image2D(i, ImageType.EMPTY_BAR));
            }
            gemBar.Add(new Image2D(length+1, ImageType.RIGHT_SIDE_BAR));

            return gemBar;
        }
    }
}
