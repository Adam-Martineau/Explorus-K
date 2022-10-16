using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Replay
{
    internal class Invoker
    {
        List<ICommand> commands;

        public Invoker()
        {
            commands = new List<ICommand>();
        }


        public void executeCommand(ICommand command)
        {
            addCommandToList(command);
            command.execute();
        }

        public List<ICommand> getCommands()
        {
            return commands;
        }

        public void undo(int step)
        {
            for(int i = 0; i < step; i++)
            {
                commands[commands.Count - i - 1].unexecute();
            }
        }

        private void addCommandToList(ICommand command)
        {
            checkFirstCommandTimestamp();
            commands.Add(command);
        }

        private void checkFirstCommandTimestamp()
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            bool noMoreOld = true;

            if(commands.Count > 45)
            {
                  int i = 0;
            }

            while (noMoreOld)
            {
                if (commands.Count > 0)
                {
                    long result = now - commands[0].getCommandTimestamp();

                    if (result > 5000)
                    {
                        commands.RemoveAt(0);
                    }
                    else
                    {
                        noMoreOld = false;
                    }
                }
                else
                {
                    noMoreOld= false;
                }
            }
        }
    }
}
