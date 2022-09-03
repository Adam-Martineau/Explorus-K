using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    internal class Image2D
    {
        private int id;
        private ImageType type;
        private int x;
        private int y;
        private Bitmap image;

        public int Y { get => y; set => y = value; }
        public int X { get => x; set => x = value; }

        public Image2D(int id, ImageType type, Bitmap image)
        {
            this.id = id;
            this.type = type;
            this.image = image;
        }

        public Image2D(int id, ImageType type, Bitmap image, int x, int y)
        {
            this.id = id;
            this.type = type;
            this.image = image;
            this.x = x;
            this.y = y;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public Image2D withId(int id)
        {
            setId(id);
            return this;
        }

        public ImageType getType()
        {
            return type;
        }

        public void setType(ImageType type)
        {
            this.type = type;
        }

        public Image2D withType(ImageType type)
        {
            setType(type);
            return this;
        }

        public Bitmap getImage()
        {
            return image;
        }

        public void setImage(Bitmap image)
        {
            this.image = image;
        }

        public Image2D withImage(Bitmap image)
        {
            setImage(image);
            return this;
        }
    }
}
