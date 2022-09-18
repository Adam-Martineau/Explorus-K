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
    public interface Player
    {
        ImageType getImageType();

        int getPosX();

        int getPosY();

        int getLifes();

        int decreaseLife();

        MovementDirection getMovementDirection();

        void setMovementDirection(MovementDirection direction);

        void setImageType(ImageType imageType);

        void moveDown(int stepSize);

        void moveUp(int stepSize);

        void moveLeft(int stepSize);

        void moveRight(int stepSize);

        Image2D refreshPlayer();

        ImageType getAnimationDictValue(MovementDirection key, int value);

        int getAnimationCount();

        void setAnimationCount(int count);

        void setLabyrinthName(string name);

        string getLabyrinthName();
    }
}
