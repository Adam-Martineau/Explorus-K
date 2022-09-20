using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K.Models
{
    public class Image2D
    {
        private SpriteType spriteType;
        private ImageType type;
        private int x;
        private int y;

        public int radius { get; set;  }
        public int Y { get => y; set => y = value; }
        public int X { get => x; set => x = value; }
        public CollisionContext collisionStrategy { get; set; } = new CollisionContext();
        public Guid id { get; }

        public Image2D() { }

        public Image2D(SpriteType id, ImageType type)
        {
            this.spriteType = id;
            this.type = type;

            buildObj();
        }

        public Image2D(SpriteType id, ImageType type, int x, int y)
        {
            this.spriteType = id;
            this.type = type;
            this.x = x;
            this.y = y;

            buildObj();
        }

        public Image2D(SpriteType id, ImageType type, int x, int y, Guid id1) : this(id, type, x, y)
        {
            this.spriteType = id;
            this.type = type;
            this.x = x;
            this.y = y;
            this.id = id1;

            buildObj();
        }

        public SpriteType getId()
        {
            return spriteType;
        }

        public void setId(SpriteType id)
        {
            this.spriteType = id;
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

        private void buildObj()
        {
            if (spriteType == SpriteType.GEM)
            {
                collisionStrategy.SetStrategy(new GemStrategy());
                radius = Constant.SMALL_SPRITE_DIMENSION - 10;
            }
            else if (spriteType == SpriteType.DOOR)
            {
                collisionStrategy.SetStrategy(new DoorStrategy());
                radius = Constant.LARGE_SPRITE_DIMENSION - 20;
            }
            else if (spriteType == SpriteType.MINI_SLIMUS)
            {
                collisionStrategy.SetStrategy(new MiniSlimeStrategy());
                radius = Constant.SMALL_SPRITE_DIMENSION - 15;
            }
            else if (spriteType == SpriteType.TOXIC_SLIME)
            {
                collisionStrategy.SetStrategy(new ToxicSlimeStrategy());
                radius = Constant.LARGE_SPRITE_DIMENSION - 20;
            }
            else if (spriteType == SpriteType.BUBBLE)
            {
                collisionStrategy.SetStrategy(new ToxicSlimeStrategy());
                radius = Constant.SMALL_SPRITE_DIMENSION - 10;
            }
            else if (spriteType == SpriteType.SLIMUS)
            {
                radius = 13;
            }
        }
    }
}
