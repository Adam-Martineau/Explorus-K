using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Replay
{
    internal class DecreaseHealthBar : ICommand
    {
        HealthBar healthBar;
        long timestamp;

        public DecreaseHealthBar(HealthBar gemBar)
        {
            this.healthBar = gemBar;
            this.timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void execute()
        {
            Receiver receiver = new Receiver();
            receiver.computeDecreaseHealthBarCommand(healthBar, true);
        }

        public void unexecute()
        {
            Receiver receiver = new Receiver();
            receiver.computeDecreaseHealthBarCommand(healthBar, false);
        }

        public long getCommandTimestamp()
        {
            return timestamp;
        }
    }
}
