using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Replay
{
    internal class GemCollectingCommand : ICommand
    {
        GemBar gemBar;
        LabyrinthImage labyrinthImage;
        Image2D image2D;
        long timestamp;

        public GemCollectingCommand(GemBar gemBar, LabyrinthImage labyrinthImage, Image2D image2D)
        {
            this.gemBar = gemBar;
            this.labyrinthImage = labyrinthImage;
            this.image2D = image2D;
            this.timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void execute()
        {
            Receiver receiver = new Receiver();
            receiver.computeGemCollectingCommand(gemBar, labyrinthImage, image2D, true);
        }

        public void unexecute()
        {
            Receiver receiver = new Receiver();
            receiver.computeGemCollectingCommand(gemBar, labyrinthImage, image2D, false);
        }

        public long getCommandTimestamp()
        {
            return timestamp;
        }
    }
}
