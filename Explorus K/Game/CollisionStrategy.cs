﻿using Explorus_K.Game;
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

        public void executeStrategy(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            _strategy.execute(labyrinthImage, imageIndex, type);
        }
    }

    interface IStrategy
    {
        void execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type);
    }

    class GemStrategy : IStrategy
    {
        public void execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            if(type == SpriteType.SLIMUS)
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
    }

    class DoorStrategy : IStrategy
    {
        public void execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            if (type == SpriteType.SLIMUS)
            {
                if (labyrinthImage.KeyState.CurrentState() == "WithKeyState")
                {
                    labyrinthImage.removeImageAt(imageIndex);
                }
            }
        }
    }

    class MiniSlimeStrategy : IStrategy
    {
        public void execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            if (type == SpriteType.SLIMUS)
            {
                Thread.Sleep(3000);
                Application.Exit();
            }
        }
    }

    class ToxicSlimeStrategy : IStrategy
    {
        public void execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            if (type == SpriteType.SLIMUS)
            {
                labyrinthImage.HealthBar.Decrease();
                
                if(labyrinthImage.HealthBar.getCurrent() == 0)
                {
                    //labyrinthImage.gameOver();
                    Thread.Sleep(3000);
                    Application.Exit();
                }
            }
            if (type == SpriteType.BUBBLE)
            {
                //Perte de vie du toxic slime
            }
        }
    }
}
