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
    internal class ImageContainer
    {
        private static ImageContainer instance = new ImageContainer();

        private int IMAGE_DIMENSION = 48;
        private int IMAGE_SEPARATOR = 16;

        private List<Image2D> image2DList;

        private ImageContainer()
        {
            image2DList = new List<Image2D>();
            generateList();
        }

        public static ImageContainer getInstance()
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

        }


        private static Bitmap cropImage(Rectangle cropArea)
        {
            Bitmap imageComplete = Explorus_K.Properties.Resources.TilesSheet;
            return imageComplete.Clone(cropArea, imageComplete.PixelFormat);
        }




    }
}
