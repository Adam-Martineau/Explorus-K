﻿using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Models;
using Explorus_K.NewFolder1;
using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using Size = System.Drawing.Size;

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
		private PictureBox gamePause = new PictureBox();
        private PictureBox gameOver = new PictureBox();

        private int screenWidth = 1000;
		private int screenHeight = 1000;		

		public LabyrinthImage labyrinthImage;		

		public GameView(GameEngine gameEngine)
		{
			this.gameEngine = gameEngine;
			gameForm = new GameForm(this);
			gameForm.Size = new Size(screenWidth, screenHeight);
			gameForm.MinimumSize = new Size(800, 1000);

			gameHeader.Dock = DockStyle.Top;
			gameLabyrinth.Dock = DockStyle.Fill;
			gamePause.Dock = DockStyle.Bottom;

			gameHeader.Paint += new PaintEventHandler(this.HeaderRenderer);
			gameLabyrinth.Paint += new PaintEventHandler(this.LabyrinthRenderer);
			gamePause.Paint += new PaintEventHandler(this.PauseRenderer);
            //gameOver.Paint += new PaintEventHandler(this.GameOverRenderer);

            gameForm.Controls.Add(gameHeader);
			gameForm.Controls.Add(gameLabyrinth);
			gameForm.Controls.Add(gamePause);

			labyrinthImage = new LabyrinthImage(gameEngine.GetLabyrinth(), gameEngine.getBubbleManager());

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

		public void Update(double fps)
		{
            gameTitle = "Explorus-K - FPS " + Math.Round(fps, 1).ToString();

            labyrinthImage.IsColliding(SpriteId.SLIMUS, SpriteId.GEM);

			labyrinthImage.IsColliding(SpriteId.SLIMUS, SpriteId.DOOR);

            labyrinthImage.IsColliding(SpriteId.SLIMUS, SpriteId.MINI_SLIMUS);

            labyrinthImage.IsColliding(SpriteId.SLIMUS, SpriteId.TOXIC_SLIME);
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

			if (gameEngine.Paused)
				g.DrawString("PAUSE", new Font("Arial", 80), Brushes.White, screenWidth/2, screenHeight/2);
		}

        private void GameOverRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            //if (gameEngine.Paused)
            //    g.DrawString("PAUSE", new Font("Arial", 80), Brushes.White, screenWidth / 2, screenHeight / 2);
        }

        public Player getSlimusObject()
		{
			return labyrinthImage.getSlimus();
		}
		public BubbleBar getBubbleBarObject()
        {
            return labyrinthImage.BubbleBar;
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

		internal void LostFocus()
		{
			gameEngine.pause();
		}

		internal void GainFocus()
		{
			gameEngine.unpause();
			//ToDo: ajouté un délais de 3 sec
		}

        public void UpdateStatusBar(String msg, Color color)
        {
            gameForm.UpdateStatusBar(msg, color);
        }

		public LabyrinthImage getLabyrinthImage()
		{
			return labyrinthImage;
		}
    }
}
