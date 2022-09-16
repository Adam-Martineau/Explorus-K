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

        private const int SMALL_SQUARE_DIM = 48;
        private const int BIG_SQUARE_DIM = 96;
        private const int TILESHEET_FIRST_ROW_FIRST_HALF = 0;
        private const int TILESHEET_FIRST_ROW_SECOND_HALF = 48;
        private const int TILESHEET_SECOND_ROW = 96;
        private const int TILESHEET_THIRD_ROW = 192;
        private const int TILESHEET_FOURTH_ROW = 288;
        private const int TILESHEET_FIFTH_ROW = 384;

        private Dictionary<ImageType, Bitmap> bitmapsDictionary;

        private SpriteContainer()
        {
            bitmapsDictionary = new Dictionary<ImageType, Bitmap>();
            generateList();
        }

        public static SpriteContainer getInstance()
        {
            return instance;
        }

        public Bitmap getBitmapByImageType(ImageType imageType)
        {
            if(!bitmapsDictionary.ContainsKey(imageType))
            {
                return new Bitmap(0, 0);
            }

            return bitmapsDictionary[imageType];
        }

        private void generateList()
        {
            bitmapsDictionary.Add(ImageType.WALL, cropImage(new Rectangle(0, TILESHEET_FIRST_ROW_FIRST_HALF, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_TITLE, cropImage(new Rectangle(BIG_SQUARE_DIM, TILESHEET_FIRST_ROW_FIRST_HALF, SMALL_SQUARE_DIM * 4, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.HEARTH, cropImage(new Rectangle(SMALL_SQUARE_DIM * 6, TILESHEET_FIRST_ROW_FIRST_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.BUBBLE_BIG, cropImage(new Rectangle(SMALL_SQUARE_DIM * 7, TILESHEET_FIRST_ROW_FIRST_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.BUBBLE_SMALL, cropImage(new Rectangle(SMALL_SQUARE_DIM * 8, TILESHEET_FIRST_ROW_FIRST_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.BUBBLE_EXPLODED, cropImage(new Rectangle(SMALL_SQUARE_DIM * 9, TILESHEET_FIRST_ROW_FIRST_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.GEM, cropImage(new Rectangle(SMALL_SQUARE_DIM * 10, TILESHEET_FIRST_ROW_FIRST_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.KEY, cropImage(new Rectangle(SMALL_SQUARE_DIM * 11, TILESHEET_FIRST_ROW_FIRST_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));

            bitmapsDictionary.Add(ImageType.LEFT_SIDE_BAR, cropImage(new Rectangle(BIG_SQUARE_DIM, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.HEALTH_BAR_FULL, cropImage(new Rectangle(SMALL_SQUARE_DIM * 3, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.HEALTH_BAR_HALF, cropImage(new Rectangle(SMALL_SQUARE_DIM * 4, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.BUBBLE_BAR_FULL, cropImage(new Rectangle(SMALL_SQUARE_DIM * 5, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.BUBBLE_BAR_HALF, cropImage(new Rectangle(SMALL_SQUARE_DIM * 6, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.GEM_BAR_FULL, cropImage(new Rectangle(SMALL_SQUARE_DIM * 7, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.GEM_BAR_HALF, cropImage(new Rectangle(SMALL_SQUARE_DIM * 8, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.EMPTY_BAR, cropImage(new Rectangle(SMALL_SQUARE_DIM * 9, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.RIGHT_SIDE_BAR, cropImage(new Rectangle(SMALL_SQUARE_DIM * 10, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SMALL_SLIMUS, cropImage(new Rectangle(SMALL_SQUARE_DIM * 11, TILESHEET_FIRST_ROW_SECOND_HALF, SMALL_SQUARE_DIM, SMALL_SQUARE_DIM)));

            bitmapsDictionary.Add(ImageType.SLIMUS_DOWN_ANIMATION_1, cropImage(new Rectangle(0, TILESHEET_SECOND_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_DOWN_ANIMATION_2, cropImage(new Rectangle(BIG_SQUARE_DIM, TILESHEET_SECOND_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_DOWN_ANIMATION_3, cropImage(new Rectangle(BIG_SQUARE_DIM * 2, TILESHEET_SECOND_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_RIGHT_ANIMATION_1, cropImage(new Rectangle(BIG_SQUARE_DIM * 3, TILESHEET_SECOND_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_RIGHT_ANIMATION_2, cropImage(new Rectangle(BIG_SQUARE_DIM * 4, TILESHEET_SECOND_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_RIGHT_ANIMATION_3, cropImage(new Rectangle(BIG_SQUARE_DIM * 5, TILESHEET_SECOND_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));

            bitmapsDictionary.Add(ImageType.SLIMUS_UP_ANIMATION_1, cropImage(new Rectangle(0, TILESHEET_THIRD_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_UP_ANIMATION_2, cropImage(new Rectangle(BIG_SQUARE_DIM, TILESHEET_THIRD_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_UP_ANIMATION_3, cropImage(new Rectangle(BIG_SQUARE_DIM * 2, TILESHEET_THIRD_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_LEFT_ANIMATION_1, cropImage(new Rectangle(BIG_SQUARE_DIM * 3, TILESHEET_THIRD_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_LEFT_ANIMATION_2, cropImage(new Rectangle(BIG_SQUARE_DIM * 4, TILESHEET_THIRD_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.SLIMUS_LEFT_ANIMATION_3, cropImage(new Rectangle(BIG_SQUARE_DIM * 5, TILESHEET_THIRD_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));

            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_DOWN_ANIMATION_1, cropImage(new Rectangle(0, TILESHEET_FOURTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_DOWN_ANIMATION_2, cropImage(new Rectangle(BIG_SQUARE_DIM, TILESHEET_FOURTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_DOWN_ANIMATION_3, cropImage(new Rectangle(BIG_SQUARE_DIM * 2, TILESHEET_FOURTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_RIGHT_ANIMATION_1, cropImage(new Rectangle(BIG_SQUARE_DIM * 3, TILESHEET_FOURTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_RIGHT_ANIMATION_2, cropImage(new Rectangle(BIG_SQUARE_DIM * 4, TILESHEET_FOURTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_RIGHT_ANIMATION_3, cropImage(new Rectangle(BIG_SQUARE_DIM * 5, TILESHEET_FOURTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));

            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_UP_ANIMATION_1, cropImage(new Rectangle(0, TILESHEET_FIFTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_UP_ANIMATION_2, cropImage(new Rectangle(BIG_SQUARE_DIM, TILESHEET_FIFTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_UP_ANIMATION_3, cropImage(new Rectangle(BIG_SQUARE_DIM * 2, TILESHEET_FIFTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_LEFT_ANIMATION_1, cropImage(new Rectangle(BIG_SQUARE_DIM * 3, TILESHEET_FIFTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_LEFT_ANIMATION_2, cropImage(new Rectangle(BIG_SQUARE_DIM * 4, TILESHEET_FIFTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));
            bitmapsDictionary.Add(ImageType.TOXIC_SLIME_LEFT_ANIMATION_3, cropImage(new Rectangle(BIG_SQUARE_DIM * 5, TILESHEET_FIFTH_ROW, BIG_SQUARE_DIM, BIG_SQUARE_DIM)));

        }

        private Bitmap cropImage(Rectangle cropArea)
        {
            Bitmap imageComplete = Explorus_K.Properties.Resources.TilesSheet;
            return imageComplete.Clone(cropArea, imageComplete.PixelFormat);
        }
    }
}
