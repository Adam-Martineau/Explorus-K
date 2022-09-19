using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Explorus_K.Models
{
    internal class Sprite
    {
        public SpriteId spriteId { get; set; }
        public int x_pos { get; set; }
        public int y_pos { get; set; }
        public int radius { get; set; }

        public Sprite (SpriteId spriteId, int x_pos, int y_pos, int radius)
        {
            this.spriteId = spriteId;
            this.x_pos = x_pos;
            this.y_pos = y_pos;
            this.radius = radius;
        }
    }
}
