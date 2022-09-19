using Explorus_K.Game;
using Explorus_K.Models;
using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Explorus_K.Controllers
{
	public class GameEngine
	{
		private const int MS_PER_FRAME = 16;

		public GameView gameView { get; set; }
		private List<Binding> bindings;
		private int lifeCount = 6;
		private int bubbleCount = 6;
		private int gemCount = 6;
		private Labyrinth labyrinth;
		ActionManager actionManager;
		PlayerMovement playerMovement;
		BubbleManager bubbleManager;
		private GameState gameState;

        Thread physicsThread;
		Thread thread;

        public static EventWaitHandle physicsWaitHandle;

        public GameState State { get => gameState; set => gameState = value; }

        public GameEngine()

		{
            bubbleManager = new BubbleManager();
            labyrinth = new Labyrinth();
            //The game engine get passed from contructor to constructor until it reach GameForm.cs
            gameView = new GameView(this);
			bindings = initiate_bindings();
            gameState = GameState.RESUME;
            playerMovement = new PlayerMovement(gameView.getSlimusObject().getIterator());
            actionManager = new ActionManager(this, playerMovement);
            
			thread = new Thread(new ThreadStart(GameLoop));
			thread.Start();
			
			Physics physics = new Physics(this);
            physicsWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            physicsThread = new Thread(new ThreadStart(physics.startThread));
			physicsThread.Start();

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
            bindings.Add(new Binding(Keys.R, Actions.resume));
            bindings.Add(new Binding(Keys.Escape, Actions.exit));
            bindings.Add(new Binding(Keys.Space, Actions.shoot));
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

				if (gameState == GameState.PLAY)
				{
					actionManager.characterActionsManagement(gameView, bubbleManager);
					playerMovement.moveAndAnimatePlayer(gameView.getLabyrinthImage().getPlayerList());
					playerMovement.moveAndAnimateBubbles(bubbleManager);

					if (lag >= MS_PER_FRAME)
					{

						float fps = 1000f / (float)lag;

						while (lag >= MS_PER_FRAME)
						{
							gameView.Update(fps);
							lag -= MS_PER_FRAME;
						}
						
						gameView.Render();
						physicsWaitHandle.Set();
					}

					Thread.Sleep(1);
				}
				else
				{
                    gameView.Render();
                    Thread.Sleep(1);

                    if (gameState == GameState.STOP || gameState == GameState.RESTART)
                    {
						Thread.Sleep(3000);
						restart();
                    }
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
					actionManager.actionHandler(binding.Action, gameView.getSlimusObject().getIterator());
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

		public BubbleManager getBubbleManager()
		{
			return this.bubbleManager;
		}

		public void pause()
		{
			gameState = GameState.PAUSE;
		}

		public void resume()
		{
            gameState = GameState.RESUME;
			gameView.Resume();
        }

        public void play()
        {
            gameState = GameState.PLAY;
        }

        public void stop()
        {
            gameState = GameState.STOP;
        }

		public void restart()
		{
            bubbleManager = new BubbleManager();
            labyrinth = new Labyrinth();
            playerMovement = new PlayerMovement(gameView.getSlimusObject().getIterator());
            actionManager = new ActionManager(this, playerMovement);
            gameView.Restart(this);
            gameView.InitializeHeaderBar(new HealthBarCreator(), lifeCount);
            gameView.InitializeHeaderBar(new BubbleBarCreator(), bubbleCount);
            gameView.InitializeHeaderBar(new GemBarCreator(), gemCount);
            resume();
        }
    }
}
