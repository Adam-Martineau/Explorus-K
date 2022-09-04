using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Explorus_K.Controllers
{
    class GameEngine
    {
        private GameView oView;
        private int MS_PER_FRAME = 16;
        private List<Binding> Bindings;
        private Actions game_state = Actions.none;

        public GameEngine()
        {
            oView = new GameView();
            Thread thread = new Thread(new ThreadStart(GameLoop));
            thread.Start();
            oView.Show();
            Bindings = initiate_bindings();
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

                //Key input management
                //Ceci est de la merde, il faut que l'event de la key est lieu dans le GameForm.cs
                //Je sais pas comment l'apporter ici
                foreach(Binding binding in Bindings)
                {
                    if() //check the key of the Binding
                    {
                        //Do something
                    }
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
    
        private List<Binding> initiate_bindings()
        {
            List<Binding> bindings = new List<Binding>();
            bindings.Add(new Binding(Keys.Up, Actions.move_up));
            bindings.Add(new Binding(Keys.Down, Actions.move_down));
            bindings.Add(new Binding(Keys.Left, Actions.move_left));
            bindings.Add(new Binding(Keys.Right, Actions.move_right));
            bindings.Add(new Binding(Keys.Escape, Actions.pause));
            return bindings;
        }
    }
}
