using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K.Models
{
    internal class SpriteContainer
    {
        private static SpriteContainer instance = new SpriteContainer();

        private const int IMAGE_DIMENSION = 48;

        private List<Image2D> image2DList;

        private SpriteContainer()
        {
            image2DList = new List<Image2D>();
            generateList();
        }

        public static SpriteContainer getInstance()
        {
            return instance;
        }

        public List<Image2D> getImage2DList()
        {
            return image2DList;
        }

        private void generateList()
        {
            image2DList.Add(new Image2D(1, ImageType.MUR, cropImage(new Rectangle(0, 0, IMAGE_DIMENSION * 2, IMAGE_DIMENSION * 2))));
            image2DList.Add(new Image2D(6, ImageType.SLIMUS_TITLE, cropImage(new Rectangle(IMAGE_DIMENSION * 2, 0, IMAGE_DIMENSION * 4, IMAGE_DIMENSION))));

        }


        private Bitmap cropImage(Rectangle cropArea)
        {
            Bitmap imageComplete = Explorus_K.Properties.Resources.TilesSheet;
            return imageComplete.Clone(cropArea, imageComplete.PixelFormat);
        }

        private List<Bitmap> extractSquareSpriteFromTileSheetRow(int startPointX, int startPointY, int numberToExtract, int spriteSquareSize)
        {
            List<Bitmap> bitmaps = new List<Bitmap>();

            for(int i = 0; i < numberToExtract; i++)
            {
                bitmaps.Add(cropImage(new Rectangle(startPointX, startPointY, spriteSquareSize, spriteSquareSize)));
                startPointX += spriteSquareSize;
            }

            return bitmaps;
        }

        private void fillSpriteListWithSmallSquare()
        {

        }

    }
}
