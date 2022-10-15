using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game
{
    internal class MenuOption
    {
        private MenuCursor type;
        private Bitmap[] image;

        public MenuOption(MenuCursor type, Bitmap[] image)
        {
            this.Type = type;
            this.Image = image;
        }

        public MenuCursor Type { get => type; set => type = value; }
        public Bitmap[] Image { get => image; set => image = value; }
    }
}
