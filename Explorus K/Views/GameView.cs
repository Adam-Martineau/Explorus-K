﻿using Explorus_K.Controllers;
using Explorus_K.Models;
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

        private PictureBox gameHeader = new PictureBox();
        private PictureBox gameLabyrinth = new PictureBox();

        private int screenWidth = 600;
        private int screenHeight = 600;

        public static List<Image2D> AllSprites = new List<Image2D>();
        public List<Image2D> healthBar = new List<Image2D>();
        public List<Image2D> bubbleBar = new List<Image2D>();
        public List<Image2D> gemBar = new List<Image2D>();

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
            oGameForm.Size = new Size(screenWidth, screenHeight);
            oGameForm.MinimumSize = new Size(600, 600);
            headerHeight = screenHeight * headerRatio;
            headerPosition = new Point(0, 0);
            labyrinthPosition = new Point((int)((screenWidth-labyrinthWidth)/2), (int)headerHeight);

            gameHeader.Dock = DockStyle.Top;
            gameLabyrinth.Dock = DockStyle.Fill;

            gameHeader.Paint += new PaintEventHandler(this.HeaderRenderer);
            gameLabyrinth.Paint += new PaintEventHandler(this.LabyrinthRenderer);

            oGameForm.Controls.Add(gameHeader);
            oGameForm.Controls.Add(gameLabyrinth);
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

        private void HeaderRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            oGameForm.Text = gameTitle;

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.SLIMUS_TITLE), (screenWidth / 4) * 0, (float)((headerHeight-smallSpriteDimension)/2), smallSpriteDimension*4, smallSpriteDimension);

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.HEARTH), (int)((screenWidth/4)*0.95), (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
            
            foreach (Image2D image in healthBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 4) * 1)+(image.getId()* smallSpriteDimension), (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
            }

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.BUBBLE_BIG), (int)((screenWidth / 4) * 1.95), (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
            foreach (Image2D image in bubbleBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 4) * 2) + (image.getId() * smallSpriteDimension), (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
            }

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.GEM), (int)((screenWidth / 4) * 2.95), (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
            foreach (Image2D image in gemBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 4) * 3) + (image.getId() * smallSpriteDimension), (float)((headerHeight - smallSpriteDimension) / 2), smallSpriteDimension, smallSpriteDimension);
            }
        }

        private void LabyrinthRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            foreach (Image2D sp in AllSprites)
            {
                ImageType type = sp.getType();
                if (type == ImageType.SMALL_SLIMUS || type == ImageType.GEM)
                {
                    float pos = (bigSpriteDimension - smallSpriteDimension)/ 2;
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(type), (float)(sp.X + pos), (float)(sp.Y + labyrinthPosition.Y + pos), smallSpriteDimension, smallSpriteDimension);
                }
                else
                {
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(type), (float)(sp.X), (float)(sp.Y + labyrinthPosition.Y), bigSpriteDimension, bigSpriteDimension);
                }
            }
        }

        public void setWindowTitle(string title)
        {
            gameTitle = title;
        }

        public void OnLoad()
        {
            for (int i = 0; i < Map.GetLength(0); i++)

            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == "w")
                        AllSprites.Add(new Image2D(0, ImageType.WALL, j * bigSpriteDimension, i * bigSpriteDimension));
                    else if (Map[i, j] == "g")
                        AllSprites.Add(new Image2D(0, ImageType.GEM, j * bigSpriteDimension, i * bigSpriteDimension));
                    else if (Map[i, j] == "m")
                    {
                        AllSprites.Add(new Image2D(0, ImageType.SMALL_SLIMUS, j * bigSpriteDimension, i * bigSpriteDimension));
                    }
                    else if (Map[i, j] == "s")
                    {
                        AllSprites.Add(new Image2D(0, ImageType.SLIMUS_DOWN_ANIMATION_1, j * bigSpriteDimension, i * bigSpriteDimension));
                    }
                }
            }
            //Player = new Sprite2D(PlayerPostion, new Vector2(64 - StepSize, 64 - StepSize), Player_Ref, "Player");
        }

        public void InitializeHeaderBar(ProgressionBarCreator creator, int count)
        {
            List<Image2D> bar = creator.InitializeBar(count);

            if(creator.GetType() == typeof(HealthBarCreator))
            {
                this.healthBar = bar;
            }
            else if (creator.GetType() == typeof(BubbleBarCreator))
            {
                this.bubbleBar = bar;
            }
            else if (creator.GetType() == typeof(GemBarCreator))
            {
                this.gemBar = bar;
            }
        }

        public static void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyValue)
            {
                case (char)Keys.Right:
                    //do something;
                case (char)Keys.Left:
                    //do something;
                case (char)Keys.Up:
                    //do something;
                case (char)Keys.Down:
                //do something;
                default:
                    //do nothing
                    break;
            }
        }
    }
}
