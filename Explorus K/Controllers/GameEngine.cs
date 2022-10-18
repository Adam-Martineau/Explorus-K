using Explorus_K.Game;
using Explorus_K.Game.Audio;
using Explorus_K.Game.Replay;
using Explorus_K.Models;
using Explorus_K.Threads;
using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		AudioBabillard audioBabillard;
		PhysicsThread physics;
        private int gameLevel = 1;
		private bool show_fps;
		private int musicVolume;
        private int soundVolume;
		private bool muteMusic;
        private bool muteSound;
		private GameDifficulty difficulty;
		private bool undoDone = false;

        public static object gameStatelock = new object();
        Thread physicsThread;
		AudioThread audio;
		Thread audioThread;
        Thread mainThread;
        Invoker commandInvoker;

        public GameState State { get => gameState; set => gameState = value; }
        public int MusicVolume { get => musicVolume; set => musicVolume = value; }
        public int SoundVolume { get => soundVolume; set => soundVolume = value; }
        public bool MuteMusic { get => muteMusic; set => muteMusic = value; }
        public bool MuteSound { get => muteSound; set => muteSound = value; }
        public GameDifficulty GameDifficulty { get => difficulty; set => difficulty = value; }

        public GameEngine()
		{
            commandInvoker = new Invoker();
            audioBabillard = new AudioBabillard();
            gameState = GameState.MENU;
			difficulty = new GameDifficulty();
            audioBabillard = new AudioBabillard();
            bubbleManager = new BubbleManager(difficulty.getBubbleTimer());
            labyrinth = new Labyrinth();
            //The game engine get passed from contructor to constructor until it reach GameForm.cs
            gameView = new GameView(this, gameLevel);
			bindings = initiate_bindings();
            
			show_fps = true;
			musicVolume = Constant.MUSIC_VOLUME;
			soundVolume = Constant.SOUND_VOLUME;
			muteMusic = false;
            muteSound = false;
            playerMovement = new PlayerMovement(gameView.getSlimusObject().getIterator());
            actionManager = new ActionManager(this, playerMovement);

			audio = new AudioThread(audioBabillard);
            audioThread = new Thread(new ThreadStart(audio.Run));
			
			mainThread = new Thread(new ThreadStart(GameLoop));
			
            physics = new PhysicsThread(this.gameView.labyrinthImage, audioBabillard, commandInvoker);
            physicsThread = new Thread(new ThreadStart(physics.startThread));
			
			physicsThread.Start();
            audioThread.Start();
            mainThread.Start();

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
			bindings.Add(new Binding(Keys.Enter, Actions.select_menu));
            bindings.Add(new Binding(Keys.M, Actions.mute));
            return bindings;
		}

		private void GameLoop()
		{
            gameView.InitializeHeaderBar(new HealthBarCreator(), difficulty.getSlimusLives(), difficulty.getSlimusLives());
            gameView.InitializeHeaderBar(new BubbleBarCreator(), Constant.INITIAL_BUBBLE_COUNT, Constant.INITIAL_BUBBLE_COUNT);
            gameView.InitializeHeaderBar(new GemBarCreator(), Constant.INITIAL_GEM_COUNT, 0);

            double previous_time = getCurrentTime();
			double lag = 0.0;

            bool replayInitiated = false;
			double elapsedTimeCombined = 0;
            long firstListTimestamp = 0;

            while (true)
			{
				//Actions state machine
				if (State == GameState.MENU || State == GameState.PAUSE)
				{
                    actionManager.systemMenuManagement(gameView);
                }
				else
				{
                    actionManager.systemActionsManagement();
                }

				double current_time = getCurrentTime();
				double elapsed_time = current_time - previous_time;
				previous_time = current_time;
				lag += elapsed_time;

				if (gameState == GameState.PLAY)
				{
					actionManager.characterActionsManagement(gameView, bubbleManager, audioBabillard, commandInvoker);
					playerMovement.moveAndAnimatePlayers(gameView.getLabyrinthImage().getPlayerList(), commandInvoker, gameState);
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
						physics.Notify();
						gameState = physics.getGameState();
                    }

					Thread.Sleep(1);
				}
				else if(gameState == GameState.REPLAY)
				{
					List<ICommand> commands = commandInvoker.getCommands();

					elapsedTimeCombined +=  13;

					if(!replayInitiated)
					{
                        firstListTimestamp = commands[0].getCommandTimestamp();
						replayInitiated = true;
                    }

                    foreach (ICommand command in new List<ICommand>(commands))
					{
						long millisecondsBeetwenComand = command.getCommandTimestamp() - firstListTimestamp;

						if(millisecondsBeetwenComand > elapsedTimeCombined)
						{
							break;
						}
						else
						{
							command.execute();
							commands.Remove(command);
						}
					}

                    playerMovement.moveAndAnimatePlayers(gameView.getLabyrinthImage().getPlayerList(), null, gameState);
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
                        physics.Notify();
                        //gameState = physics.getGameState();
                    }

                    if (commands.Count == 0)
					{
                        gameState = GameState.MENU;
						replayInitiated = false;
					}

                    Thread.Sleep(1);

                }
				else if(gameState == GameState.UNDO)
				{
                    physicsThread.Abort();
                    

                    if (!undoDone)
					{
						foreach(Player player in gameView.getLabyrinthImage().getPlayerList())
						{
							player.setMovementDirection(MovementDirection.none);
						}

                        for (int i = 0; i < commandInvoker.getCommands().Count; i++)
                        {
                            commandInvoker.getCommands()[commandInvoker.getCommands().Count - i - 1].unexecute();
                            playerMovement.moveAndAnimatePlayers(gameView.getLabyrinthImage().getPlayerList(), null, gameState);
                        }

                        gameState = GameState.REPLAY;
						undoDone = true;
                        gameView.Render();
                    }
					else
					{
						gameState = GameState.REPLAY;
					}

                    Thread.Sleep(50);

                    physics = new PhysicsThread(gameView.labyrinthImage, audioBabillard, null);
                    physicsThread = new Thread(new ThreadStart(physics.startThread));
                    physicsThread.Start();

                    Thread.Sleep(50);
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
					if (State == GameState.MENU || State == GameState.PAUSE)
					{
                        actionManager.menuHandler(binding.Action);
                    }
					else
					{
                        actionManager.actionHandler(binding.Action, gameView.getSlimusObject().getIterator());
                    }
					
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

		public void downMusicVolume()
		{
			if (musicVolume > 0 && !muteMusic)
			{
				musicVolume -= 1;
				audioBabillard.AddMessage(AudioName.SET_MUSIC, musicVolume);
			}
        }

        public void upMusicVolume()
        {
			if (musicVolume < 100 && !muteMusic)
			{
                musicVolume += 1;
                audioBabillard.AddMessage(AudioName.SET_MUSIC, musicVolume);
            }
        }

        public void muteMusicVolume()
        {
			if (!muteMusic)
			{
                audioBabillard.AddMessage(AudioName.SET_MUSIC, 0);
				muteMusic = true;	
            }
			else
			{
                audioBabillard.AddMessage(AudioName.SET_MUSIC, musicVolume);
                muteMusic = false;
            }
        }

        public void downSoundVolume()
        {
			if (soundVolume > 0 && !muteSound)
			{
                soundVolume -= 1;
                audioBabillard.AddMessage(AudioName.SET_SOUND, soundVolume);
            }
        }

        public void upSoundVolume()
        {
			if (soundVolume < 100 && !muteSound)
			{
				soundVolume += 1;
				audioBabillard.AddMessage(AudioName.SET_SOUND, soundVolume);
			}
        }

        public void muteSoundVolume()
        {
			if (!muteSound)
			{
                audioBabillard.AddMessage(AudioName.SET_SOUND, 0);
                muteSound = true;
            }
			else
			{
                audioBabillard.AddMessage(AudioName.SET_SOUND, soundVolume);
                muteSound = false;
            }
        }

		public void changeDifficulty()
		{
            difficulty.changeDifficulty();
			playerMovement.setPlayerSpeed(difficulty.getPlayerSpeed());
        }

        public void restart()
		{
            gameLevel += 1;
            bubbleManager = new BubbleManager(difficulty.getBubbleTimer());
            labyrinth = new Labyrinth();
            int remainingLifes = gameView.getLabyrinthImage().HealthBar.getCurrent();
            if (gameState == GameState.STOP)
			{
				gameLevel = 1;
				remainingLifes = difficulty.getSlimusLives();
				gameState = GameState.MENU;
			}
            gameView.Restart(this, gameLevel);
            playerMovement = new PlayerMovement(gameView.getSlimusObject().getIterator());
            actionManager = new ActionManager(this, playerMovement);
            physicsThread.Abort();
            commandInvoker = new Invoker();
            physics = new PhysicsThread(gameView.labyrinthImage, audioBabillard, commandInvoker);
            physicsThread = new Thread(new ThreadStart(physics.startThread));
            physicsThread.Start();
            gameView.InitializeHeaderBar(new HealthBarCreator(), difficulty.getSlimusLives(), remainingLifes);
            gameView.InitializeHeaderBar(new BubbleBarCreator(), Constant.INITIAL_BUBBLE_COUNT, Constant.INITIAL_BUBBLE_COUNT);
            gameView.InitializeHeaderBar(new GemBarCreator(), Constant.INITIAL_GEM_COUNT, 0);
            audio.restartMusic();

			if (gameState == GameState.RESTART)
			{
                resume();
            }
			else
			{
                gameView.Pause();
            }
        }
    }
}
