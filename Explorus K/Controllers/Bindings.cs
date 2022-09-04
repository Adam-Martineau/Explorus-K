using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K.Controllers
{
    enum Actions
    {
        move_up,
        move_down,
        move_left,
        move_right,
        pause
    }

    internal class Bindings
    {
        private Keys key;
        private Actions action;
        
        public Bindings(Keys key, Actions action)
        {
            this.key = key;
            this.action = action;
        }

        public Keys Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        public Actions Action 
        { 
            get { return this.action; } 
            set { action = value; }
        }
    }
}
