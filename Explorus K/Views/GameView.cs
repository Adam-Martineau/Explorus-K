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
        private int LARGE_SPRITE_DIMENSION = 52;
        private int SMALL_SPRITE_DIMENSION = 26;
        private double LABYRINTH_HEIGHT = 48 * 9;
        private double LABYRINTH_WIDTH = 48 * 11;

        private PictureBox GAME_HEADER = new PictureBox();
        private PictureBox GAME_LABYRINTH = new PictureBox();

        private int SCREEN_WIDTH = 600;
        private int SREEN_HEIGHT = 600;

        public HealthBar HEALTH_BAR = new HealthBar();
        public BubbleBar BUBBLE_BAR = new BubbleBar();
        public GemBar GEM_BAR = new GemBar();

        public Slimus SLIMUS;
        Context KEY_STATE = null;

        public static List<Image2D> ALL_SPRITE = new List<Image2D>();

        public GameView(GameEngine gameEngine)
        {
            GAME_FORM = new GameForm(gameEngine);
            GAME_FORM.Size = new Size(SCREEN_WIDTH, SREEN_HEIGHT);
            GAME_FORM.MinimumSize = new Size(600, 600);
            HEADER_HIGHT = SREEN_HEIGHT * HEADER_RATIO;
            HEADER_POSITION = new Point(0, 0);
            LABYRINTH_POSITION = new Point((int)((SCREEN_WIDTH-LABYRINTH_WIDTH)/2), (int)HEADER_HIGHT);
            KEY_STATE = new Context(new NoKeyState());

            GAME_HEADER.Dock = DockStyle.Top;
            GAME_LABYRINTH.Dock = DockStyle.Fill;

            GAME_HEADER.Paint += new PaintEventHandler(this.HeaderRenderer);
            GAME_LABYRINTH.Paint += new PaintEventHandler(this.LabyrinthRenderer);

            GAME_FORM.Controls.Add(GAME_HEADER);
            GAME_FORM.Controls.Add(GAME_LABYRINTH);
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

        public void Update(double elapsed)
        {
            double FPS = Math.Round(1000 / elapsed, 1);
            setWindowTitle("Explorus-K - FPS " + FPS.ToString());

            if (IsColliding(SpriteId.GEM))
            {
                IncreaseGemBar();
            }

            if (KEY_STATE.CurrentState() == "WithKeyState")
            {
                IsColliding(SpriteId.DOOR);
            }
        }

        private void HeaderRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            GAME_FORM.Text = GAME_TITLE;

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.SLIMUS_TITLE), (SCREEN_WIDTH / 5) * 0, (float)((HEADER_HIGHT-SMALL_SPRITE_DIMENSION)/2), SMALL_SPRITE_DIMENSION*4, SMALL_SPRITE_DIMENSION);

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.HEARTH), (int)((SCREEN_WIDTH/5)*0.95), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            
            foreach (Image2D image in HEALTH_BAR.healthBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 5) * 1)+(image.X* SMALL_SPRITE_DIMENSION), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            }

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.BUBBLE_BIG), (int)((SCREEN_WIDTH / 5) * 1.95), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            foreach (Image2D image in BUBBLE_BAR.bubbleBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 5) * 2) + (image.X * SMALL_SPRITE_DIMENSION), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            }

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.GEM), (int)((SCREEN_WIDTH / 5) * 2.95), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            foreach (Image2D image in GEM_BAR.gemBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 5) * 3) + (image.X * SMALL_SPRITE_DIMENSION), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            }

            if(KEY_STATE.CurrentState() == "WithKeyState")
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.KEY), ((SCREEN_WIDTH / 5) * 4), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            }
        }
        private void LabyrinthRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            

            foreach (Image2D sp in ALL_SPRITE)
            {
                SpriteId spriteId = sp.getId();
                if (spriteId == SpriteId.MINI_SLIMUS || spriteId == SpriteId.GEM)
                {
                    float pos = (LARGE_SPRITE_DIMENSION - SMALL_SPRITE_DIMENSION)/ 2;
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + pos), (float)(sp.Y + LABYRINTH_POSITION.Y + pos), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
                }
                else if (spriteId == SpriteId.SLIMUS)
                {
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(SLIMUS.getImageType()), SLIMUS.getPosX(), SLIMUS.getPosY() + LABYRINTH_POSITION.Y, LARGE_SPRITE_DIMENSION, LARGE_SPRITE_DIMENSION);
                }
                else if (spriteId == SpriteId.DOOR)
                {
                    Graphics graphics = Graphics.FromImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()));
                    ColorMatrix colormatrix = new ColorMatrix();
                    colormatrix.Matrix33 = 50;
                    ImageAttributes imgAttribute = new ImageAttributes();
                    imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), new Rectangle(new Point(sp.X, sp.Y + LABYRINTH_POSITION.Y)), (float)(sp.X), (float)(sp.Y + LABYRINTH_POSITION.Y), LARGE_SPRITE_DIMENSION, LARGE_SPRITE_DIMENSION, GraphicsUnit.Pixel, imgAttribute);
                }
                else
                {
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X), (float)(sp.Y + LABYRINTH_POSITION.Y), LARGE_SPRITE_DIMENSION, LARGE_SPRITE_DIMENSION);
                }
            }
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
                        SLIMUS = new Slimus(i * LARGE_SPRITE_DIMENSION, j * LARGE_SPRITE_DIMENSION);
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

        public Slimus getSlimusObject()
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

        public int IncreaseGemBar()
        {
            GEM_BAR.Increase();
            Console.WriteLine(GEM_BAR.getCurrent());
            Console.WriteLine(GEM_BAR.getLength());
            if (GEM_BAR.getCurrent() == GEM_BAR.getLength())
            {
                KEY_STATE.RequestChangingState();
            }
            return GEM_BAR.getCurrent();
        }

        public bool IsColliding(SpriteId sprite)
        {
            float pos = (LARGE_SPRITE_DIMENSION - SMALL_SPRITE_DIMENSION) / 2;

            float slimusX = SLIMUS.getPosX();
            float slimusY = SLIMUS.getPosY() + LABYRINTH_POSITION.Y;

            //foreach (Image2D sp in ALL_SPRITE.FindAll(im => im.getId() == sprite))
            for (int i = 0; i < ALL_SPRITE.Count; i++)
            {
                Image2D sp = ALL_SPRITE[i];
                if(sp.getId() == sprite)
                {
                    float objectX = sp.X + pos;
                    float objectY = sp.Y + LABYRINTH_POSITION.Y + pos;

                    if (slimusX < objectX + SMALL_SPRITE_DIMENSION &&
                    slimusX + LARGE_SPRITE_DIMENSION > objectX &&
                    slimusY < objectY + SMALL_SPRITE_DIMENSION &&
                    slimusY + LARGE_SPRITE_DIMENSION > objectY)
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
