using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public void executeStrategy(LabyrinthImage labyrinthImage, int imageIndex)
        {
            _strategy.execute(labyrinthImage, imageIndex);
        }
    }

    interface IStrategy
    {
        void execute(LabyrinthImage labyrinthImage, int imageIndex);
    }

    class GemStrategy : IStrategy
    {
        public void execute(LabyrinthImage labyrinthImage, int imageIndex)
        {
            labyrinthImage.GemBar.Increase();
            if (labyrinthImage.GemBar.getCurrent() == labyrinthImage.GemBar.getLength())
            {
                labyrinthImage.KeyState.RequestChangingState();
                Point pos = labyrinthImage.Labyrinth.MapIterator.findPosition("p");
                labyrinthImage.Labyrinth.MapIterator.replaceAt(".", pos.X, pos.Y);
            }
            labyrinthImage.removeImageAt(imageIndex);
        }
    }

    class DoorStrategy : IStrategy
    {
        public void execute(LabyrinthImage labyrinthImage, int imageIndex)
        {
            if (labyrinthImage.KeyState.CurrentState() == "WithKeyState")
            {
                labyrinthImage.removeImageAt(imageIndex);
            }
        }
    }

    class MiniSlimeStrategy : IStrategy
    {
        public void execute(LabyrinthImage labyrinthImage, int imageIndex)
        {
            Thread.Sleep(3000);
            Application.Exit();
        }
    }
}
