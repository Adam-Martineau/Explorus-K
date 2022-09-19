﻿using Explorus_K.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Explorus_K.Models
{
    internal class Sprite
    {
        public SpriteType spriteType { get; set; }
        public int x_pos { get; set; }
        public int y_pos { get; set; }
        public int radius { get; set; }

        public Sprite (SpriteType spriteId, int x_pos, int y_pos, int radius)
        {
            this.spriteType = spriteId;
            this.x_pos = x_pos;
            this.y_pos = y_pos;
            this.radius = radius;
        }
    }
}
