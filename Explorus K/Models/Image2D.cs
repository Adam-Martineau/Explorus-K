using Explorus_K.NewFolder1;
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
        private SpriteId id;
        private ImageType type;
        private int x;
        private int y;

        public int Y { get => y; set => y = value; }
        public int X { get => x; set => x = value; }

        public Image2D(SpriteId id, ImageType type)
        {
            this.id = id;
            this.type = type;
        }

        public Image2D(SpriteId id, ImageType type, int x, int y)
        {
            this.id = id;
            this.type = type;
            this.x = x;
            this.y = y;
        }

        public SpriteId getId()
        {
            return id;
        }

        public void setId(SpriteId id)
        {
            this.id = id;
        }

        public Image2D withId(SpriteId id)
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
        
        public Bitmap getBitmapFromContainer()
        {
            return SpriteContainer.getInstance().getBitmapByImageType(type);
        }
    }
}
