using Explorus_K.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Explorus_K.Models
{

    public abstract class Iterator
    {
        public abstract Point Current();
        public abstract void Move(MovementDirection dir);
        public abstract void replaceAt(object newValue, int x, int y);

        public abstract object getMapEntryAt(int x, int y);

        public abstract Point getPosition(object key);
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

            _position = getPosition(key);
        }

        public override Point getPosition(object key)
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

        public override void Move(MovementDirection dir)
        {
            int updatedX = -1;
            int updatedY = -1;

            switch (dir)
            {
                case MovementDirection.left:
                    updatedX = this._position.X - 1;
                    updatedY = this._position.Y;

                    if (updatedX >= 0)
                    {
                        this._position = new Point(updatedX, updatedY);
                    }
                    break;
                case MovementDirection.right:
                    updatedX = this._position.X + 1;
                    updatedY = this._position.Y;

                    if (updatedX < _collection.getLengthX())
                    {
                        this._position = new Point(updatedX, updatedY);
                    }
                    break;
                case MovementDirection.up:
                    updatedX = this._position.X;
                    updatedY = this._position.Y - 1;

                    if (updatedY >= 0)
                    {
                        this._position = new Point(updatedX, updatedY);
                    }
                    break;
                case MovementDirection.down:
                    updatedX = this._position.X;
                    updatedY = this._position.Y + 1;

                    if (updatedY < _collection.getLengthY())
                    {
                        this._position = new Point(updatedX, updatedY);
                    }
                    break;
                default:
                    break;
            }
        }

        public override void replaceAt(object newValue, int x, int y)
        {
            if (x < _collection.getLengthX() && x >= 0 && y < _collection.getLengthX() && y >= 0)
            {
                _collection.getMap()[x, y] = newValue;
            }
        }

        public override object getMapEntryAt(int x, int y)
        {
            if (x < _collection.getLengthX() && x >= 0 && y < _collection.getLengthX() && y >= 0)
            {
                return _collection.getMap()[x, y];
            }
            return new object();
        }
    }

    // Concrete Collections provide one or several methods for retrieving fresh
    // iterator instances, compatible with the collection class.
    class MapCollection : IteratorAggregate
    {
        object[,] _collection = null;

        public MapCollection(object[,] collection)
        {
            this._collection = collection;
        }

        public object[,] getMap()
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
