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
		private const int MS_PER_FRAME = 16;

		private GameView gameView;
		private List<Binding> bindings;
		private Actions currentAction = Actions.none;
		private bool exit = false;
		private bool isPaused = false;
		private int lifeCount = 3;
		private int bubbleCount = 3;
		private int gemCount = 3;
		private int animationCount = 0;

		//Time space continum related global variables
		private double start_time = 0;
		private int count = 0;
		int next_expected_pos = 0;

		private MapCollection gameMap = new MapCollection(new string[,]{
            { "w", "w", "w", "w", "w", "w", "w", "w", "w"},
            { "w", "." ,".", ".", ".", ".", ".", ".", "w"},
            { "w", ".", "w", ".", "w", "w", "w", ".", "w"},
            { "w", "g", "w", ".", ".", ".", ".", ".", "w"},
            { "w", ".", "w", ".", "w", "w", "w", "s", "w"},
            { "w", ".", ".", ".", "w", "g", "w", "w", "w"},
            { "w", ".", "w", "w", "w", ".", ".", ".", "w"},
            { "w", ".", "w", "m", "p", ".", "w", ".", "w"},
            { "w", ".", "w", "w", "w", ".", "w", "g", "w"},
            { "w", ".", ".", ".", ".", ".", ".", ".", "w"},
            { "w", "w", "w", "w", "w", "w", "w", "w", "w"}
        });

        private Iterator mapIterator = null;

        public GameEngine()

		{
			//The game engine get passed from contructor to constructor until it reach GameForm.cs
			gameView = new GameView(this);
			bindings = initiate_bindings();
            mapIterator = gameMap.CreateIterator("s");
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
			gameView.OnLoad(gameMap);

			double previous_time = getCurrentTime();
			double lag = 0.0;

			while (!exit)
			{
				//Actions state machine
				systemActionsManagement();

				double current_time = getCurrentTime();
				double elapsed_time = current_time - previous_time;
				previous_time = current_time;
				lag += elapsed_time;

				if (!isPaused)
				{
					characterActionsManagement(elapsed_time);

					if (lag >= MS_PER_FRAME)
					{

						float fps = 1000f / (float)lag;

						while (lag >= MS_PER_FRAME)
						{
							gameView.Update(fps, mapIterator);
							lag -= MS_PER_FRAME;
						}

						gameView.Render();
					}

					Thread.Sleep(1);
				}
			}

			Application.Exit();
		}

		internal void resize()
		{
			if (gameView != null)
				gameView.resize();
		}

		//Receving the event from a keypress and checking if we have a action bind to that key
		internal void KeyEventHandler(KeyEventArgs e)
		{
			foreach(Binding binding in bindings)
			{
				if(binding.Key == e.KeyCode)
				{
					actionHandler(binding.Action);
				}
			}
		}
		
		//If we have a action bind to that kay, we check if that action can be done
		private void actionHandler(Actions action)
        {
            if (action == Actions.pause || action == Actions.exit)
				currentAction = action;
			else if (action == Actions.move_left && mapIterator.isAbleToMoveLeft() && mapIterator.GetLeft() != "w" && mapIterator.GetLeft() != "p" && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_right && mapIterator.isAbleToMoveRight() && mapIterator.GetRight() != "w" && mapIterator.GetRight() != "p" && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_up && mapIterator.isAbleToMoveUp() && mapIterator.GetUp() != "w" && mapIterator.GetUp() != "p" && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_down && mapIterator.isAbleToMoveDown() && mapIterator.GetDown() != "w" && mapIterator.GetDown() != "p" && currentAction == Actions.none)
				currentAction = action;
		}

		//If the action can be done, we use a state machine to wait until the action is over
		private void characterActionsManagement(double elapsed_time)
		{
			//Actions state machine
			if (currentAction == Actions.none) { }
			else if (currentAction == Actions.move_left)
			{
				if (count < gameView.largeSpriteDimension)
				{
					count+=2;
					gameView.getSlimusObject().moveLeft(2);
          
					if (count < 8)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_1);
					else if (count > 8 && count < 16)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_2);
					else if (count > 16 && count < 32)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_3);
					else if (count > 32 && count < 40)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_2);
					else if (count > 40)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_1);
				}
				else
				{
					count = 0;
					currentAction = Actions.none;
                    mapIterator.MoveLeft();
                }
			}
			else if (currentAction == Actions.move_right)
			{
				if (count < gameView.largeSpriteDimension)
				{
					count += 2;
					gameView.getSlimusObject().moveRight(2);

					if (count < 8)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_1);
					else if (count > 8 && count < 16)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_2);
					else if (count > 16 && count < 32)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_3);
					else if (count > 32 && count < 40)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_2);
					else if (count > 40)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_1);
				}
				else
				{
					count = 0;
					currentAction = Actions.none;
					mapIterator.MoveRight();
				}

			}
			else if (currentAction == Actions.move_up) 
			{
				if (count < gameView.largeSpriteDimension)
				{
					count += 2;
					gameView.getSlimusObject().moveUp(2);

					if (count < 8)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_1);
					else if (count > 8 && count < 16)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_2);
					else if (count > 16 && count < 32)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_3);
					else if (count > 32 && count < 40)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_2);
					else if (count > 40)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_1);
				}
				else
				{
					count = 0;
					currentAction = Actions.none;
                    mapIterator.MoveUp();
                }
			}
			else if (currentAction == Actions.move_down) 
			{
				if (count < gameView.largeSpriteDimension)
				{
					count += 2;
					gameView.getSlimusObject().moveDown(2);

					if (count < 8)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_1);
					else if (count > 8 && count < 16)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_2);
					else if (count > 16 && count < 32)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_3);
					else if (count > 32 && count < 40)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_2);
					else if (count > 40)
						gameView.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_1);
				}
				else
				{
					count = 0;
					currentAction = Actions.none;
                    mapIterator.MoveDown();
                }
			}
		}

		private void systemActionsManagement()
		{
			//Actions state machine
			if (currentAction == Actions.none) { }
			else if (currentAction == Actions.pause)
			{
				isPaused = !isPaused;
				currentAction = Actions.none;
			}
			else if (currentAction == Actions.exit)
			{
				exit = true;
			}
		}

		//Return the time in ms
		private long getCurrentTime()
		{
			return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}
	}
}
