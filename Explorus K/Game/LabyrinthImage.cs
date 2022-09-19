using Explorus_K.Models;
using Explorus_K.NewFolder1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Explorus_K.Game
{
    public class LabyrinthImage
    {
        List<Image2D> labyrinthImages;
        Labyrinth labyrinth;
        Slimus slimus;
        Point labyrinthPosition;
        CollisionContext collisionStrategy = null;
        Context keyState = null;
        HealthBar healthBar = new HealthBar();
        BubbleBar bubbleBar = new BubbleBar();
        GemBar gemBar = new GemBar();
        List<Player> playerList = new List<Player>();

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

        public LabyrinthImage(Labyrinth labyrinth)
        {
            labyrinthPosition = new Point();
            labyrinthImages = new List<Image2D>();
            collisionStrategy = new CollisionContext();
            keyState = new Context(new NoKeyState());
            this.labyrinth = labyrinth;
            headerHeight = screenHeight * headerRatio;
            fillLabyrinthImages();
            labyrinthHeight = 48 * labyrinth.Map.getLengthY();
            labyrinthWidth = 48 * labyrinth.Map.getLengthX();

            invincibilityTimer = new Timer(100);
            invincibilityTimer.Elapsed += OnTimedEventInvincible;

            bubbleTimer = new Timer(500);
            bubbleTimer.Elapsed += OnTimedEventBubble;
            bubbleTimer.AutoReset = true;
            bubbleTimer.Enabled = true;
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

        public void InitializeHeaderBar(ProgressionBarCreator creator, int count)
        {
            IBar bar = creator.InitializeBar(count);

            if (creator.GetType() == typeof(HealthBarCreator))
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

        public void drawLabyrinthImage(Graphics g)
        {
            refreshPlayerSprite();

            foreach (Image2D sp in labyrinthImages.ToArray())
            {
                SpriteId spriteId = sp.getId();
                if (spriteId == SpriteId.MINI_SLIMUS || spriteId == SpriteId.GEM)
                {
                    float pos = (Constant.LARGE_SPRITE_DIMENSION - Constant.SMALL_SPRITE_DIMENSION) / 2;
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + labyrinthPosition.X + pos), (float)(sp.Y + labyrinthPosition.Y + pos), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
                }
                else if (spriteId == SpriteId.SLIMUS)
                {
                    Bitmap opacityImage = SetOpacity(new Bitmap(SpriteContainer.getInstance().getBitmapByImageType(slimus.getImageType())), slimusOpacity);
                    g.DrawImage(opacityImage, slimus.getPosX() + labyrinthPosition.X, slimus.getPosY() + labyrinthPosition.Y, Constant.LARGE_SPRITE_DIMENSION, Constant.LARGE_SPRITE_DIMENSION);
                }
                else if (spriteId == SpriteId.DOOR)
                {
                    Bitmap opacityImage = SetOpacity(new Bitmap(SpriteContainer.getInstance().getBitmapByImageType(sp.getType())), 0.4f);
                    g.DrawImage(opacityImage, (float)(sp.X + labyrinthPosition.X), (float)(sp.Y + labyrinthPosition.Y), Constant.LARGE_SPRITE_DIMENSION, Constant.LARGE_SPRITE_DIMENSION);
                }
                else if(spriteId == SpriteId.TOXIC_SLIME)
                {
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(sp.getType()), (float)(sp.X + labyrinthPosition.X), (float)(sp.Y + labyrinthPosition.Y), Constant.LARGE_SPRITE_DIMENSION, Constant.LARGE_SPRITE_DIMENSION);
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

        public GameState IsColliding(SpriteId sprite1, SpriteId sprite2)
		{
            int pixel = 0;
            if (sprite2 == SpriteId.GEM)
            {
                collisionStrategy.SetStrategy(new GemStrategy());
                pixel = 10;
            }
            else if (sprite2 == SpriteId.DOOR)
            {
                collisionStrategy.SetStrategy(new DoorStrategy());
                pixel = 5;
            }
            else if (sprite2 == SpriteId.MINI_SLIMUS)
            {
                collisionStrategy.SetStrategy(new MiniSlimeStrategy());
                pixel = 15;
            }
            else if (sprite2 == SpriteId.TOXIC_SLIME)
            {
                collisionStrategy.SetStrategy(new ToxicSlimeStrategy());
                pixel = 5;
            }

            float pos = (Constant.LARGE_SPRITE_DIMENSION - Constant.SMALL_SPRITE_DIMENSION) / 2;

            float slimusX = slimus.getPosX();
            float slimusY = slimus.getPosY() + labyrinthPosition.Y;

            for (int i = 0; i < labyrinthImages.Count; i++)
            {
                Image2D sp = labyrinthImages[i];
                
                if (sp.getId() == sprite2)
                {
                    float objectX = sp.X + pos;
                    float objectY = sp.Y + labyrinthPosition.Y + pos;

                    if (slimusX < objectX + Constant.SMALL_SPRITE_DIMENSION - pixel &&
                    slimusX + Constant.LARGE_SPRITE_DIMENSION - pixel > objectX &&
                    slimusY < objectY + Constant.SMALL_SPRITE_DIMENSION - pixel &&
                    slimusY + Constant.LARGE_SPRITE_DIMENSION - pixel > objectY)
                    {
                        if(sprite2 == SpriteId.TOXIC_SLIME)
                        {
                            if (slimus.getInvincible() == false)
                            {
                                startInvincibilityTimer();
                                return collisionStrategy.executeStrategy(this, i, sprite1);
                            }
                        }
                        else
                        {
                            return collisionStrategy.executeStrategy(this, i, sprite1);
                        }
                        
                        return GameState.PLAY;
                    }
                }
            }

            return GameState.PLAY;
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

        private void fillLabyrinthImages()
        {
            for (int i = 0; i < labyrinth.Map.getLengthX(); i++)
            {
                for (int j = 0; j < labyrinth.Map.getLengthY(); j++)
                {
                    if (labyrinth.getMapEntryAt(i, j) == "w")
                        labyrinthImages.Add(new Image2D(SpriteId.WALL, ImageType.WALL, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    else if (labyrinth.getMapEntryAt(i, j) == "g")
                        labyrinthImages.Add(new Image2D(SpriteId.GEM, ImageType.GEM, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    else if (labyrinth.getMapEntryAt(i, j) == "m")
                    {
                        labyrinthImages.Add(new Image2D(SpriteId.MINI_SLIMUS, ImageType.SMALL_SLIMUS, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    }
                    else if (labyrinth.getMapEntryAt(i, j) == "s")
                    {
                        slimus = new Slimus(i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION, ImageType.SLIMUS_DOWN_ANIMATION_1, Constant.SLIMUS_LIVES, Labyrinth.Map.CreateIterator("s"));
                        labyrinthImages.Add(new Image2D(SpriteId.SLIMUS, slimus.getImageType(), slimus.getPosX(), slimus.getPosY()));
                        playerList.Add(slimus);
                    }
                    else if (labyrinth.getMapEntryAt(i, j) == "p")
                    {
                        labyrinthImages.Add(new Image2D(SpriteId.DOOR, ImageType.WALL, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    }
                    else if (labyrinth.getMapEntryAt(i, j) != ".")
                    {
                        string tempLabyrinthName = "t" + labyrintNameCount.ToString();
                        ToxicSlime tempToxicSlime = new ToxicSlime(i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION, ImageType.TOXIC_SLIME_DOWN_ANIMATION_1, Constant.TOXIC_SLIME_LIVES, Labyrinth.Map.CreateIterator(tempLabyrinthName));
                        tempToxicSlime.setLabyrinthName(tempLabyrinthName);
                        labyrintNameCount++;
                        playerList.Add(tempToxicSlime);
                        labyrinthImages.Add(new Image2D(SpriteId.TOXIC_SLIME, ImageType.TOXIC_SLIME_DOWN_ANIMATION_1, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
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
                if(image.getId() == SpriteId.SLIMUS || image.getId() == SpriteId.TOXIC_SLIME)
                {
                    labyrinthImages.Remove(image);
                }
            }

            foreach(Player player in playerList)
            {
                labyrinthImages.Add(player.refreshPlayer());
            }
        }
    }
}
