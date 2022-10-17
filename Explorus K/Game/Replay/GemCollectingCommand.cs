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
        long timestamp;

        public GemCollectingCommand(GemBar gemBar)
        {
            this.gemBar = gemBar;
            this.timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void execute()
        {
            Receiver receiver = new Receiver();
            receiver.computeGemCollectingCommand(gemBar, true);
        }

        public void unexecute()
        {
            Receiver receiver = new Receiver();
            receiver.computeGemCollectingCommand(gemBar, false);
        }

        public long getCommandTimestamp()
        {
            return timestamp;
        }
    }
}
