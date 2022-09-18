using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game
{
    public class Labyrinth
    {
        /*private MapCollection map = new MapCollection(new string[,]{
            { "w", "w", "w", "w", "w", "w", "w", "w", "w"},
            { "w", "." ,".", ".", ".", ".", ".", ".", "w"},
            { "w", ".", "w", ".", "w", "w", "w", ".", "w"},
            { "w", "g", "w", ".", ".", ".", ".", ".", "w"},
            { "w", ".", "w", ".", "w", "w", "w", "s", "w"},
            { "w", ".", ".", ".", "w", "g", "w", "w", "w"},
            { "w", ".", "w", "w", "w", ".", ".", ".", "w"},
            { "w", ".", "w", "m", "p", ".", "w", ".", "w"},
            { "w", ".", "w", "w", "w", ".", "w", "g", "w"},
            { "w", ".", ".", ".", ".", ".", ".", ".", "w"},
            { "w", "w", "w", "w", "w", "w", "w", "w", "w"}
        });*/

        private MapCollection map = new MapCollection(new string[,]{
            { "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w"},
            { "w", ".", ".", "t0", ".", ".", ".", "w", ".", ".", ".", "t1", ".", ".", "w"},
            { "w", ".", "w", "w", "w", "w", ".", "w", ".", "w", "w", "w", "w", ".", "w"},
            { "w", ".", "w", ".", ".", ".", ".", ".", ".", ".", ".", ".", "w", ".", "w"},
            { "w", ".", ".", ".", "w", "w", "w", ".", "w", "w", "w", ".", ".", ".", "w"},
            { "w", "w", "w", ".", "w", ".", ".", ".", ".", ".", "w", "g", "w", "w", "w"},
            { "w", ".", ".", ".", "w", ".", "w", "w", "w", ".", "w", "g", ".", ".", "w"},
            { "w", ".", "w", ".", "w", ".", "w", ".", "w", ".", "w", "g", "w", ".", "w"},
            { "w", "t2", "w", ".", ".", "t3", "p", "m", "w", ".", ".", "g", "w", "s", "w"},
            { "w", ".", "w", ".", "w", ".", "w", ".", "w", ".", "w", "g", "w", ".", "w"},
            { "w", ".", ".", ".", "w", ".", "w", "w", "w", ".", "w", "g", ".", ".", "w"},
            { "w", "w", "w", ".", "w", ".", ".", ".", ".", ".", "w", ".", "w", "w", "w"},
            { "w", ".", ".", ".", "w", "w", "w", ".", "w", "w", "w", ".", ".", ".", "w"},
            { "w", ".", "w", ".", ".", ".", ".", ".", ".", ".", ".", ".", "w", ".", "w"},
            { "w", ".", "w", "w", "w", "w", ".", "w", ".", "w", "w", "w", "w", ".", "w"},
            { "w", ".", ".", "t5", ".", ".", ".", "w", ".", ".", ".", "t4", ".", ".", "w"},
            { "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w"}
        });

        private Iterator mapIterator = null;

        public Labyrinth()
        {
            mapIterator = map.CreateIterator("s");
        }

        internal Iterator MapIterator { get => mapIterator; set => mapIterator = value; }
        internal MapCollection Map { get => map; set => map = value; }

        public String getMapEntryAt(int x, int y)
        {
            return (String)mapIterator.getMapEntryAt(x, y);
        }
    }
}
