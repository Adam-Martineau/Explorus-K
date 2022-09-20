using Explorus_K.Game;
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
    public class CollisionContext
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

        public GameState executeStrategy(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            return _strategy.execute(labyrinthImage, imageIndex, type);
        }
    }

    public interface IStrategy
    {
        GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type);
    }

    class GemStrategy : IStrategy
    {
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            if(type == SpriteType.SLIMUS)
            {
                labyrinthImage.GemBar.Increase();
                if (labyrinthImage.GemBar.getCurrent() == labyrinthImage.GemBar.getLength())
                {
                    labyrinthImage.KeyState.RequestChangingState();
                    Point pos = labyrinthImage.getSlimus().getIterator().findPosition("p");
                    labyrinthImage.getSlimus().getIterator().replaceAt("l", pos.X, pos.Y);
                }
                labyrinthImage.removeImageAt(imageIndex);
            }

            return GameState.PLAY;
        }
    }

    class DoorStrategy : IStrategy
    {
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            if (type == SpriteType.SLIMUS)
            {
                if (labyrinthImage.KeyState.CurrentState() == "WithKeyState")
                {
                    Point pos = labyrinthImage.getSlimus().getIterator().findPosition("l");
                    labyrinthImage.getSlimus().getIterator().replaceAt(".", pos.X, pos.Y);
                    labyrinthImage.removeImageAt(imageIndex);
                }
            }

            return GameState.PLAY;
        }
    }

    class MiniSlimeStrategy : IStrategy
    {
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            if (type == SpriteType.SLIMUS)
            {
                return GameState.RESTART;
            }
            else
            {
                return GameState.PLAY;
            }
        }
    }

    class ToxicSlimeStrategy : IStrategy
    {
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type)
        {
            if (type == SpriteType.SLIMUS)
            {
                labyrinthImage.HealthBar.Decrease();
                labyrinthImage.getSlimus().decreaseLife();
                
                if(labyrinthImage.HealthBar.getCurrent() == 0)
                {
                    labyrinthImage.stopInvincibilityTimer();
                    return GameState.STOP;
                }
            }
            else if (type == SpriteType.BUBBLE)
            {
                //Perte de vie du toxic slime
                Image2D toxicSlime = labyrinthImage.labyrinthImages[imageIndex];


                foreach (Player player in labyrinthImage.getPlayerList().ToList())
                {
                    if (player.getPosX() == toxicSlime.X && player.getPosY() == toxicSlime.Y)
                    {
                        player.decreaseLife();
                        if (player.getLifes() < 1)
                        {
                            labyrinthImage.getPlayerList().Remove(player);
                            labyrinthImage.labyrinthImages.Remove(toxicSlime);
                        }
                    }
                }
            }

            return GameState.PLAY;
        }
    }
}
