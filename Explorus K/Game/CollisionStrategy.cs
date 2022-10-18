using Explorus_K.Game;
using Explorus_K.Game;
using Explorus_K.Game.Audio;
using Explorus_K.Game.Replay;
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

        public GameState executeStrategy(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio, Invoker commandInvoker)
        {
            return _strategy.execute(labyrinthImage, imageIndex, type, audio, commandInvoker);
        }
    }

    public interface IStrategy
    {
        GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio, Invoker commandInvoker);
    }

    class GemStrategy : IStrategy
    {
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio, Invoker commandInvoker)
        {
            if(type == SpriteType.SLIMUS)
            {
                commandInvoker.executeCommand(new GemCollectingCommand(labyrinthImage.GemBar));
                if (labyrinthImage.GemBar.getCurrent() == labyrinthImage.GemBar.getLength())
                {
                    labyrinthImage.KeyState.RequestChangingState();
                    Point pos = labyrinthImage.getSlimus().getIterator().getPosition("p");
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
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio, Invoker commandInvoker)
        {
            if (type == SpriteType.SLIMUS)
            {
                if (labyrinthImage.KeyState.CurrentState() == "WithKeyState")
                {
                    Point pos = labyrinthImage.getSlimus().getIterator().getPosition("l");
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
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio, Invoker commandInvoker)
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
        public GameState execute(LabyrinthImage labyrinthImage, int imageIndex, SpriteType type, AudioBabillard audio, Invoker commandInvoker)
        {
            if (type == SpriteType.SLIMUS)
            {
                commandInvoker.executeCommand(new DecreaseLifeCommand(labyrinthImage.getSlimus()));
                commandInvoker.executeCommand(new DecreaseHealthBar(labyrinthImage.HealthBar));
                
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
                        commandInvoker.executeCommand(new DecreaseLifeCommand(player));
                        audio.AddMessage(AudioName.GETTING_HIT);
                        if (player.getLifes() < 1)
                        {
                            commandInvoker.executeCommand(new ToxicSlimeDeadCommand(labyrinthImage, player, toxicSlime));
                        }
                    }
                }
            }

            return GameState.PLAY;
        }
    }
}
