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
		private int MS_PER_FRAME = 200;
		private bool EXIT = false;
		private bool PAUSED = false;
		private int LIFE_COUNT = 3;
		private int BUBBLE_COUNT = 3;
		private int GEM_COUNT = 3;

		//Time space continum related global variables
		private double start_time = 0;
		private int count = 0;
		int next_expected_pos = 0;

		private MapCollection MAP = new MapCollection(new string[,]{
            { "w", "w", "w", "w", "w", "w", "w", "w", "w"},
            { "w", "." ,".", ".", ".", ".", ".", ".", "w"},
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

			double previous_time = getCurrentTime();
			double lag = 0.0;

			while (!EXIT)
			{
				//Actions state machine
				systemActionsManagement();

				double current_time = getCurrentTime();
				double elapsed_time = current_time - previous_time;
				previous_time = current_time;
				lag += elapsed_time;

				if (!PAUSED)
				{
					characterActionsManagement(elapsed_time);

					while (lag >= MS_PER_FRAME)
					{
						GAME_VIEW.Update(elapsed_time);
						lag -= MS_PER_FRAME;
					}

					GAME_VIEW.Render();
				}

				Thread.Sleep(199);
			}

			Application.Exit();
		}

		//Receving the event from a keypress and checking if we have a action bind to that key
		public void KeyEventHandler(KeyEventArgs e)
		{
			foreach(Binding binding in BINDINGS)
			{
				if(binding.Key == e.KeyCode)
				{
					actionHandler(binding.Action);
				}
			}
		}
		
		//If we have a action bind to that kay, we check if that action can be done
		public void actionHandler(Actions action)
        {
			if (action == Actions.pause || action == Actions.exit)
				CURRENT_ACTION = action;
			else if (action == Actions.move_left && MAP_ITERATOR.MoveLeft() && CURRENT_ACTION == Actions.none)
			{
				CURRENT_ACTION = action;
				next_expected_pos = (MAP_ITERATOR.Current()[0] * 52) - 52;
				start_time = getCurrentTime();
			}
			else if (action == Actions.move_right && MAP_ITERATOR.MoveRight() && CURRENT_ACTION == Actions.none)
				CURRENT_ACTION = action;
			else if (action == Actions.move_up && MAP_ITERATOR.MoveUp() && CURRENT_ACTION == Actions.none)
				CURRENT_ACTION = action;
			else if (action == Actions.move_down && MAP_ITERATOR.MoveDown() && CURRENT_ACTION == Actions.none)
				CURRENT_ACTION = action;
		}

		//If the action can be done, we use a state machine to wait until the action is over
		public void characterActionsManagement(double elapsed_time)
		{
			//Actions state machine
			if (CURRENT_ACTION == Actions.none) { }
			else if (CURRENT_ACTION == Actions.move_left)
			{
				if (count < 52)
				{
					count++;
					GAME_VIEW.getSlimusObject().moveLeft(1);
				}
				else
				{
					count = 0;
					CURRENT_ACTION = Actions.none;
				}
			}
			else if (CURRENT_ACTION == Actions.move_right)
			{
				GAME_VIEW.getSlimusObject().moveRight(52);
				CURRENT_ACTION = Actions.none;

			}
			else if (CURRENT_ACTION == Actions.move_up) 
			{
				GAME_VIEW.getSlimusObject().moveUp(52);
				CURRENT_ACTION = Actions.none;
			}
			else if (CURRENT_ACTION == Actions.move_down) 
			{
				GAME_VIEW.getSlimusObject().moveDown(52);
				CURRENT_ACTION = Actions.none;
			}
		}

		public void systemActionsManagement()
		{
			//Actions state machine
			if (CURRENT_ACTION == Actions.none) { }
			else if (CURRENT_ACTION == Actions.pause)
			{
				PAUSED = !PAUSED;
				CURRENT_ACTION = Actions.none;
			}
			else if (CURRENT_ACTION == Actions.exit)
			{
				EXIT = true;
			}
		}

		//Return the time in ms
		private long getCurrentTime()
		{
			return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}
	}
}
