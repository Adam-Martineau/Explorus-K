using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Replay
{
    internal class SlimusCommand : ICommand
    {
        Player slimus;
        MovementDirection direction;
        long timestamp;

        public SlimusCommand(Player slimus, MovementDirection movementDirection)
        {
            this.slimus = slimus;
            this.timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.direction = movementDirection;
        }

        public void execute()
        {
            Receiver receiver = new Receiver();
            receiver.computePlayerCommand(slimus, direction);
        }

        public void unexecute()
        {
            Receiver receiver = new Receiver();
            receiver.computePlayerCommand(slimus, getOppositeMovementDirection(direction));
        }

        public long getCommandTimestamp()
        {
            return timestamp;
        }

        private MovementDirection getOppositeMovementDirection(MovementDirection movementDirection)
        {
            switch (movementDirection)
            {
                case MovementDirection.down:
                    return MovementDirection.up;
                case MovementDirection.up:
                    return MovementDirection.down;
                case MovementDirection.left:
                    return MovementDirection.right;
                case MovementDirection.right:
                    return MovementDirection.left;
                default:
                    return MovementDirection.none;
            }
        }
    }
}
