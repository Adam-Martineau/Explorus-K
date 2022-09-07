using Explorus_K.Game;
using Explorus_K.Models;
using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Explorus_K.Controllers
{
	public class GameEngine
	{
		private GameView GAME_VIEW;
		private Labyrinth labyrinth;
		private ActionManager actionManager;
		private List<Binding> BINDINGS;
		
		private int MS_PER_FRAME = 16;
		private int LIFE_COUNT = 3;
		private int BUBBLE_COUNT = 3;
		private int GEM_COUNT = 3;
		private int ANIMATION_COUNT = 0;

		//Time space continum related global variables
		private double start_time = 0;
		int next_expected_pos = 0;

        public GameEngine()

		{
			//The game engine get passed from contructor to constructor until it reach GameForm.cs
			GAME_VIEW = new GameView(this);
			labyrinth = new Labyrinth();
			actionManager = new ActionManager();
			BINDINGS = initiate_bindings();
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
			GAME_VIEW.OnLoad(labyrinth.Map);

			double previous_time = getCurrentTime();
			double lag = 0.0;

			while (true)
			{
				//Actions state machine
				actionManager.systemActionsManagement();

				double current_time = getCurrentTime();
				double elapsed_time = current_time - previous_time;
				previous_time = current_time;
				lag += elapsed_time;

				if (!actionManager.Paused)
				{
					actionManager.characterActionsManagement(GAME_VIEW, labyrinth.MapIterator);

					if (lag >= MS_PER_FRAME)
					{

						float fps = 1000f / (float)lag;

						while (lag >= MS_PER_FRAME)
						{
							GAME_VIEW.Update(fps, labyrinth.MapIterator);
							lag -= MS_PER_FRAME;
						}

						GAME_VIEW.Render();
					}

					Thread.Sleep(1);
				}
			}

			Application.Exit();
		}

		internal void resize()
		{
			if (GAME_VIEW != null)
				GAME_VIEW.resize();
		}

		//Receving the event from a keypress and checking if we have a action bind to that key
		internal void KeyEventHandler(KeyEventArgs e)
		{
			foreach(Binding binding in BINDINGS)
			{
				if(binding.Key == e.KeyCode)
				{
					actionManager.actionHandler(binding.Action, labyrinth.MapIterator);
				}
			}
		}

		//Return the time in ms
		private long getCurrentTime()
		{
			return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}
	}
}
