using Explorus_K.Game;
using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Automation;

namespace Explorus_K
{
    public abstract class Player : Movement
    {
        protected ImageType imageType;
        protected int lifeCount;
        protected string labyrinthName;
        protected Iterator iterator;
        protected Guid id = Guid.NewGuid();
        protected Dictionary<MovementDirection, List<ImageType>> animationDict;

        public Player(int posX, int posY, ImageType imageType, int life, Iterator iterator) : base(posX, posY)
        {
            this.imageType = imageType;
            this.lifeCount = life;
            this.iterator = iterator;
            movementDirection = MovementDirection.none;
        }

        public ImageType getImageType()
        {
            return imageType;
        }

        public int getLifes()
        {
            return lifeCount;
        }

        public int decreaseLife()
        {
            return lifeCount--;
        }

        public void setLives(int lives)
        {
            this.lifeCount = lives;
        }

        public Guid GetGuid()
        {
            return this.id;
        }

        public void setImageType(ImageType imageType)
        {
            this.imageType = imageType;
        }

        public void setLabyrinthName(string name)
        {
            labyrinthName = name;
        }

        public string getLabyrinthName()
        {
            return labyrinthName;
        }

        public Iterator getIterator()
        {
            return iterator;
        }

        public ImageType getAnimationDictValue(MovementDirection key, int value)
        {
            return animationDict[key][value];
        }

        public abstract Image2D refreshPlayer();
        protected abstract void fillAnimationDict();
    }
}
