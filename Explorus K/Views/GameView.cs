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
        public GameForm oGameForm;
        string gameTitle;

        private const double headerRatio = 0.125;
        private Point headerPosition = new Point();
        private Point labyrinthPosition = new Point();
        private double headerHeight = 0;
        private int bigSpriteDimension = 52;
        private int smallSpriteDimension = 26;
        private double labyrinthHeight = 48 * 9;
        private double labyrinthWidth = 48 * 11;

        private int screenWidth = 600;
        private int screenHeight = 600;

        private List<Image2D> AllSprites = new List<Image2D>();

        private Slimus slimus;

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
            oGameForm = new GameForm(gameEngine);
            oGameForm.Paint += new PaintEventHandler(this.GameRenderer);
            oGameForm.Size = new Size(screenWidth, screenHeight);
            oGameForm.MinimumSize = new Size(600, 600);
            headerHeight = screenHeight * headerRatio;
            headerPosition = new Point(0, 0);
            labyrinthPosition = new Point((int)((screenWidth-labyrinthWidth)/2), (int)headerHeight);
            Console.WriteLine(labyrinthPosition);
        }

        public void Show() 
        {
            Application.Run(oGameForm); 
        }

        public void Render()
        {
            if (oGameForm.Visible)
                oGameForm.BeginInvoke((MethodInvoker)delegate {
                    oGameForm.Refresh();
                });
        }

        public void Close()
        {
            if (oGameForm.Visible)
                oGameForm.BeginInvoke((MethodInvoker)delegate {
                    oGameForm.Close();
                });
        }

        public void Update(double elapsed)
        {
            i += (int) elapsed;
            double FPS = Math.Round(1000 / elapsed, 1);
            setWindowTitle("Explorus-K - FPS " + FPS.ToString());
        }

        private void GameRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            oGameForm.Text = gameTitle;

            foreach (Image2D sp in AllSprites)
            {
                SpriteId spriteId = sp.getId();
                if (spriteId == SpriteId.MINI_SLIMUS || spriteId == SpriteId.GEM)
                {
                    float pos = (bigSpriteDimension - smallSpriteDimension)/ 2;
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + pos), (float)(sp.Y + labyrinthPosition.Y + pos), smallSpriteDimension, smallSpriteDimension);
                }
                else if(spriteId == SpriteId.SLIMUS)
                {
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(slimus.getImageType()), slimus.getPosX(), slimus.getPosY() + labyrinthPosition.Y, bigSpriteDimension, bigSpriteDimension);
                }
                else
                {
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X), (float)(sp.Y + labyrinthPosition.Y), bigSpriteDimension, bigSpriteDimension);
                }
                
            }

            //Console.WriteLine("Render");
        }

        public void setWindowTitle(string title)
        {
            gameTitle = title;
        }

        public void OnLoad()
        {
            Console.WriteLine("ONLOAD::All Resources::Sprites Game Objects UI any thing you need to load before Engine start");

            for (int i = 0; i < Map.GetLength(0); i++)

            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == "w")
                        AllSprites.Add(new Image2D(SpriteId.WALL, ImageType.WALL, j * bigSpriteDimension, i * bigSpriteDimension));
                    else if (Map[i, j] == "g")
                        AllSprites.Add(new Image2D(SpriteId.GEM, ImageType.GEM, j * bigSpriteDimension, i * bigSpriteDimension));
                    else if (Map[i, j] == "m")
                    {
                        AllSprites.Add(new Image2D(SpriteId.MINI_SLIMUS, ImageType.SMALL_SLIMUS, j * bigSpriteDimension, i * bigSpriteDimension));
                    }
                    else if(Map[i, j] == "s")
                    {
                        slimus = new Slimus(j * bigSpriteDimension, i * bigSpriteDimension);
                        AllSprites.Add(new Image2D(SpriteId.SLIMUS, slimus.getImageType(), slimus.getPosX(), slimus.getPosY()));
                    }

                }
            }
        }

        //public void refreshSingleSprite(SpriteId spriteId, )

        public Slimus getSlimusObject()
        {
            return slimus;
        }
    }
}
