using Explorus_K.Models;
using Explorus_K.NewFolder1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game
{
    internal class LabyrinthImage
    {
        List<Image2D> labyrinthImages;
        Labyrinth labyrinth;
        Player slimus;
        Point labyrinthPosition;

        private double labyrinthHeight = 48 * 9;
        private double labyrinthWidth = 48 * 11;
        private int headerOffset = 0;
        private double headerHeight = 0;
        private const double headerRatio = 0.125;

        private int screenWidth = 600;
        private int screenHeight = 600;


        public LabyrinthImage(Labyrinth labyrinth)
        {
            labyrinthPosition = new Point();
            labyrinthImages = new List<Image2D>();
            this.labyrinth = labyrinth;
            headerHeight = screenHeight * headerRatio;
            fillLabyrinthImages();
        }

        public void removeImageAt(int index)
        {
            labyrinthImages.RemoveAt(index);
        }

        public Player getSlimus()
        {
            return slimus;
        }

        public void drawLabyrinthImage(Graphics g)
        {
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
                    g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(slimus.getImageType()), slimus.getPosX() + labyrinthPosition.X, slimus.getPosY() + labyrinthPosition.Y, Constant.LARGE_SPRITE_DIMENSION, Constant.LARGE_SPRITE_DIMENSION);
                }
                else if (spriteId == SpriteId.DOOR)
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

        public void drawHeader(Graphics g, HealthBar healthBar, GemBar gemBar, BubbleBar bubbleBar, Context keyState)
        {
            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.SLIMUS_TITLE), headerOffset, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION * 4, Constant.SMALL_SPRITE_DIMENSION);

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.HEARTH), (int)((screenWidth / 5) * 0.95) + headerOffset, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);

            foreach (Image2D image in healthBar.healthBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 5) * 1) + (image.X * Constant.SMALL_SPRITE_DIMENSION) + headerOffset, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            }

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.BUBBLE_BIG), (int)((screenWidth / 5) * 1.95) + headerOffset, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            foreach (Image2D image in bubbleBar.bubbleBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 5) * 2) + (image.X * Constant.SMALL_SPRITE_DIMENSION) + headerOffset, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            }

            g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.GEM), (int)((screenWidth / 5) * 2.95) + headerOffset, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            foreach (Image2D image in gemBar.gemBar)
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(image.getType()), ((screenWidth / 5) * 3) + (image.X * Constant.SMALL_SPRITE_DIMENSION) + headerOffset, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            }

            if (keyState.CurrentState() == "WithKeyState")
            {
                g.DrawImage(SpriteContainer.getInstance().getBitmapByImageType(ImageType.KEY), ((screenWidth / 5) * 4) + headerOffset, (float)((headerHeight - Constant.SMALL_SPRITE_DIMENSION) / 2), Constant.SMALL_SPRITE_DIMENSION, Constant.SMALL_SPRITE_DIMENSION);
            }
        }

        public bool IsColliding(int pixel, SpriteId sprite)
		{

            float pos = (Constant.LARGE_SPRITE_DIMENSION - Constant.SMALL_SPRITE_DIMENSION) / 2;

            float slimusX = slimus.getPosX();
            float slimusY = slimus.getPosY() + labyrinthPosition.Y;

            for (int i = 0; i < labyrinthImages.Count; i++)
            {
                Image2D sp = labyrinthImages[i];
                if (sp.getId() == sprite)
                {
                    float objectX = sp.X + pos;
                    float objectY = sp.Y + labyrinthPosition.Y + pos;

                    if (slimusX < objectX + Constant.SMALL_SPRITE_DIMENSION - pixel &&
                    slimusX + Constant.LARGE_SPRITE_DIMENSION - pixel > objectX &&
                    slimusY < objectY + Constant.SMALL_SPRITE_DIMENSION - pixel &&
                    slimusY + Constant.LARGE_SPRITE_DIMENSION - pixel > objectY)
                    {
                        labyrinthImages.RemoveAt(i);

                        return true;
                    }
                }
            }

            return false;
		}

        public void resize(GameForm gameForm)
        {

            labyrinthPosition = new Point((gameForm.Size.Width / 2) - ((int)labyrinthWidth / 2) - 31,
                               ((gameForm.Size.Height - (int)headerHeight) / 2) - ((int)labyrinthHeight / 2) + 5);

            headerOffset = (gameForm.Size.Width / 2) - 250;
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
                        slimus = new Player(i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION);
                        labyrinthImages.Add(new Image2D(SpriteId.SLIMUS, slimus.getImageType(), slimus.getPosX(), slimus.getPosY()));
                    }
                    else if (labyrinth.getMapEntryAt(i, j) == "s")
                    {
                        labyrinthImages.Add(new Image2D(0, ImageType.SLIMUS_DOWN_ANIMATION_1, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
                    }
                    else if (labyrinth.getMapEntryAt(i, j) == "p")
                        labyrinthImages.Add(new Image2D(SpriteId.DOOR, ImageType.WALL, i * Constant.LARGE_SPRITE_DIMENSION, j * Constant.LARGE_SPRITE_DIMENSION));
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
    }
}
