using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Replay
{
    internal class ToxicSlimeDeadCommand : ICommand
    {
        LabyrinthImage labyrinthImage;
        Player player;
        Image2D image2D;
        long timestamp;

        public ToxicSlimeDeadCommand(LabyrinthImage labyrinthImage, Player player, Image2D image2D)
        {
            this.labyrinthImage = labyrinthImage;
            this.player = player;
            this.image2D = image2D;
            this.timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void execute()
        {
            Receiver receiver = new Receiver();
            receiver.computeToxicSlimeDeadCommand(labyrinthImage, player, image2D, false);
        }

        public void unexecute()
        {
            Receiver receiver = new Receiver();
            receiver.computeToxicSlimeDeadCommand(labyrinthImage, player, image2D, true);
        }

        public long getCommandTimestamp()
        {
            return timestamp;
        }
    }
}
