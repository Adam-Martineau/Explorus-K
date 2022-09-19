using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    public class Image2D
    {
        private SpriteType id;
        private ImageType type;
        private int x;
        private int y;

        public int Y { get => y; set => y = value; }
        public int X { get => x; set => x = value; }

        public Image2D() { }

        public Image2D(SpriteType id, ImageType type)
        {
            this.id = id;
            this.type = type;
        }

        public Image2D(SpriteType id, ImageType type, int x, int y)
        {
            this.id = id;
            this.type = type;
            this.x = x;
            this.y = y;
        }

        public SpriteType getId()
        {
            return id;
        }

        public void setId(SpriteType id)
        {
            this.id = id;
        }

        public Image2D withId(SpriteType id)
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

        public Image2D withX(int x)
        {
            X = x;
            return this;
        }

        public Image2D withY(int y)
        {
            Y = y;
            return this;
        }

        public Bitmap getBitmapFromContainer()
        {
            return SpriteContainer.getInstance().getBitmapByImageType(type);
        }
    }
}
