using Explorus_K.Controllers;
using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameEngine ge = new GameEngine(GameMap.get());
            ge.StartGame();
            System.Environment.Exit(0);
        }
    }
}
