using Explorus_K.Controllers;
using Explorus_K.Models;
using Explorus_K.NewFolder1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Size = System.Drawing.Size;

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

        private Slimus SLIMUS;

        public static List<Image2D> ALL_SPRITE = new List<Image2D>();

        readonly string[,] Map =
        {
            {"w","w","w","w","w","w","w","w","w","w","w"},
            {"w",".",".","g",".",".",".",".",".",".","w"},
            {"w",".","w","w","w",".","w","w","w",".","w"},
            {"w",".",".",".",".",".","w","m","w",".","w"},
            {"w",".","w",".","w","w","w","w","w",".","w"},
            {"w",".","w",".","w","g",".",".",".",".","w"},
            {"w",".","w",".","w","w",".","w","w",".","w"},
            {"w",".",".",".","s","w",".",".","g",".","w"},
            {"w","w","w","w","w","w","w","w","w","w","w"}
        };

        int i = 0;

        public GameView(GameEngine gameEngine)
        {
            GAME_FORM = new GameForm(gameEngine);
            GAME_FORM.Size = new Size(SCREEN_WIDTH, SREEN_HEIGHT);
            GAME_FORM.MinimumSize = new Size(600, 600);
            HEADER_HIGHT = SREEN_HEIGHT * HEADER_RATIO;
            HEADER_POSITION = new Point(0, 0);
            LABYRINTH_POSITION = new Point((int)((SCREEN_WIDTH-LABYRINTH_WIDTH)/2), (int)HEADER_HIGHT);

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
            i += (int) elapsed;
            double FPS = Math.Round(1000 / elapsed, 1);
            setWindowTitle("Explorus-K - FPS " + FPS.ToString());
        }

        private void HeaderRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            GAME_FORM.Text = GAME_TITLE;

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.SLIMUS_TITLE), (SCREEN_WIDTH / 4) * 0, (float)((HEADER_HIGHT-SMALL_SPRITE_DIMENSION)/2), SMALL_SPRITE_DIMENSION*4, SMALL_SPRITE_DIMENSION);

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.HEARTH), (int)((SCREEN_WIDTH/4)*0.95), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            
            foreach (Image2D image in HEALTH_BAR.healthBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 4) * 1)+(image.X* SMALL_SPRITE_DIMENSION), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            }

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.BUBBLE_BIG), (int)((SCREEN_WIDTH / 4) * 1.95), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            foreach (Image2D image in BUBBLE_BAR.bubbleBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 4) * 2) + (image.X * SMALL_SPRITE_DIMENSION), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            }

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.GEM), (int)((SCREEN_WIDTH / 4) * 2.95), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
            foreach (Image2D image in GEM_BAR.gemBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((SCREEN_WIDTH / 4) * 3) + (image.X * SMALL_SPRITE_DIMENSION), (float)((HEADER_HIGHT - SMALL_SPRITE_DIMENSION) / 2), SMALL_SPRITE_DIMENSION, SMALL_SPRITE_DIMENSION);
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
                else if(spriteId == SpriteId.SLIMUS)
                {
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(SLIMUS.getImageType()), SLIMUS.getPosX(), SLIMUS.getPosY() + LABYRINTH_POSITION.Y, LARGE_SPRITE_DIMENSION, LARGE_SPRITE_DIMENSION);
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

        public void OnLoad()
        {
            for (int i = 0; i < Map.GetLength(0); i++)

            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == "w")
                        ALL_SPRITE.Add(new Image2D(SpriteId.WALL, ImageType.WALL, j * LARGE_SPRITE_DIMENSION, i * LARGE_SPRITE_DIMENSION));
                    else if (Map[i, j] == "g")
                        ALL_SPRITE.Add(new Image2D(SpriteId.GEM, ImageType.GEM, j * LARGE_SPRITE_DIMENSION, i * LARGE_SPRITE_DIMENSION));
                    else if (Map[i, j] == "m")
                    {
                        ALL_SPRITE.Add(new Image2D(SpriteId.MINI_SLIMUS, ImageType.SMALL_SLIMUS, j * LARGE_SPRITE_DIMENSION, i * LARGE_SPRITE_DIMENSION));
                    }
                    else if(Map[i, j] == "s")
                    {
                        SLIMUS = new Slimus(j * LARGE_SPRITE_DIMENSION, i * LARGE_SPRITE_DIMENSION);
                        ALL_SPRITE.Add(new Image2D(SpriteId.SLIMUS, SLIMUS.getImageType(), SLIMUS.getPosX(), SLIMUS.getPosY()));
                    }
                    else if (Map[i, j] == "s")
                    {
                        ALL_SPRITE.Add(new Image2D(0, ImageType.SLIMUS_DOWN_ANIMATION_1, j * LARGE_SPRITE_DIMENSION, i * LARGE_SPRITE_DIMENSION));
                    }
                }
            }
        }

        //public void refreshSingleSprite(SpriteId spriteId, )

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
            return GEM_BAR.getCurrent();
        }
    }
}
