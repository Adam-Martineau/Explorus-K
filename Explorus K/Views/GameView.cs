using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Models;
using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Forms.Application;
using Size = System.Drawing.Size;
using Timer = System.Timers.Timer;

namespace Explorus_K.Views
{
	public class GameView
	{
		public GameForm gameForm;
        GameEngine gameEngine;
        string gameTitle;

		internal int largeSpriteDimension = 52;

		private PictureBox gameHeader = new PictureBox();
		private PictureBox gameLabyrinth = new PictureBox();

        private int screenWidth = 1000;
		private int screenHeight = 1000;
		private int menuHeight = 250;

        private static Timer resumeTimer;
        private int countdown = 3;


        public LabyrinthImage labyrinthImage;		

		public GameView(GameEngine gameEngine, int level)
		{
			this.gameEngine = gameEngine;
			gameForm = new GameForm(this);
			gameForm.Size = new Size(screenWidth, screenHeight);
			gameForm.MinimumSize = new Size(800, 1000);

			gameHeader.Dock = DockStyle.Top;
			gameLabyrinth.Dock = DockStyle.Fill;

			gameHeader.Paint += new PaintEventHandler(this.HeaderRenderer);
			gameLabyrinth.Paint += new PaintEventHandler(this.LabyrinthRenderer);

            gameForm.Controls.Add(gameHeader);
			gameForm.Controls.Add(gameLabyrinth);

			labyrinthImage = new LabyrinthImage(gameEngine.GetLabyrinth(), gameEngine.getBubbleManager());

            resumeTimer = new Timer(1000);
            resumeTimer.Elapsed += OnTimedEventResume;

            gameForm.UpdateLevel("Level " + level.ToString(), Color.Red);

            resize();
		}

		public void Show() 
		{
			Application.Run(gameForm); 
		}

        public void Render()
		{
			if (gameForm.Visible)
				gameForm.BeginInvoke((MethodInvoker)delegate {
					gameForm.Refresh();
				});
		}

        public void Restart(GameEngine gameEngine, int level)
		{
			gameForm.UpdateLevel("Level " + level.ToString(), Color.Red);
            this.gameEngine = gameEngine;
            labyrinthImage = new LabyrinthImage(gameEngine.GetLabyrinth(), gameEngine.getBubbleManager());
            resize();
            Render();
        }

        public void Close()
		{
			if (gameForm.Visible)
				gameForm.BeginInvoke((MethodInvoker)delegate {
					gameForm.Close();
				});
		}

		public void resize()
		{
			screenWidth = gameForm.Width;
            screenHeight = gameForm.Height;
            if (labyrinthImage != null)
			{
                labyrinthImage.resize(gameForm);
            }
		}

		public void Update(double fps)
		{
			GameState state = GameState.PLAY;

            gameTitle = "Explorus-K - FPS " + Math.Round(fps, 1).ToString();

			/*if (gameEngine.State == GameState.PLAY)
			{
				labyrinthImage.IsColliding(SpriteType.SLIMUS, SpriteType.GEM);

				labyrinthImage.IsColliding(SpriteType.SLIMUS, SpriteType.DOOR);

				state = labyrinthImage.IsColliding(SpriteType.SLIMUS, SpriteType.MINI_SLIMUS);
				if (state == GameState.RESTART)
				{
					gameEngine.State = state;
				}

				state = labyrinthImage.IsColliding(SpriteType.SLIMUS, SpriteType.TOXIC_SLIME);
				if (state == GameState.STOP)
				{
					gameEngine.State = state;
				}
			}*/
        }

		private void showMenu(Graphics g ,string text)
		{
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            Brush brush = new SolidBrush(Color.FromArgb(64, 255, 255, 255));

            Rectangle menu = new Rectangle(0, (screenHeight / 2) - (menuHeight / 2), screenWidth, menuHeight);
            g.DrawString(text, new Font("Arial", 80), Brushes.Red, menu, stringFormat);
            g.FillRectangle(brush, Rectangle.Round(menu));
        }

		private void HeaderRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);
			gameForm.Text = gameTitle;

			labyrinthImage.drawHeader(g);
		}

		private void LabyrinthRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);

			labyrinthImage.drawLabyrinthImage(g);

            if (gameEngine.State == GameState.PAUSE)
			{
				showMenu(g, "PAUSE");
            }
			else if (gameEngine.State == GameState.RESUME)
			{
				showMenu(g, countdown.ToString());
			}
			else if (gameEngine.State == GameState.STOP)
			{
                showMenu(g, "YOU DIED");
            }
            else if (gameEngine.State == GameState.RESTART)
            {
                showMenu(g, "YOU WIN");
            }
            gameForm.UpdateStatus(gameEngine.State.ToString(), Color.Red);
        }

        public Player getSlimusObject()
		{
			return labyrinthImage.getSlimus();
		}
		public BubbleBar getBubbleBarObject()
        {
            return labyrinthImage.BubbleBar;
        }

        public void InitializeHeaderBar(ProgressionBarCreator creator, int length, int current)
		{
			IBar bar = creator.InitializeBar(length, current);

			if(creator.GetType() == typeof(HealthBarCreator))
			{
				this.labyrinthImage.HealthBar = (HealthBar)bar;
			}
			else if (creator.GetType() == typeof(BubbleBarCreator))
			{
                this.labyrinthImage.BubbleBar = (BubbleBar)bar;
			}
			else if (creator.GetType() == typeof(GemBarCreator))
			{
                this.labyrinthImage.GemBar = (GemBar)bar;
			}
		}

        internal void ReceiveKeyEvent(KeyEventArgs e)
        {
			gameEngine.KeyEventHandler(e);
        }

		internal void LostFocus()
		{
			gameEngine.pause();
		}

		internal void GainFocus()
		{
			gameEngine.resume();
		}

		public void Resume()
		{
            resumeTimer.Start();
            countdown = 3;
        }

        private void OnTimedEventResume(Object source, ElapsedEventArgs e)
        {
            countdown -= 1;
            if (countdown == 0)
            {
                resumeTimer.Stop();
                gameEngine.play();
            }
        }

		public LabyrinthImage getLabyrinthImage()
		{
			return labyrinthImage;
		}
    }
}
