using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Replay
{
    internal class BubbleCommand : ICommand
    {
        Bubble bubble;
        BubbleManager bubbleManager;
        long timestamp;

        public BubbleCommand(Bubble bubble, BubbleManager bubbleManager)
        {
            this.bubble = bubble;
            this.timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.bubbleManager = bubbleManager;
        }

        public void execute()
        {
            Receiver receiver = new Receiver();
            receiver.computeBubbleCommand(bubble, bubbleManager);
        }

        public void unexecute()
        {
            //do nothing
        }

        public long getCommandTimestamp()
        {
            return timestamp;
        }
    }
}
