using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Audio
{
    public interface IListener
    {
        void Notify();
        void Process((string,int) fileName);
    }
}
