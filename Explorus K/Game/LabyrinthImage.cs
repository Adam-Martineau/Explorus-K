using Explorus_K.Controllers;
using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;

namespace Explorus_K.Game
{
    public class LabyrinthImage
    {
        public List<Image2D> labyrinthImages;
        Labyrinth labyrinth;
        public Slimus slimus;
        Point labyrinthPosition;
        CollisionContext collisionStrategy = null;
        Context keyState = null;
        HealthBar healthBar = null;
        BubbleBar bubbleBar = null;
        GemBar gemBar = null;
        List<Player> playerList = new List<Player>();

        public BubbleManager bubbleManager;

        private static Timer invincibilityTimer;
        private static Timer bubbleTimer;
        private float slimusOpacity = 1.0f;
        private int numberOfTrigger = 0;

        private double labyrinthHeight = 0;
        private double labyrinthWidth = 0;
        private int headerOffset = 0;
        private double headerHeight = 0;
        private const double headerRatio = 0.125;

        private int screenWidth = 1000;
        private int screenHeight = 600;

        private int labyrintNameCount = 0;

        internal Context KeyState { get => keyState; set => keyState = value; }
        internal HealthBar HealthBar { get => healthBar; set => healthBar = value; }
        internal BubbleBar BubbleBar { get => bubbleBar; set => bubbleBar = value; }
        internal GemBar GemBar { get => gemBar; set => gemBar = value; }
        internal Labyrinth Labyrinth { get => labyrinth; set => labyrinth = value; }

        public LabyrinthImage(Labyrinth labyrinth, BubbleManager bubbleManager, GameDifficulty diff)
        {
            labyrinthPosition = new Point();
            labyrinthImages = new List<Image2D>();
            collisionStrategy = new CollisionContext();
            keyState = new Context(new NoKeyState());
            this.labyrinth = labyrinth;
            headerHeight = screenHeight * headerRatio;
            fillLabyrinthImages(diff);
            labyrinthHeight = 48 * labyrinth.Map.getLengthY();
            labyrinthWidth = 48 * labyrinth.Map.getLengthX();

            invincibilityTimer = new Timer(100);
            invincibilityTimer.Elapsed += OnTimedEventInvincible;
            numberOfTrigger = 0;

            bubbleTimer = new Timer(bubbleManager.getBubbleTimer());
            bubbleTimer.Elapsed += OnTimedEventBubble;
            bubbleTimer.AutoReset = true;
            bubbleTimer.Enabled = true;

            this.bubbleManager = bubbleManager;
        }

        public void setBubbleTimerInterval(int timer)
        {
            bubbleTimer.Interval = timer;
        }

        public void setSlimusLives(int lives)
        {
            slimus.setLives(lives);
            IBar bar = new HealthBarCreator().InitializeBar(lives, lives);
            healthBar = (HealthBar)bar;
        }

        public void setToxicLives(int lives)
        {
            foreach (Player p in playerList)
            {
                if (p.getLabyrinthName() != "s")
                {
                    p.setLives(lives);
                }
            }
        }

        public void removeImageAt(int index)
        {
            labyrinthImages.RemoveAt(index);
        }

        public Player getSlimus()
        {
            foreach(Player player in playerList)
            {
                if(player.GetType() == typeof(Slimus))
                {
                    return player;
                }
            }

            return null;
        }

        public void drawLabyrinthImage(Graphics g)
        {
            refreshPlayerSprite();

            foreach (Image2D sp in labyrinthImages.ToArray())
            {
                SpriteType SpriteType = sp.getId();
                if (SpriteType == SpriteType.MINI_SLIMUS || SpriteType == SpriteType.GEM)
                {
                    float pos = (Constant.LARGE_SPRITE_DIMENSION - Constant.SMALL_SPRITE_DIMENSION) / 2;
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + labyrinthPosition.X + pos), (float)(sp.Y + labyrinthPosition.Y + pos), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
                }
                else if (SpriteType == SpriteType.SLIMUS)
                {
                    Bitmap opacityImage = SetOpacity(new Bitmap(SpriteContainer.getInstance().getBitmapByImageType(slimus.getImageType())), slimusOpacity);
                    g.DrawImage(opacityImage, slimus.getPosX() + labyrinthPosition.X, slimus.getPosY() + labyrinthPosition.Y, Constant.LARGE_SPRITE_DIMENSION, Constant.LARGE_SPRITE_DIMENSION);
                }
                else if (SpriteType == SpriteType.DOOR)
                {
                    Bitmap opacityImage = SetOpacity(new Bitmap(SpriteContainer.getInstance().getBitmapByImageType(sp.getType())), 0.4f);
                    g.DrawImage(opacityImage, (float)(sp.X + labyrinthPosition.X), (float)(sp.Y + labyrinthPosition.Y), Constant.LARGE_SPRITE_DIMENSION, Constant.LARGE_SPRITE_DIMENSION);
                }
                else
                {
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + labyrinthPosition.X), (float)(sp.Y + labyrinthPosition.Y), Constant.LARGE_SPRITE_DIMENSION, Constant.LARGE_SPRITE_DIMENSION);
                }
            }
        }

        public void drawHeader(Graphics g)
        {
            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.SLIMUS_TITLE), getWidthColumn(0, 2) - ((Constant.SMALL_SPRITE_DIMENSION * 4) / 2), (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION * 4, Constant.SMALL_SPRITE_DIMENSION);

            int healthBarWidth = (healthBar.getLength()+2) * Constant.SMALL_SPRITE_DIMENSION;
            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.HEARTH), getWidthColumn(2, 3) - (healthBarWidth / 2) - Constant.SMALL_SPRITE_DIMENSION, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);

            foreach (Image2D image in healthBar.healthBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), getWidthColumn(2, 3) - (healthBarWidth / 2) + (image.X * Constant.SMALL_SPRITE_DIMENSION), (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            }

            int bubbleBarWidth = (bubbleBar.getLength() + 2) * Constant.SMALL_SPRITE_DIMENSION;
            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.BUBBLE_BIG), getWidthColumn(5, 3) - (bubbleBarWidth / 2) - Constant.SMALL_SPRITE_DIMENSION, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            foreach (Image2D image in bubbleBar.bubbleBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), getWidthColumn(5, 3) - (bubbleBarWidth / 2) + (image.X * Constant.SMALL_SPRITE_DIMENSION), (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            }

            int gemBarWidth = (gemBar.getLength() + 2) * Constant.SMALL_SPRITE_DIMENSION;
            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.GEM), getWidthColumn(8, 3) - (gemBarWidth / 2) - Constant.SMALL_SPRITE_DIMENSION, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            foreach (Image2D image in gemBar.gemBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), getWidthColumn(8, 3) - (gemBarWidth / 2) + (image.X * Constant.SMALL_SPRITE_DIMENSION), (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            }

            if (keyState.CurrentState() == "WithKeyState")
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.KEY), getWidthColumn(11, 1) - (Constant.SMALL_SPRITE_DIMENSION / 2), (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            }
        }

        private int getWidthColumn(int index, int column)
        {
            return (headerOffset * index) + ((headerOffset * column)/2);
        }
        private void OnTimedEventInvincible(Object source, ElapsedEventArgs e)
        {
            numberOfTrigger += 1;
            if (numberOfTrigger < 30)
            {
                if (slimusOpacity == 1.0f)
                {
                    slimusOpacity = 0.4f;
                }
                else
                {
                    slimusOpacity = 1.0f;
                }
            }
            else
            {
                slimusOpacity = 1.0f;
                invincibilityTimer.Stop();
                slimus.setInvincible(false);
            }
        }

        public void startInvincibilityTimer()
        {
            invincibilityTimer.Start();
            numberOfTrigger = 0;
            slimus.setInvincible(true);
        }

        public void stopInvincibilityTimer()
        {
            invincibilityTimer.Stop();
            slimus.setInvincible(false);
        }

        private void OnTimedEventBubble(Object source, ElapsedEventArgs e)
        {
            if(bubbleBar.getCurrent() != bubbleBar.getLength())
            {
                bubbleBar.Increase();
            }
        }

        public void startBubbleTimer()
        {
            bubbleTimer.Start();
        }

        public void resize(GameForm gameForm)
        {

            labyrinthPosition = new Point((gameForm.Size.Width / 2) - ((int)labyrinthWidth / 2) - 31,
                               ((gameForm.Size.Height - (int)headerHeight) / 2) - ((int)labyrinthHeight / 2) + 5);

            headerOffset = (gameForm.Size.Width / 12);
        }

        public List<Player> getPlayerList()
        {
            return playerList;
        }    

        private void fillLabyrinthImages(GameDifficulty diff)
        {
            for (int i = 0; i < labyrinth.Map.getLengthX(); i++)
            {
                for (int j = 0; j < labyrinth.Map.getLengthY(); j++)
                {
                    if (labyrinth.getMapEntryAt(i, j) == "w")
                        labyrinthImages.Add(new Image2D(SpriteType.WALL, ImageType.WALL, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    else if (labyrinth.getMapEntryAt(i, j) == "g")
                        labyrinthImages.Add(new Image2D(SpriteType.GEM, ImageType.GEM, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    else if (labyrinth.getMapEntryAt(i, j) == "m")
                    {
                        labyrinthImages.Add(new Image2D(SpriteType.MINI_SLIMUS, ImageType.SMALL_SLIMUS, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    }
                    else if (labyrinth.getMapEntryAt(i, j) == "s")
                    {
                        slimus = new Slimus(i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION, ImageType.SLIMUS_DOWN_ANIMATION_1, diff.getSlimusLives(), Labyrinth.Map.CreateIterator("s"));
                        labyrinthImages.Add(new Image2D(SpriteType.SLIMUS, slimus.getImageType(), slimus.getPosX(), slimus.getPosY()));
                        playerList.Add(slimus);
                    }
                    else if (labyrinth.getMapEntryAt(i, j) == "p")
                    {
                        labyrinthImages.Add(new Image2D(SpriteType.DOOR, ImageType.WALL, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    }
                    else if (labyrinth.getMapEntryAt(i, j) != ".")
                    {
                        string tempLabyrinthName = "t" + labyrintNameCount.ToString();
                        ToxicSlime tempToxicSlime = new ToxicSlime(i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION, ImageType.TOXIC_SLIME_DOWN_ANIMATION_1, diff.getToxicLives(), Labyrinth.Map.CreateIterator(tempLabyrinthName));
                        tempToxicSlime.setLabyrinthName(tempLabyrinthName);
                        labyrintNameCount++;
                        playerList.Add(tempToxicSlime);
                        labyrinthImages.Add(new Image2D(SpriteType.TOXIC_SLIME, ImageType.TOXIC_SLIME_DOWN_ANIMATION_1, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION,tempToxicSlime.GetGuid()));
                    }
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

                Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);

                gr.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return output;
        }

        private void refreshPlayerSprite()
        {
            List<Image2D> tempSpriteList = new List<Image2D>(labyrinthImages);

            foreach(Image2D image in tempSpriteList)
            {
                if(image.getId() == SpriteType.SLIMUS || image.getId() == SpriteType.TOXIC_SLIME || image.getId() == SpriteType.BUBBLE)
                {
                    labyrinthImages.Remove(image);
                }
            }

            foreach(Player player in new List<Player>(playerList))
            {
                labyrinthImages.Add(player.refreshPlayer());
            }

            foreach (Bubble bubble in bubbleManager.getBubbleList())
            {
                labyrinthImages.Add(bubble.refreshBubble());
            }
        }
    }
}
