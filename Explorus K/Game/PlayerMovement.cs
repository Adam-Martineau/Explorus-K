using Explorus_K.Controllers;
using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K.Game
{
    internal class PlayerMovement
    {
        private Iterator iterator;
        private Random r = new Random();

        private const int playerStepSize = 2;

        public PlayerMovement(Iterator iterator)
        {
            this.iterator = iterator;
        }

        public bool canPlayerMove(MovementDirection movementDirection, Point position)
        {
            switch(movementDirection)
            {
                case MovementDirection.down:
                    return isWallOrDoor((String)iterator.getMapEntryAt(position.X, position.Y + 1));
                case MovementDirection.up:
                    return isWallOrDoor((String)iterator.getMapEntryAt(position.X, position.Y - 1));
                case MovementDirection.left:
                    return isWallOrDoor((String)iterator.getMapEntryAt(position.X - 1, position.Y));
                case MovementDirection.right:
                    return isWallOrDoor((String)iterator.getMapEntryAt(position.X + 1, position.Y));
                default:
                    return false;
            }
        }

        public void moveAndAnimatePlayer(List<Player> players)
        {
            foreach(Player player in players)
            {
                if(player.GetType() == typeof(ToxicSlime) && player.getMovementDirection() == MovementDirection.none)
                {
                    setToxicSlimeMovementDirection(player);
                }

                if(player.getMovementDirection() != MovementDirection.none)
                {
                    animatePlayer(player, player.getAnimationCount(), playerStepSize);
                }
            }
        }

        private void movePlayer(Player player, MovementDirection movementDirection, int stepSize)
        {
            switch (movementDirection)
            {
                case MovementDirection.down:
                    player.moveDown(stepSize);
                    break;
                case MovementDirection.up:
                    player.moveUp(stepSize);
                    break;
                case MovementDirection.left:
                    player.moveLeft(stepSize);
                    break;
                case MovementDirection.right:
                    player.moveRight(stepSize);
                    break;
                default:
                    break;
            }
            player.setMovementDirection(movementDirection);
        }

        private void animatePlayer(Player player, int count, int stepSize)
        {
            MovementDirection movementDirection = player.getMovementDirection();

            if (count < Constant.LARGE_SPRITE_DIMENSION && player.getMovementDirection() != MovementDirection.none)
            {
                count += stepSize;
                movePlayer(player, movementDirection, stepSize);

                if (count < 8)
                    player.setImageType(player.getAnimationDictValue(movementDirection, ((int)AnimationEnum.BIG)));
                else if (count > 8 && count < 16)
                    player.setImageType(player.getAnimationDictValue(movementDirection, ((int)AnimationEnum.MEDIUM)));
                else if (count > 16 && count < 32)
                    player.setImageType(player.getAnimationDictValue(movementDirection, ((int)AnimationEnum.SMALL)));
                else if (count > 32 && count < 40)
                    player.setImageType(player.getAnimationDictValue(movementDirection, ((int)AnimationEnum.MEDIUM)));
                else if (count > 40)
                    player.setImageType(player.getAnimationDictValue(movementDirection, ((int)AnimationEnum.BIG)));
            }
            else
            {
                movePlayerIterator(player, movementDirection);
                player.setMovementDirection(MovementDirection.none);
                count = 0;
            }

            player.setAnimationCount(count);
        }


        private bool isWallOrDoor(string destinationObject)
        {
            string wall = "w";
            string door = "p";
            return (destinationObject != wall && destinationObject != door);
        }

        private void setToxicSlimeMovementDirection(Player player)
        {

            int randomInt = r.Next(0, 3);

            MovementDirection movementDirection;

            switch (randomInt)
            {
                case 0:
                    movementDirection = MovementDirection.up;
                    if (canPlayerMove(movementDirection, player.getIterator().Current()))
                    {
                        player.setMovementDirection(movementDirection);
                    }
                    break;
                case 1:
                    movementDirection = MovementDirection.down;
                    if (canPlayerMove(movementDirection, player.getIterator().Current()))
                    {
                        player.setMovementDirection(movementDirection);
                    }
                    break;
                case 2:
                    movementDirection = MovementDirection.left;
                    if (canPlayerMove(movementDirection, player.getIterator().Current()))
                    {
                        player.setMovementDirection(movementDirection);
                    }
                    break;
                case 3:
                    movementDirection = MovementDirection.right;
                    if (canPlayerMove(movementDirection, player.getIterator().Current()))
                    {
                        player.setMovementDirection(movementDirection);
                    }
                    break;
                default:
                    break;
            }
        }

        private void movePlayerIterator(Player player, MovementDirection movementDirection)
        {
            switch (movementDirection)
            {
                case MovementDirection.down:
                    player.getIterator().MoveDown();
                    break;
                case MovementDirection.up:
                    player.getIterator().MoveUp();
                    break;
                case MovementDirection.left:
                    player.getIterator().MoveLeft();
                    break;
                case MovementDirection.right:
                    player.getIterator().MoveRight();
                    break;
                default:
                    break;
            }
        }
    }
}
