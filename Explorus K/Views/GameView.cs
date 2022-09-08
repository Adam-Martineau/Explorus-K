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

namespace Explorus_K.Views
{
	class GameView
	{
		public GameForm gameForm;
		string gameTitle;

		private const double headerRatio = 0.125;
		private Point headerPosition = new Point();
		private Point labyrinthPosition = new Point();
		private double headerHeight = 0;
		internal int largeSpriteDimension = 52;
		private int smallSpriteDimension = 26;
		private double labyrinthHeight = 48 * 9;
		private double labyrinthWidth = 48 * 11;
		private int headerOffset = 0;

		private PictureBox gameHeader = new PictureBox();
		private PictureBox gameLabyrinth = new PictureBox();

		private int screenWidth = 600;
		private int screenHeight = 600;

		public HealthBar healthBar = new HealthBar();
		public BubbleBar bubbleBar = new BubbleBar();
		public GemBar gemBar = new GemBar();

		public LabyrinthImage labyrinthImage;
		Context keyState = null;
		CollisionContext collisionStart = null;

		private Size oldsize = new Size(1, 1);

		public GameView(GameEngine gameEngine)
		{
			gameForm = new GameForm(gameEngine);
			gameForm.Size = new Size(screenWidth, screenHeight);
			gameForm.MinimumSize = new Size(600, 600);
			headerHeight = screenHeight * headerRatio;
			keyState = new Context(new NoKeyState());
			collisionStart = new CollisionContext();

			gameHeader.Dock = DockStyle.Top;
			gameLabyrinth.Dock = DockStyle.Fill;

			gameHeader.Paint += new PaintEventHandler(this.HeaderRenderer);
			gameLabyrinth.Paint += new PaintEventHandler(this.LabyrinthRenderer);

			gameForm.Controls.Add(gameHeader);
			gameForm.Controls.Add(gameLabyrinth);

			labyrinthImage = new LabyrinthImage(new Labyrinth());

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
			labyrinthImage.resize(gameForm);
		}

		public void Update(double fps, Iterator mapIterator)
		{
			setWindowTitle("Explorus-K - FPS " + Math.Round(fps, 1).ToString());

			if (IsColliding(SpriteId.GEM))
			{
				IncreaseGemBar(mapIterator);
			}

			if (keyState.CurrentState() == "WithKeyState")
			{
				IsColliding(SpriteId.DOOR);
			}

			if (IsColliding(SpriteId.MINI_SLIMUS))
			{
				Close();
			}
		}

		private void HeaderRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);
			gameForm.Text = gameTitle;

			labyrinthImage.drawHeader(g, healthBar, gemBar, bubbleBar, keyState);
		}
		private void LabyrinthRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);

			labyrinthImage.drawLabyrinthImage(g);
		}

		

		public void setWindowTitle(string title)
		{
			gameTitle = title;
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
				this.healthBar = (HealthBar)bar;
			}
			else if (creator.GetType() == typeof(BubbleBarCreator))
			{
				this.bubbleBar = (BubbleBar)bar;
			}
			else if (creator.GetType() == typeof(GemBarCreator))
			{
				this.gemBar = (GemBar)bar;
			}
		}

		public int DecreaseHealthBar()
		{
			healthBar.Decrease();
			return healthBar.getCurrent();
		}

		public int DecreaseBubbleBar()
		{
			bubbleBar.Decrease();
			return bubbleBar.getCurrent();
		}

		public int DecreaseGemBar()
		{
			gemBar.Decrease();
			return gemBar.getCurrent();
		}

		public int IncreaseHealthBar()
		{
			healthBar.Increase();
			return healthBar.getCurrent();
		}

		public int IncreaseBubbleBar()
		{
			bubbleBar.Increase();
			return bubbleBar.getCurrent();
		}

		public int IncreaseGemBar(Iterator mapIterator)
		{
			gemBar.Increase();
			if (gemBar.getCurrent() == gemBar.getLength())
			{
				keyState.RequestChangingState();
				Point pos = mapIterator.findPosition("p");
				mapIterator.replaceAt(".", pos.X, pos.Y);
			}
			return gemBar.getCurrent();
		}

		public bool IsColliding(SpriteId sprite)
		{
			if(sprite == SpriteId.GEM)
			{
				collisionStart.SetStrategy(new GemStrategy());
			}
			else if (sprite == SpriteId.DOOR)
			{
				collisionStart.SetStrategy(new DoorStrategy());
			}
			else if (sprite == SpriteId.MINI_SLIMUS)
			{
				collisionStart.SetStrategy(new MiniSlimeStrategy());
			}

			int pixel = collisionStart.executeStrategy();

			return labyrinthImage.IsColliding(pixel, sprite);
		}
	}
}
