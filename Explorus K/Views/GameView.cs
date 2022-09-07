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
		public GameForm GAME_FORM;
		string GAME_TITLE;

		private const double HEADER_RATIO = 0.125;
		private Point HEADER_POSITION = new Point();
		private Point LABYRINTH_POSITION = new Point();
		private double HEADER_HIGHT = 0;
		internal int LARGE_SPRITE_DIMENSION = 52;
		private int SMALL_SPRITE_DIMENSION = 26;
		private double LABYRINTH_HEIGHT = 48 * 9;
		private double LABYRINTH_WIDTH = 48 * 11;
		private int HEADER_OFFSET = 0;

		private PictureBox GAME_HEADER = new PictureBox();
		private PictureBox GAME_LABYRINTH = new PictureBox();

		private int SCREEN_WIDTH = 600;
		private int SREEN_HEIGHT = 600;

		public HealthBar HEALTH_BAR = new HealthBar();
		public BubbleBar BUBBLE_BAR = new BubbleBar();
		public GemBar GEM_BAR = new GemBar();

		public Player SLIMUS;
		Context KEY_STATE = null;
		CollisionContext COLLISION_STRAT = null;

		private Size oldsize = new Size(1, 1);

		public static List<Image2D> ALL_SPRITE = new List<Image2D>();

		public GameView(GameEngine gameEngine)
		{
			GAME_FORM = new GameForm(gameEngine);
			GAME_FORM.Size = new Size(SCREEN_WIDTH, SREEN_HEIGHT);
			GAME_FORM.MinimumSize = new Size(600, 600);
			HEADER_HIGHT = SREEN_HEIGHT * HEADER_RATIO;
			KEY_STATE = new Context(new NoKeyState());
			COLLISION_STRAT = new CollisionContext();

			GAME_HEADER.Dock = DockStyle.Top;
			GAME_LABYRINTH.Dock = DockStyle.Fill;

			GAME_HEADER.Paint += new PaintEventHandler(this.HeaderRenderer);
			GAME_LABYRINTH.Paint += new PaintEventHandler(this.LabyrinthRenderer);

			GAME_FORM.Controls.Add(GAME_HEADER);
			GAME_FORM.Controls.Add(GAME_LABYRINTH);

			oldsize = GAME_FORM.Size;
			resize();
		}

		public void Show() 
		{
			Application.Run(GAME_FORM); 
		}

		public void Render()
		{
			if (GAME_FORM.Visible)
				GAME_FORM.BeginInvoke((MethodInvoker)delegate {
					GAME_FORM.Refresh();
				});
		}

		public void Close()
		{
			if (GAME_FORM.Visible)
				GAME_FORM.BeginInvoke((MethodInvoker)delegate {
					GAME_FORM.Close();
				});
		}

		public void resize()
		{

			LABYRINTH_POSITION = new Point((GAME_FORM.Size.Width / 2) - ((int)LABYRINTH_WIDTH / 2) - 31,
							   ((GAME_FORM.Size.Height - (int)HEADER_HIGHT) / 2) - ((int)LABYRINTH_HEIGHT / 2) + 5);

			HEADER_OFFSET = (GAME_FORM.Size.Width / 2) - 250;

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

		public void Update(double fps, Iterator mapIterator)
		{
			setWindowTitle("Explorus-K - FPS " + Math.Round(fps, 1).ToString());

			if (IsColliding(SpriteId.GEM))
			{
				IncreaseGemBar(mapIterator);
			}

			if (KEY_STATE.CurrentState() == "WithKeyState")
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
			GAME_FORM.Text = GAME_TITLE;

			g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.SLIMUS_TITLE), HEADER_OFFSET, (float)((HEADER_HIGHT-SMALL_SPRITE_DIMENSION)/2), SMALL_SPRITE_DIMENSION*4, SMALL_SPRITE_DIMENSION);

			g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.HEARTH), (int)((SCREEN_WIDTH/5)*0.95) + HEADER_OFFSET, (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
			
			foreach (Image2D image in HEALTH_BAR.healthBar)
			{
				g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 5) * 1)+(image.X* SMALL_SPRITE_DIMENSION) + HEADER_OFFSET, (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
			}

			g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.BUBBLE_BIG), (int)((SCREEN_WIDTH / 5) * 1.95) + HEADER_OFFSET, (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
			foreach (Image2D image in BUBBLE_BAR.bubbleBar)
			{
				g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 5) * 2) + (image.X * SMALL_SPRITE_DIMENSION) + HEADER_OFFSET, (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
			}

			g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.GEM), (int)((SCREEN_WIDTH / 5) * 2.95) + HEADER_OFFSET, (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
			foreach (Image2D image in GEM_BAR.gemBar)
			{
				g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 5) * 3) + (image.X * SMALL_SPRITE_DIMENSION) + HEADER_OFFSET, (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
			}

			if(KEY_STATE.CurrentState() == "WithKeyState")
			{
				g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.KEY), ((SCREEN_WIDTH / 5) * 4) + HEADER_OFFSET, (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
			}
		}
		private void LabyrinthRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);

			foreach (Image2D sp in ALL_SPRITE.ToArray())
			{
				SpriteId spriteId = sp.getId();
				if (spriteId == SpriteId.MINI_SLIMUS || spriteId == SpriteId.GEM)
				{
					float pos = (LARGE_SPRITE_DIMENSION - SMALL_SPRITE_DIMENSION)/ 2;
					g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + LABYRINTH_POSITION.X + pos), (float)(sp.Y + LABYRINTH_POSITION.Y + pos), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
				}
				else if (spriteId == SpriteId.SLIMUS)
				{
					g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(SLIMUS.getImageType()), SLIMUS.getPosX() + LABYRINTH_POSITION.X, SLIMUS.getPosY() + LABYRINTH_POSITION.Y, LARGE_SPRITE_DIMENSION, LARGE_SPRITE_DIMENSION);
				}
				else if (spriteId == SpriteId.DOOR)
				{
					Bitmap opacityImage = SetOpacity(new Bitmap(SpriteContainer.getInstance().getBitmapByImageType(sp.getType())), 0.4f);
					g.DrawImage(opacityImage, (float)(sp.X + LABYRINTH_POSITION.X), (float)(sp.Y + LABYRINTH_POSITION.Y), LARGE_SPRITE_DIMENSION, LARGE_SPRITE_DIMENSION);
				}
				else
				{
					g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + LABYRINTH_POSITION.X), (float)(sp.Y + LABYRINTH_POSITION.Y), LARGE_SPRITE_DIMENSION, LARGE_SPRITE_DIMENSION);
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
			GAME_TITLE = title;
		}

		public void OnLoad(MapCollection Map)
		{
			for (int i = 0; i < Map.getLengthX(); i++)
			{
				for (int j = 0; j < Map.getLengthY(); j++)
				{
					if (Map.getMap()[i, j] == "w")
						ALL_SPRITE.Add(new Image2D(SpriteId.WALL, ImageType.WALL, i * LARGE_SPRITE_DIMENSION, j * LARGE_SPRITE_DIMENSION));
					else if (Map.getMap()[i, j] == "g")
						ALL_SPRITE.Add(new Image2D(SpriteId.GEM, ImageType.GEM, i * LARGE_SPRITE_DIMENSION, j * LARGE_SPRITE_DIMENSION));
					else if (Map.getMap()[i, j] == "m")
					{
						ALL_SPRITE.Add(new Image2D(SpriteId.MINI_SLIMUS, ImageType.SMALL_SLIMUS, i * LARGE_SPRITE_DIMENSION, j * LARGE_SPRITE_DIMENSION));
					}
					else if(Map.getMap()[i, j] == "s")
					{
						SLIMUS = new Player(i * LARGE_SPRITE_DIMENSION, j * LARGE_SPRITE_DIMENSION);
						ALL_SPRITE.Add(new Image2D(SpriteId.SLIMUS, SLIMUS.getImageType(), SLIMUS.getPosX(), SLIMUS.getPosY()));
					}
					else if (Map.getMap()[i, j] == "s")
					{
						ALL_SPRITE.Add(new Image2D(0, ImageType.SLIMUS_DOWN_ANIMATION_1, i * LARGE_SPRITE_DIMENSION, j * LARGE_SPRITE_DIMENSION));
					}
					else if (Map.getMap()[i, j] == "p")
						ALL_SPRITE.Add(new Image2D(SpriteId.DOOR, ImageType.WALL, i * LARGE_SPRITE_DIMENSION, j * LARGE_SPRITE_DIMENSION));
				}
			}
		}

		public Player getSlimusObject()
		{
			return SLIMUS;
		}

		public void InitializeHeaderBar(ProgressionBarCreator creator, int count)
		{
			IBar bar = creator.InitializeBar(count);

			if(creator.GetType() == typeof(HealthBarCreator))
			{
				this.HEALTH_BAR = (HealthBar)bar;
			}
			else if (creator.GetType() == typeof(BubbleBarCreator))
			{
				this.BUBBLE_BAR = (BubbleBar)bar;
			}
			else if (creator.GetType() == typeof(GemBarCreator))
			{
				this.GEM_BAR = (GemBar)bar;
			}
		}

		public int DecreaseHealthBar()
		{
			HEALTH_BAR.Decrease();
			return HEALTH_BAR.getCurrent();
		}

		public int DecreaseBubbleBar()
		{
			BUBBLE_BAR.Decrease();
			return BUBBLE_BAR.getCurrent();
		}

		public int DecreaseGemBar()
		{
			GEM_BAR.Decrease();
			return GEM_BAR.getCurrent();
		}

		public int IncreaseHealthBar()
		{
			HEALTH_BAR.Increase();
			return HEALTH_BAR.getCurrent();
		}

		public int IncreaseBubbleBar()
		{
			BUBBLE_BAR.Increase();
			return BUBBLE_BAR.getCurrent();
		}

		public int IncreaseGemBar(Iterator mapIterator)
		{
			GEM_BAR.Increase();
			if (GEM_BAR.getCurrent() == GEM_BAR.getLength())
			{
				KEY_STATE.RequestChangingState();
				Point pos = mapIterator.findPosition("p");
				mapIterator.removeAt(pos.X, pos.Y);
			}
			return GEM_BAR.getCurrent();
		}

		public bool IsColliding(SpriteId sprite)
		{
			if(sprite == SpriteId.GEM)
			{
				COLLISION_STRAT.SetStrategy(new GemStrategy());
			}
			else if (sprite == SpriteId.DOOR)
			{
				COLLISION_STRAT.SetStrategy(new DoorStrategy());
			}
			else if (sprite == SpriteId.MINI_SLIMUS)
			{
				COLLISION_STRAT.SetStrategy(new MiniSlimeStrategy());
			}

			float pos = (LARGE_SPRITE_DIMENSION - SMALL_SPRITE_DIMENSION) / 2;

			float slimusX = SLIMUS.getPosX();
			float slimusY = SLIMUS.getPosY() + LABYRINTH_POSITION.Y;

			int pixel = COLLISION_STRAT.executeStrategy();

			//foreach (Image2D sp in ALL_SPRITE.FindAll(im => im.getId() == sprite))
			for (int i = 0; i < ALL_SPRITE.Count; i++)
			{
				Image2D sp = ALL_SPRITE[i];
				if(sp.getId() == sprite)
				{
					float objectX = sp.X + pos;
					float objectY = sp.Y + LABYRINTH_POSITION.Y + pos;

					if (slimusX < objectX + SMALL_SPRITE_DIMENSION - pixel &&
					slimusX + LARGE_SPRITE_DIMENSION - pixel > objectX &&
					slimusY < objectY + SMALL_SPRITE_DIMENSION - pixel &&
					slimusY + LARGE_SPRITE_DIMENSION - pixel > objectY)
					{
						ALL_SPRITE.RemoveAt(i);
						
						return true;
					}
				}
			}

			return false;
		}
	}
}
