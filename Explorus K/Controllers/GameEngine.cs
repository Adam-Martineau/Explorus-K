using Explorus_K.Game;
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
		private const int MS_PER_FRAME = 16;

		private GameView gameView;
		private List<Binding> bindings;
		private int lifeCount = 6;
		private int bubbleCount = 6;
		private int gemCount = 6;
		private Labyrinth labyrinth;
		ActionManager actionManager;
        private bool paused = false;

        public bool Paused { get => paused; set => paused = value; }

        public GameEngine()

		{
            labyrinth = new Labyrinth();
            //The game engine get passed from contructor to constructor until it reach GameForm.cs
            gameView = new GameView(this);
			bindings = initiate_bindings();
			actionManager = new ActionManager(this);
            Thread thread = new Thread(new ThreadStart(GameLoop));
			thread.Start();
			gameView.Show();
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
			gameView.InitializeHeaderBar(new HealthBarCreator(), lifeCount);
			gameView.InitializeHeaderBar(new BubbleBarCreator(), bubbleCount);
			gameView.InitializeHeaderBar(new GemBarCreator(), gemCount);

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

				if (!Paused)
				{
					actionManager.characterActionsManagement(gameView, labyrinth.MapIterator);

					if (lag >= MS_PER_FRAME)
					{

						float fps = 1000f / (float)lag;

						while (lag >= MS_PER_FRAME)
						{
							gameView.Update(fps, labyrinth.MapIterator);
							lag -= MS_PER_FRAME;
						}

						gameView.Render();
					}

					Thread.Sleep(1);
				}
			}
		}

		//Receving the event from a keypress and checking if we have a action bind to that key
		internal void KeyEventHandler(KeyEventArgs e)
		{
			foreach(Binding binding in bindings)
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

		public Labyrinth GetLabyrinth()
		{
			return this.labyrinth;
		}

		public void pause()
		{
			paused = true;
		}

		public void unpause()
		{
			paused = false;
		}
	}
}
