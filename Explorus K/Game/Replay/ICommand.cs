﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Replay
{
    internal interface ICommand
    {
        void execute();

        void unexecute();

        long getCommandTimestamp();
    }
}
