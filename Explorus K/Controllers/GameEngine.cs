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
		private GameView GAME_VIEW;
		private List<Binding> BINDINGS;
		private Actions CURRENT_ACTION = Actions.none;
		private int MS_PER_FRAME = 16;
		private bool EXIT = false;
		private bool PAUSED = false;
		private int LIFE_COUNT = 3;
		private int BUBBLE_COUNT = 3;
		private int GEM_COUNT = 3;

        private MapCollection MAP = new MapCollection(new string[,]{
            {"w", "w", "w", "w", "w", "w", "w", "w", "w"},
            {"w", "." ,".", ".", ".", ".", ".", ".", "w"},
            { "w", ".", "w", ".", "w", "w", "w", ".", "w"},
            { "w", "g", "w", ".", ".", ".", ".", ".", "w"},
            { "w", ".", "w", ".", "w", "w", "w", "s", "w"},
            { "w", ".", ".", ".", "w", "g", "w", "w", "w"},
            { "w", ".", "w", "w", "w", ".", ".", ".", "w"},
            { "w", ".", "w", "m", "w", ".", "w", ".", "w"},
            { "w", ".", "w", "w", "w", ".", "w", "g", "w"},
            { "w", ".", ".", ".", ".", ".", ".", ".", "w"},
            { "w", "w", "w", "w", "w", "w", "w", "w", "w"}
        });

        private Iterator MAP_ITERATOR = null;

        public GameEngine()
		{
			//The game engine get passed from contructor to constructor until it reach GameForm.cs
			GAME_VIEW = new GameView(this);
			BINDINGS = initiate_bindings();
            MAP_ITERATOR = MAP.CreateIterator();
            Thread thread = new Thread(new ThreadStart(GameLoop));
			thread.Start();
			GAME_VIEW.Show();
		}

		private List<Binding> initiate_bindings()
		{
			List<Binding> bindings = new List<Binding>();
			bindings.Add(new Binding(Keys.Up, Actions.move_up));
			bindings.Add(new Binding(Keys.Down, Actions.move_down));
			bindings.Add(new Binding(Keys.Left, Actions.move_left));
			bindings.Add(new Binding(Keys.Right, Actions.move_right));
			bindings.Add(new Binding(Keys.P, Actions.pause));
			bindings.Add(new Binding(Keys.Escape, Actions.exit));
			return bindings;
		}

		private void GameLoop()
		{
			GAME_VIEW.InitializeHeaderBar(new HealthBarCreator(), LIFE_COUNT);
			GAME_VIEW.InitializeHeaderBar(new BubbleBarCreator(), BUBBLE_COUNT);
			GAME_VIEW.InitializeHeaderBar(new GemBarCreator(), GEM_COUNT);
			GAME_VIEW.OnLoad(MAP);

			double previous = getCurrentTime();
			double lag = 0.0;

			while (!EXIT)
			{
				//Actions state machine
				systemActionsManagement();

				double current = getCurrentTime();
				double elapsed = current - previous;
				previous = current;
				lag += elapsed;

				if (!PAUSED)
				{
					characterActionsManagement();

					while (lag >= MS_PER_FRAME)
					{
						GAME_VIEW.Update(elapsed);
						lag -= MS_PER_FRAME;
					}

					//gemCount = oView.IncreaseGemBar();
					//bubbleCount = oView.DecreaseBubbleBar();
					//lifeCount = oView.DecreaseHealthBar();

					GAME_VIEW.Render();
				}

				Thread.Sleep(1);
			}

			Application.Exit();
		}

		public void systemActionsManagement()
		{
			//Actions state machine
			if (CURRENT_ACTION == Actions.none) { }
			else if (CURRENT_ACTION == Actions.pause) {
				PAUSED = !PAUSED;
				CURRENT_ACTION = Actions.none;
			}
			else if (CURRENT_ACTION == Actions.exit) {
				EXIT = true;
			}
		}

		public void characterActionsManagement()
		{
			//Actions state machine
			if (CURRENT_ACTION == Actions.none) { }
			else if (CURRENT_ACTION == Actions.move_left)
			{
                if (MAP_ITERATOR.MoveLeft())
                {
                    GAME_VIEW.getSlimusObject().moveLeft(52);
                }
				CURRENT_ACTION = Actions.none;
			}
			else if (CURRENT_ACTION == Actions.move_right)
			{
                if (MAP_ITERATOR.MoveRight())
                {
                    GAME_VIEW.getSlimusObject().moveRight(52);
                }
                CURRENT_ACTION = Actions.none;
			}
			else if (CURRENT_ACTION == Actions.move_up)
			{
                if (MAP_ITERATOR.MoveUp())
                {
                    GAME_VIEW.getSlimusObject().moveUp(52);
                }
                CURRENT_ACTION = Actions.none;
			}
			else if (CURRENT_ACTION == Actions.move_down)
			{
                if (MAP_ITERATOR.MoveDown())
                {
                    GAME_VIEW.getSlimusObject().moveDown(52);
                }
                CURRENT_ACTION = Actions.none;
			}
		}

		private long getCurrentTime()
		{
			return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}

		public void KeyEventHandler(KeyEventArgs e)
		{
			foreach(Binding binding in BINDINGS)
			{
				if(binding.Key == e.KeyCode)
				{
					CURRENT_ACTION = binding.Action;
				}
			}
		}
    }
}
