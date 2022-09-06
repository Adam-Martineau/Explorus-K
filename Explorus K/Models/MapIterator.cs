using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Models
{
    abstract class Iterator
    {
        public abstract Point Current();
        public abstract void MoveLeft();
        public abstract void MoveRight();
        public abstract void MoveUp();
        public abstract void MoveDown();
        public abstract bool isAbleToMoveLeft();
        public abstract bool isAbleToMoveRight();
        public abstract bool isAbleToMoveUp();
        public abstract bool isAbleToMoveDown();

        public abstract Point findPosition(object key);
    }

    /// <summary>
    /// The 'Aggregate' abstract class
    /// </summary>
    abstract class IteratorAggregate
    {
        public abstract Iterator CreateIterator(object key);
    }

    class MapIterator : Iterator
    {
        private MapCollection _collection;

        private Point _position = new Point(-1, -1);

        public MapIterator(MapCollection collection, object key)
        {
            this._collection = collection;

            _position = findPosition(key);
        }

        public override Point findPosition(object key)
        {
            // initialising result array to -1 in case keyString
            // is not found
            Point result = new Point(-1, -1);

            // iteration over all the elements of the 2-D array
            // row

            for (int i = 0; i < _collection.getLengthX(); i++)
            {
                for (int j = 0; j < _collection.getLengthY(); j++)
                {
                    // if keyString is found
                    if (_collection.getMap()[i, j].Equals(key))
                    {
                        result.X = i;
                        result.Y = j;
                        return result;
                    }
                }
            }

            // if keyString is not found then -1 is returned
            return result;
        }

        public override Point Current()
        {
            return _position;
        }

        public override void MoveLeft()
        {
            int updatedX = this._position.X - 1;
            int updatedY = this._position.Y;

            if (isAbleToMoveLeft())
            {
                this._position = new Point(updatedX, updatedY);
            }
        }

        public override void MoveRight()
        {
            int updatedX = this._position.X + 1;
            int updatedY = this._position.Y;

            if (isAbleToMoveRight())
            {
                this._position = new Point(updatedX, updatedY);
            }
        }

        public override void MoveUp()
        {
            int updatedX = this._position.X;
            int updatedY = this._position.Y - 1;

            if (isAbleToMoveUp())
            {
                this._position = new Point(updatedX, updatedY);
            }
        }

        public override void MoveDown()
        {
            int updatedX = this._position.X;
            int updatedY = this._position.Y + 1;

            if (isAbleToMoveDown())
            {
                this._position = new Point(updatedX, updatedY);
            }
        }

        public override bool isAbleToMoveLeft()
        {
            int updatedX = this._position.X - 1;
            int updatedY = this._position.Y;

            if (updatedX > 0)
            {
                return true;
            }

            return false;
        }

        public override bool isAbleToMoveRight()
        {
            int updatedX = this._position.X + 1;
            int updatedY = this._position.Y;

            if (updatedX < _collection.getLengthX())
            {
                return true;
            }

            return false;
        }

        public override bool isAbleToMoveDown()
        {
            int updatedX = this._position.X;
            int updatedY = this._position.Y + 1;

            if (updatedY < _collection.getLengthY())
            {
                return true;
            }

            return false;
        }

        public override bool isAbleToMoveUp()
        {
            int updatedX = this._position.X;
            int updatedY = this._position.Y - 1;

            if (updatedY > 0)
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
        public override Iterator CreateIterator(object key)
        {
            return new MapIterator(this, key);
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
