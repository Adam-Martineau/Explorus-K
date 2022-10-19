using Explorus_K.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game
{
    internal class MenuOptions
    {
        private List<(MenuCursor, Bitmap[])> menuOptions;
        private int current;

        public MenuOptions()
        {
            menuOptions = new List<(MenuCursor, Bitmap[])>();
            current = 0;
        }

        public void addOption(MenuCursor menuCursor, Bitmap[] bitmap)
        {
            menuOptions.Add((menuCursor, bitmap));
        }

        public void cursorUp()
        {
            if (current > 0)
            {
                current -= 1;
            }
        }

        public void cursorDown()
        {
            if (current < menuOptions.Count - 1)
            {
                current += 1;
            }
        }

        public int getLength()
        {
            return menuOptions.Count;
        }

        public int getCurrentIndex()
        {
            return current;
        }

        public MenuCursor getType(int index)
        {
            return menuOptions[index].Item1;
        }

        public MenuCursor getCurrentType()
        {
            return menuOptions[current].Item1;
        }

        public Bitmap[] getBitmap(int index)
        {
            return menuOptions[index].Item2;
        }
    }
}
