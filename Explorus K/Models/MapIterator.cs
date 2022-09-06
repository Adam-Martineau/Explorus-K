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
        public abstract void MoveLeft();
        public abstract void MoveRight();
        public abstract void MoveUp();
        public abstract void MoveDown();
        public abstract bool isAbleToMoveLeft();
        public abstract bool isAbleToMoveRight();
        public abstract bool isAbleToMoveUp();
        public abstract bool isAbleToMoveDown();

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

            for (int i = 0; i < _collection.getLengthX(); i++)
            {
                for (int j = 0; j < _collection.getLengthY(); j++)
                {
                    // if keyString is found
                    if (_collection.getMap()[i, j].Equals(key))
                    {
                        result[0] = i;
                        result[1] = j;
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

        public override void MoveLeft()
        {
            int updatedX = this._position[0] - 1;
            int updatedY = this._position[1];

            if (isAbleToMoveLeft())
            {
                this._position = new int[] { updatedX, updatedY };
            }
        }

        public override void MoveRight()
        {
            int updatedX = this._position[0] + 1;
            int updatedY = this._position[1];

            if (isAbleToMoveRight())
            {
                this._position = new int[] { updatedX, updatedY };
            }
        }

        public override void MoveUp()
        {
            int updatedX = this._position[0];
            int updatedY = this._position[1] - 1;

            if (isAbleToMoveUp())
            {
                this._position = new int[] { updatedX, updatedY };
            }
        }

        public override void MoveDown()
        {
            int updatedX = this._position[0];
            int updatedY = this._position[1] + 1;

            if (isAbleToMoveDown())
            {
                this._position = new int[] { updatedX, updatedY };
            }
        }

        public override bool isAbleToMoveLeft()
        {
            int updatedX = this._position[0] - 1;
            int updatedY = this._position[1];

            if (updatedX >= 0 && _collection.getMap()[updatedX, updatedY] != "w")
            {
                return true;
            }

            return false;
        }

        public override bool isAbleToMoveRight()
        {
            int updatedX = this._position[0] + 1;
            int updatedY = this._position[1];

            if (updatedX < _collection.getLengthX() && _collection.getMap()[updatedX, updatedY] != "w")
            {
                return true;
            }

            return false;
        }

        public override bool isAbleToMoveDown()
        {
            int updatedX = this._position[0];
            int updatedY = this._position[1] + 1;

            if (updatedY < _collection.getLengthY() && _collection.getMap()[updatedX, updatedY] != "w")
            {
                return true;
            }

            return false;
        }

        public override bool isAbleToMoveUp()
        {
            int updatedX = this._position[0];
            int updatedY = this._position[1] - 1;

            if (updatedY >= 0 && _collection.getMap()[updatedX, updatedY] != "w")
            {
                return true;
            }

            return false;

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
