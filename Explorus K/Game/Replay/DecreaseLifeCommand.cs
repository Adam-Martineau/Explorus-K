using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Replay
{
    internal class DecreaseLifeCommand : ICommand
    {
        Player player;
        long timestamp;

        public DecreaseLifeCommand(Player player)
        {
            this.player = player;
            this.timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void execute()
        {
            Receiver receiver = new Receiver();
            receiver.computeDecreaseLifeCommand(player, true);
        }

        public void unexecute()
        {
            Receiver receiver = new Receiver();
            receiver.computeDecreaseLifeCommand(player, false);
        }

        public long getCommandTimestamp()
        {
            return timestamp;
        }
    }
}
