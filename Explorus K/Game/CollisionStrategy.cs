using Explorus_K.Game;
using Explorus_K.Game;
using Explorus_K.Game.Audio;
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

        public GameState executeStrategy(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio)
        {
            return _strategy.execute(labyrinthImage, imageIndex, type, audio);
        }
    }

    public interface IStrategy
    {
        GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio);
    }

    class GemStrategy : IStrategy
    {
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio)
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
                audio.AddMessage(AudioName.GETTTING_COIN);
            }

            return GameState.PLAY;
        }
    }

    class DoorStrategy : IStrategy
    {
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio)
        {
            if (type == SpriteType.SLIMUS)
            {
                if (labyrinthImage.KeyState.CurrentState() == "WithKeyState")
                {
                    Point pos = labyrinthImage.getSlimus().getIterator().findPosition("l");
                    labyrinthImage.getSlimus().getIterator().replaceAt(".", pos.X, pos.Y);
                    labyrinthImage.removeImageAt(imageIndex);
                    audio.AddMessage(AudioName.OPEN_DOOR);
                }
            }

            return GameState.PLAY;
        }
    }

    class MiniSlimeStrategy : IStrategy
    {
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio)
        {
            if (type == SpriteType.SLIMUS)
            {
                audio.AddMessage(AudioName.WINNING);
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
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio)
        {
            if (type == SpriteType.SLIMUS)
            {
                labyrinthImage.HealthBar.Decrease();
                labyrinthImage.getSlimus().decreaseLife();
                
                if(labyrinthImage.HealthBar.getCurrent() == 0)
                {
                    labyrinthImage.stopInvincibilityTimer();
                    audio.AddMessage(AudioName.BOOM);
                    return GameState.UNDO;
                }
                else
                {
                    audio.AddMessage(AudioName.BEEP_BOOP);
                }

            }
            else if (type == SpriteType.BUBBLE)
            {
                //Perte de vie du toxic slime
                Image2D toxicSlime = labyrinthImage.labyrinthImages[imageIndex];

                foreach (Player player in labyrinthImage.getPlayerList().ToList())
                {
                    if (player.GetGuid() == toxicSlime.id)
                    {
                        player.decreaseLife();
                        audio.AddMessage(AudioName.GETTING_HIT);
                        if (player.getLifes() < 1)
                        {
                            labyrinthImage.getPlayerList().Remove(player);
                            labyrinthImage.labyrinthImages.Remove(toxicSlime);
                            labyrinthImage.labyrinthImages.Add(new Image2D(SpriteType.GEM, ImageType.GEM, toxicSlime.X, toxicSlime.Y));
                        }
                    }
                }
            }

            return GameState.PLAY;
        }
    }
}
