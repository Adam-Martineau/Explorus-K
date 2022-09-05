using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    abstract class Iterator
    {
        public abstract int[] Current();
        public abstract bool MoveLeft();
        public abstract bool MoveRight();
        public abstract bool MoveUp();
        public abstract bool MoveDown();
        public abstract int[] findPosition(string key);
    }

    /// <summary>
    /// The 'Aggregate' abstract class
    /// </summary>
    abstract class IteratorAggregate
    {
        public abstract Iterator CreateIterator();
    }

    class MapIterator : Iterator
    {
        private MapCollection _collection;

        private int[] _position = { -1, -1 };

        public MapIterator(MapCollection collection)
        {
            this._collection = collection;

             _position = findPosition("s");
        }

        public override int[] findPosition(string key)
        {
            // initialising result array to -1 in case keyString
            // is not found
            int[] result = { -1, -1 };

            // iteration over all the elements of the 2-D array
            // row
       
            Console.WriteLine(_collection.getLengthX());
            Console.WriteLine(_collection.getLengthY());
            for (int i = 0; i < _collection.getLengthX(); i++)
            {
                for (int j = 0; j < _collection.getLengthY(); j++)
                {
                    // if keyString is found
                    if (_collection.getMap()[i, j].Equals(key))
                    {
                        result[0] = i;
                        result[1] = j;
                        Console.WriteLine(result[0]);
                        Console.WriteLine(result[1]);
                        return result;
                    }
                }
            }

            // if keyString is not found then -1 is returned
            return result;
        }

        public override int[] Current()
        {
            return _position;
        }

        public override bool MoveLeft()
        {
            int updatedX = this._position[0] - 1;
            int updatedY = this._position[1];
            Console.WriteLine(updatedX);
            Console.WriteLine(updatedY);
            Console.WriteLine(_collection.getMap()[updatedX, updatedY]);

            if (updatedX > 0 && _collection.getMap()[updatedX, updatedY] != "w")
            {
                this._position = new int[] { updatedX, updatedY };
                return true;
            }
            else
            {
                Console.WriteLine("Wall detect");
                return false;
            }
        }

        public override bool MoveRight()
        {
            int updatedX = this._position[0] + 1;
            int updatedY = this._position[1];
            Console.WriteLine(updatedX);
            Console.WriteLine(updatedY);
            Console.WriteLine(_collection.getMap()[updatedX, updatedY]);

            if (updatedX < _collection.getLengthX() && _collection.getMap()[updatedX, updatedY] != "w")
            {
                this._position = new int[] { updatedX, updatedY };
                return true;
            }
            else
            {
                Console.WriteLine("Wall detect");
                return false;
            }
        }

        public override bool MoveUp()
        {
            int updatedX = this._position[0];
            int updatedY = this._position[1] - 1;
            Console.WriteLine(updatedX);
            Console.WriteLine(updatedY);
            Console.WriteLine(_collection.getMap()[updatedX, updatedY]);

            if (updatedY > 0 && _collection.getMap()[updatedX, updatedY] != "w")
            {
                this._position = new int[] { updatedX, updatedY };
                return true;
            }
            else
            {
                Console.WriteLine("Wall detect");
                return false;
            }
        }

        public override bool MoveDown()
        {
            int updatedX = this._position[0];
            int updatedY = this._position[1] + 1;
            Console.WriteLine(updatedX);
            Console.WriteLine(updatedY);
            Console.WriteLine(_collection.getMap()[updatedX, updatedY]);

            if (updatedY < _collection.getLengthY() && _collection.getMap()[updatedX, updatedY] != "w")
            {
                this._position = new int[] { updatedX, updatedY };
                return true;
            }
            else
            {
                Console.WriteLine("Wall detect");
                return false;
            }
        }
    }

    // Concrete Collections provide one or several methods for retrieving fresh
    // iterator instances, compatible with the collection class.
    class MapCollection : IteratorAggregate
    {
        string[,] _collection = null;

        public MapCollection(string[,] collection)
        {
            this._collection = collection;
        }

        public string[,] getMap()
        {
            return _collection;
        }
        public override Iterator CreateIterator()
        {
            return new MapIterator(this);
        }

        public int getLengthX()
        {
            return _collection.GetLength(0);
        }
        public int getLengthY()
        {
            return _collection.GetLength(1);
        }
    }
}
