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

		public Slimus slimus;
		Context keyState = null;
		CollisionContext collisionStart = null;

		private Size oldsize = new Size(1, 1);

		public static List<Image2D> allSprite = new List<Image2D>();

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

			labyrinthPosition = new Point((gameForm.Size.Width / 2) - ((int)labyrinthWidth / 2) - 31,
							   ((gameForm.Size.Height - (int)headerHeight) / 2) - ((int)labyrinthHeight / 2) + 5);

			headerOffset = (gameForm.Size.Width / 2) - 250;

			/*

			double ratio;

			if (GAME_FORM.Size.Width <= GAME_FORM.Size.Height)
				ratio = (double)GAME_FORM.Size.Width / (double)this.oldsize.Width;
			else
				ratio = (double)GAME_FORM.Size.Height / (double)this.oldsize.Height;

			
			LARGE_SPRITE_DIMENSION = (int) (ratio * (float) LARGE_SPRITE_DIMENSION);
			SMALL_SPRITE_DIMENSION = (int) (ratio * (float) SMALL_SPRITE_DIMENSION);

			foreach (Image2D sp in ALL_SPRITE.ToArray())
			{
				sp.X = (int) (ratio * sp.X);
				sp.Y = (int) (ratio * sp.Y);
			}
			
			if (LARGE_SPRITE_DIMENSION > 100) LARGE_SPRITE_DIMENSION = 100;
			if (SMALL_SPRITE_DIMENSION > 80) SMALL_SPRITE_DIMENSION = 80;

			oldsize = GAME_FORM.Size;

			*/
		}

		public void Update(double elapsed, Iterator mapIterator)
		{
			double FPS = Math.Round(1000 / elapsed, 1);
			setWindowTitle("Explorus-K - FPS " + FPS.ToString());

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

			g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.SLIMUS_TITLE), headerOffset, (float)((headerHeight-smallSpriteDimension)/2), smallSpriteDimension*4, smallSpriteDimension);

			g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.HEARTH), (int)((screenWidth/5)*0.95) + headerOffset, (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
			
			foreach (Image2D image in healthBar.healthBar)
			{
				g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 5) * 1)+(image.X* smallSpriteDimension) + headerOffset, (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
			}

			g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.BUBBLE_BIG), (int)((screenWidth / 5) * 1.95) + headerOffset, (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
			foreach (Image2D image in bubbleBar.bubbleBar)
			{
				g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 5) * 2) + (image.X * smallSpriteDimension) + headerOffset, (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
			}

			g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.GEM), (int)((screenWidth / 5) * 2.95) + headerOffset, (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
			foreach (Image2D image in gemBar.gemBar)
			{
				g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 5) * 3) + (image.X * smallSpriteDimension) + headerOffset, (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
			}

			if(keyState.CurrentState() == "WithKeyState")
			{
				g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.KEY), ((screenWidth / 5) * 4) + headerOffset, (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
			}
		}
		private void LabyrinthRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);

			foreach (Image2D sp in allSprite.ToArray())
			{
				SpriteId spriteId = sp.getId();
				if (spriteId == SpriteId.MINI_SLIMUS || spriteId == SpriteId.GEM)
				{
					float pos = (largeSpriteDimension - smallSpriteDimension)/ 2;
					g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + labyrinthPosition.X + pos), (float)(sp.Y + labyrinthPosition.Y + pos), smallSpriteDimension, smallSpriteDimension);
				}
				else if (spriteId == SpriteId.SLIMUS)
				{
					g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(slimus.getImageType()), slimus.getPosX() + labyrinthPosition.X, slimus.getPosY() + labyrinthPosition.Y, largeSpriteDimension, largeSpriteDimension);
				}
				else if (spriteId == SpriteId.DOOR)
				{
					Bitmap opacityImage = SetOpacity(new Bitmap(SpriteContainer.getInstance().getBitmapByImageType(sp.getType())), 0.4f);
					g.DrawImage(opacityImage, (float)(sp.X + labyrinthPosition.X), (float)(sp.Y + labyrinthPosition.Y), largeSpriteDimension, largeSpriteDimension);
				}
				else
				{
					g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + labyrinthPosition.X), (float)(sp.Y + labyrinthPosition.Y), largeSpriteDimension, largeSpriteDimension);
				}
			}
		}

		private Bitmap SetOpacity(Bitmap image, float opacity)
		{
			Bitmap output = new Bitmap(image.Width, image.Height);

			using (Graphics gr = Graphics.FromImage(output))
			{
				ColorMatrix colorMatrix = new ColorMatrix();
				colorMatrix.Matrix33 = opacity;
				
				ImageAttributes attributes = new ImageAttributes();
				attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				Rectangle rect = new Rectangle(0,0, output.Width, output.Height);

				gr.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
			}
			return output;
		}

		public void setWindowTitle(string title)
		{
			gameTitle = title;
		}

		public void OnLoad(MapCollection Map)
		{
			for (int i = 0; i < Map.getLengthX(); i++)
			{
				for (int j = 0; j < Map.getLengthY(); j++)
				{
					if (Map.getMap()[i, j] == "w")
						allSprite.Add(new Image2D(SpriteId.WALL, ImageType.WALL, i * largeSpriteDimension, j * largeSpriteDimension));
					else if (Map.getMap()[i, j] == "g")
						allSprite.Add(new Image2D(SpriteId.GEM, ImageType.GEM, i * largeSpriteDimension, j * largeSpriteDimension));
					else if (Map.getMap()[i, j] == "m")
					{
						allSprite.Add(new Image2D(SpriteId.MINI_SLIMUS, ImageType.SMALL_SLIMUS, i * largeSpriteDimension, j * largeSpriteDimension));
					}
					else if(Map.getMap()[i, j] == "s")
					{
						slimus = new Slimus(i * largeSpriteDimension, j * largeSpriteDimension);
						allSprite.Add(new Image2D(SpriteId.SLIMUS, slimus.getImageType(), slimus.getPosX(), slimus.getPosY()));
					}
					else if (Map.getMap()[i, j] == "s")
					{
						allSprite.Add(new Image2D(0, ImageType.SLIMUS_DOWN_ANIMATION_1, i * largeSpriteDimension, j * largeSpriteDimension));
					}
					else if (Map.getMap()[i, j] == "p")
						allSprite.Add(new Image2D(SpriteId.DOOR, ImageType.WALL, i * largeSpriteDimension, j * largeSpriteDimension));
				}
			}
		}

		public Slimus getSlimusObject()
		{
			return slimus;
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
				mapIterator.removeAt(pos.X, pos.Y);
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

			float pos = (largeSpriteDimension - smallSpriteDimension) / 2;

			float slimusX = slimus.getPosX();
			float slimusY = slimus.getPosY() + labyrinthPosition.Y;

			int pixel = collisionStart.executeStrategy();

			//foreach (Image2D sp in ALL_SPRITE.FindAll(im => im.getId() == sprite))
			for (int i = 0; i < allSprite.Count; i++)
			{
				Image2D sp = allSprite[i];
				if(sp.getId() == sprite)
				{
					float objectX = sp.X + pos;
					float objectY = sp.Y + labyrinthPosition.Y + pos;

					if (slimusX < objectX + smallSpriteDimension - pixel &&
					slimusX + largeSpriteDimension - pixel > objectX &&
					slimusY < objectY + smallSpriteDimension - pixel &&
					slimusY + largeSpriteDimension - pixel > objectY)
					{
						allSprite.RemoveAt(i);
						
						return true;
					}
				}
			}

			return false;
		}
	}
}
