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
        private Image image;

        public Image2D(int id, ImageType type, Image image)
        {
            this.id = id;
            this.type = type;
            this.image = image;
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

        public Image getImage()
        {
            return image;
        }

        public void setImage(Image image)
        {
            this.image = image;
        }

        public Image2D withImage(Image image)
        {
            setImage(image);
            return this;
        }
    }
}
