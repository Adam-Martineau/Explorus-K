﻿using Explorus_K.Game;
using Explorus_K.Game.Audio;
using Explorus_K.Models;
using Explorus_K.Threads;
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
		private Labyrinth labyrinth;
		ActionManager actionManager;
		PlayerMovement playerMovement;
		BubbleManager bubbleManager;
		private GameState gameState;
		Thread thread;
		AudioBabillard audioBabillard;
		private int gameLevel = 1;
		private bool show_fps;

        public static object gameStatelock = new object();
        Thread physicsThread;
		AudioThread audio;
		Thread audioThread;
        Thread mainThread;

		Physics physics;
        public static EventWaitHandle physicsWaitHandle;

		Render render;
        public static EventWaitHandle renderWaitHandle;

        public GameState State { get => gameState; set => gameState = value; }

        public GameEngine()
		{
			audioBabillard = new AudioBabillard();
            bubbleManager = new BubbleManager();
            labyrinth = new Labyrinth();
            //The game engine get passed from contructor to constructor until it reach GameForm.cs
            gameView = new GameView(this, gameLevel);
			bindings = initiate_bindings();
            gameState = GameState.RESUME;
			show_fps = true;
            playerMovement = new PlayerMovement(gameView.getSlimusObject().getIterator());
            actionManager = new ActionManager(this, playerMovement);

			audio = new AudioThread(audioBabillard);
            audioThread = new Thread(new ThreadStart(audio.Run));
			audioThread.Start();
            
			mainThread = new Thread(new ThreadStart(GameLoop));
			mainThread.Start();
			
			physics = new Physics(this, audioBabillard);
            physicsWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            physicsThread = new Thread(new ThreadStart(physics.startThread));
			physicsThread.Start();

			render = new Render();
			renderWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
			render = new 

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
            bindings.Add(new Binding(Keys.F, Actions.show_fps));
            return bindings;
		}

		private void GameLoop()
		{
			gameView.InitializeHeaderBar(new HealthBarCreator(), Constant.SLIMUS_LIVES, Constant.SLIMUS_LIVES);
			gameView.InitializeHeaderBar(new BubbleBarCreator(), Constant.INITIAL_BUBBLE_COUNT, Constant.INITIAL_BUBBLE_COUNT);
			gameView.InitializeHeaderBar(new GemBarCreator(), Constant.INITIAL_GEM_COUNT, 0);

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
					actionManager.characterActionsManagement(gameView, bubbleManager, audioBabillard);
					playerMovement.moveAndAnimatePlayer(gameView.getLabyrinthImage().getPlayerList());
					playerMovement.moveAndAnimateBubbles(bubbleManager, audioBabillard);

					if (lag >= MS_PER_FRAME)
					{

						float fps = 1000f / (float)lag;

						while (lag >= MS_PER_FRAME)
						{
							gameView.Update(show_fps, fps);
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
            gameView.Pause();
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

        public void showingFPS()
        {
            show_fps = !show_fps;
        }

		public void setMusicVolume(int volume)
		{
            audioBabillard.AddMessage(AudioName.SET_MUSIC, volume);
        }

        public void setSoundVolume(int volume)
        {
            audioBabillard.AddMessage(AudioName.SET_SOUND, volume);
        }

        public void restart()
		{
            gameLevel += 1;
            bubbleManager = new BubbleManager();
            labyrinth = new Labyrinth();
            int remainingLifes = gameView.getLabyrinthImage().HealthBar.getCurrent();
            if (gameState == GameState.STOP)
			{
				gameLevel = 1;
				remainingLifes = Constant.SLIMUS_LIVES;
			}
            gameView.Restart(this, gameLevel);
            playerMovement = new PlayerMovement(gameView.getSlimusObject().getIterator());
            actionManager = new ActionManager(this, playerMovement);
            physicsThread.Abort();
            Physics physics = new Physics(this, audioBabillard);
            physicsWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            physicsThread = new Thread(new ThreadStart(physics.startThread));
            physicsThread.Start();
            gameView.InitializeHeaderBar(new HealthBarCreator(), Constant.SLIMUS_LIVES, remainingLifes);
            gameView.InitializeHeaderBar(new BubbleBarCreator(), Constant.INITIAL_BUBBLE_COUNT, Constant.INITIAL_BUBBLE_COUNT);
            gameView.InitializeHeaderBar(new GemBarCreator(), Constant.INITIAL_GEM_COUNT, 0);
            resume();
        }
    }
}
