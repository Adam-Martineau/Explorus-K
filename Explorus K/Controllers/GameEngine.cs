using Explorus_K.Models;
using Explorus_K.Views;
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

        private int lifeCount = 3;
        private int bubbleCount = 3;
        private int gemCount = 3;

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
            bindings.Add(new Binding(Keys.P, Actions.pause));
            bindings.Add(new Binding(Keys.R, Actions.unpause));
            return bindings;
        }

        private void GameLoop()
        {
            oView.InitializeHeaderBar(new HealthBarCreator(), lifeCount);
            oView.InitializeHeaderBar(new BubbleBarCreator(), bubbleCount);
            oView.InitializeHeaderBar(new GemBarCreator(), gemCount);
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

                gemCount = oView.IncreaseGemBar();
                //bubbleCount = oView.DecreaseBubbleBar();
                //lifeCount = oView.DecreaseHealthBar();

                oView.Render();
                Thread.Sleep(1);
            }
        }

        public void actionManagement()
        {
            //Actions state machine
            if (currentAction == Actions.none) { }
            else if (currentAction == Actions.move_left) {
                if (oView.MoveToLeft())
                {
                    oView.getSlimusObject().moveLeft(52);
                }
                currentAction = Actions.none;
            }
            else if (currentAction == Actions.move_right) {
                if (oView.MoveToRight())
                {
                    oView.getSlimusObject().moveRight(52);
                }
                currentAction = Actions.none;
            }
            else if (currentAction == Actions.move_up) {
                if (oView.MoveToUp())
                {
                    oView.getSlimusObject().moveUp(52);
                }
                currentAction = Actions.none;
            }
            else if (currentAction == Actions.move_down) {
                if (oView.MoveToDown())
                {
                    oView.getSlimusObject().moveDown(52);
                }
                currentAction = Actions.none;
            }
            else if (currentAction == Actions.pause) { 
                currentAction = Actions.none;
            }
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
