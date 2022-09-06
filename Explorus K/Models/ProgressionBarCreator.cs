using Explorus_K.Game;
using Explorus_K.NewFolder1;
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

        public IBar InitializeBar(int length)
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
        int getCurrent();
        IBar Initialize(int length);
        void Decrease();
        void Increase();
        int getLength();
    }
}
