using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Models;
using Explorus_K.NewFolder1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Forms.Application;
using Size = System.Drawing.Size;
using System.Drawing.Imaging;
using System.Security.Policy;
using System.Windows;
using Point = System.Drawing.Point;

namespace Explorus_K.Views
{
	public class GameView
	{
		public GameForm gameForm;
        GameEngine gameEngine;
        string gameTitle;

		private const double headerRatio = 0.125;
		private double headerHeight = 0;
		internal int largeSpriteDimension = 52;

		private PictureBox gameHeader = new PictureBox();
		private PictureBox gameLabyrinth = new PictureBox();
		private PictureBox gamePause = new PictureBox();

		private int screenWidth = 600;
		private int screenHeight = 600;		

		public LabyrinthImage labyrinthImage;		

		private Size oldsize = new Size(1, 1);

		public GameView(GameEngine gameEngine)
		{
			this.gameEngine = gameEngine;
			gameForm = new GameForm(this);
			gameForm.Size = new Size(screenWidth, screenHeight);
			gameForm.MinimumSize = new Size(600, 600);

			gameHeader.Dock = DockStyle.Top;
			gameLabyrinth.Dock = DockStyle.Fill;
			gamePause.Dock = DockStyle.Fill;

			gameHeader.Paint += new PaintEventHandler(this.HeaderRenderer);
			gameLabyrinth.Paint += new PaintEventHandler(this.LabyrinthRenderer);
			gamePause.Paint += new PaintEventHandler(this.PauseRenderer);


			gameForm.Controls.Add(gameHeader);
			gameForm.Controls.Add(gameLabyrinth);
			gameForm.Controls.Add(gamePause);

			labyrinthImage = new LabyrinthImage(gameEngine.GetLabyrinth());

			oldsize = gameForm.Size;
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

		public void Close()
		{
			if (gameForm.Visible)
				gameForm.BeginInvoke((MethodInvoker)delegate {
					gameForm.Close();
				});
		}

		public void resize()
		{
			if (labyrinthImage != null)
			{
                labyrinthImage.resize(gameForm);
            }
		}

		public void Update(double fps, Iterator mapIterator)
		{
            gameTitle = "Explorus-K - FPS " + Math.Round(fps, 1).ToString();

            labyrinthImage.IsColliding(SpriteId.GEM);

			labyrinthImage.IsColliding(SpriteId.DOOR);

            labyrinthImage.IsColliding(SpriteId.MINI_SLIMUS);
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
		}

		private void PauseRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);

			//if (gameEngine.Paused)
			g.DrawString("PAUSE", new Font("Arial", 80), Brushes.White, 0, 0);
		}

		public Player getSlimusObject()
		{
			return labyrinthImage.getSlimus();
		}

		public void InitializeHeaderBar(ProgressionBarCreator creator, int count)
		{
			IBar bar = creator.InitializeBar(count);

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
    }
}
