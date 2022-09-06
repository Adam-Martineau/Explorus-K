using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    class CollisionContext
    {
        private IStrategy _strategy;

        public CollisionContext()
        { }

        public CollisionContext(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void SetStrategy(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public int executeStrategy()
        {
            return _strategy.execute();
        }
    }

    public interface IStrategy
    {
        int execute();
    }

    class GemStrategy : IStrategy
    {
        public int execute()
        {
            return 10;
        }
    }

    class DoorStrategy : IStrategy
    {
        public int execute()
        {
            return 5;
        }
    }

    class MiniSlimeStrategy : IStrategy
    {
        public int execute()
        {
            return 20;
        }
    }
}
