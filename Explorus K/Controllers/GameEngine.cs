﻿using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Explorus_K.Controllers
{
    public class GameEngine
    {
        private Actions currentAction = Actions.none;
        private GameView oView;
        private int MS_PER_FRAME = 16;
        private List<Binding> bindings;
        private Actions game_state = Actions.none;

        public GameEngine()
        {
            //The game engine get passed from contructor to constructor until it reach GameForm.cs
            oView = new GameView(this);
            bindings = initiate_bindings();
            Thread thread = new Thread(new ThreadStart(GameLoop));
            thread.Start();
            oView.Show();
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

        private void GameLoop()
        {
            oView.OnLoad();

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

                //Actions state machine
                actionManagement();

                oView.Render();
                Thread.Sleep(1);
            }
        }

        public void actionManagement()
        {
            //Actions state machine
            if (currentAction == Actions.none) { }
            else if (currentAction == Actions.move_left) { MessageBox.Show(Actions.move_left.ToString()); }
            else if (currentAction == Actions.move_right) { MessageBox.Show(Actions.move_right.ToString()); }
            else if (currentAction == Actions.move_up) { MessageBox.Show(Actions.move_up.ToString()); }
            else if (currentAction == Actions.move_down) { MessageBox.Show(Actions.move_down.ToString()); }
            else if (currentAction == Actions.pause) { MessageBox.Show(Actions.pause.ToString()); }
        }

        private long getCurrentTime()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public void KeyEventHandler(KeyEventArgs e)
        {
            foreach(Binding binding in bindings)
            {
                if(binding.Key == e.KeyCode)
                {
                    actionHandler(binding.Action);
                }
            }
        }

        public void actionHandler(Actions action)
        {
            currentAction = action;
        }
    }
}
