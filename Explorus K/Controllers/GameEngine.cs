using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Explorus_K.Controllers
{
    class GameEngine
    {
        private GameView oView;
        private int MS_PER_FRAME = 16;

        public GameEngine()
        {
            oView = new GameView();
            Thread thread = new Thread(new ThreadStart(GameLoop));
            thread.Start();
            oView.Show();
        }

        private void GameLoop()
        {
            //Thread.Sleep(5000);

            double previous = getCurrentTime();
            double lag = 0.0;
            while (true)
            {
                double current = getCurrentTime();
                double elapsed = current - previous;
                previous = current;
                lag += elapsed;

                while (lag >= MS_PER_FRAME)
                {
                    oView.Update(elapsed);
                    lag -= MS_PER_FRAME;
                }

                //double FPS = 1000 / elapsed;
                //oView.setWindowTitle("Explorus-K FPS " + FPS.ToString());
                oView.Render();
                Thread.Sleep(1);
            }
        }

        private long getCurrentTime()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    
        private void goUp()
        {

        }

        private void goDown()
        {

        }

        private void goLeft()
        {

        }

        private void goRight()
        {

        }

        private void pauseGame()
        {

        }
    }
}
