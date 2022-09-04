using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Size = System.Drawing.Size;

namespace Explorus_K.Views
{
    class GameView
    {
        public GameForm oGameForm;
        string gameTitle;

        private Vector ScreenSize = new Vector(1080, 864);

        public static List<Image2D> AllSprites = new List<Image2D>();

        readonly string[,] Map =
        {
            {"w","w","w","w","w","w","w","w","w","w","w"},
            {"w",".",".","g",".",".",".",".",".",".","w"},
            {"w",".","w","w","w",".","w","w","w",".","w"},
            {"w",".",".",".",".",".","w",".","w",".","w"},
            {"w",".","w",".","w","w","w","w","w",".","w"},
            {"w",".","w",".","w","g",".",".",".",".","w"},
            {"w",".","w",".","w","w",".","w","w",".","w"},
            {"w",".",".",".","s","w",".",".",".",".","w"},
            {"w","w","w","w","w","w","w","w","w","w","w"}
        };

        int i = 0;
        

        public GameView()
        {
            oGameForm = new GameForm();
            oGameForm.Paint += new PaintEventHandler(this.GameRenderer);
            oGameForm.Size = new Size((int)this.ScreenSize.X, (int)this.ScreenSize.Y);
            oGameForm.MinimumSize = new Size(600, 600);
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
            //g.DrawImage(SpriteContainer.getInstance().getImage2DList()[0].getImage(), 20, 20);
            oGameForm.Text = gameTitle;

            foreach (Image2D sp in AllSprites)
            {
                g.DrawImage(sp.getImage(), sp.X, sp.Y);
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
                        AllSprites.Add(new Image2D(0, ImageType.MUR, SpriteContainer.getInstance().getImage2DList()[0].getImage(), j * 96, i * 96));
                    
                    /*else if (Map[i, j] == "b")
                        new Sprite2D(new Vector2(j * 64, i * 64), new Vector2(64, 64), SemSolidBlocks_Ref, "SemiSolidBlock");
                    else if (Map[i, j] == "P")
                    {
                        PlayerPostion = new Vector2(j * 64, i * 64);
                        new Sprite2D(new Vector2(j * 64, i * 64), new Vector2(64, 64), Ground_Ref, "Ground");
                    }
                    else if (Map[i, j] == "E")
                    {
                        new Sprite2D(new Vector2(j * 64, i * 64), new Vector2(64, 64), Ground_Ref, "Ground");
                        new Sprite2D(new Vector2(j * 64 + 64 / 4, i * 64 + 64 / 4), new Vector2(64 / 2, 64 / 2), EnemyBomb_Ref, "EnemyBomb");
                    }
                    else if (Map[i, j] == ".")
                        new Sprite2D(new Vector2(j * 64, i * 64), new Vector2(64, 64), Ground_Ref, "Ground");
                    else if (Map[i, j] == "c")
                    {
                        new Sprite2D(new Vector2(j * 64, i * 64), new Vector2(64, 64), Ground_Ref, "Ground");
                        new Sprite2D(new Vector2(j * 64, i * 64), new Vector2(64, 64), CoinGold_Ref, "CoinGold");
                    }*/

                }
            }
            //Player = new Sprite2D(PlayerPostion, new Vector2(64 - StepSize, 64 - StepSize), Player_Ref, "Player");
        }
    }
}
