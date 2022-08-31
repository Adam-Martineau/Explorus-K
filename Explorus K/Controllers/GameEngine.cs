using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Explorus_K.Controllers
{
    class GameEngine
    {
        private GameView oView;
        public GameEngine()
        {
            oView = new GameView();
            Thread thread = new Thread(new ThreadStart(GameLoop));
            thread.Start();
            oView.Show();
        }

        private void GameLoop()
        {
            oView.Render();
            Thread.Sleep(5000);
            oView.Close();
        }

    }
}
