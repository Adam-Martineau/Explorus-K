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

        private const int playerStepSize = 2;

        public PlayerMovement(Iterator iterator)
        {
            this.iterator = iterator;
        }

        public bool canPlayerMove(MovementDirection MovementDirectionDirection, Point position)
        {
            try
            {
                switch (MovementDirectionDirection)
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
            catch
            {
                Console.WriteLine(position);
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

        private void movePlayer(Player player, MovementDirection movementDirectionDirection, int stepSize)
        {
            switch (movementDirectionDirection)
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
            player.setMovementDirection(movementDirectionDirection);
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
            Random r = new Random();
            int randomInt = r.Next(0, 3);

            MovementDirection movementDirection;

            Point iteratorPosition = iterator.findPosition(player.getLabyrinthName());

            switch (randomInt)
            {
                case 0:
                    movementDirection = MovementDirection.up;
                    if (canPlayerMove(movementDirection, iteratorPosition))
                    {
                        player.setMovementDirection(movementDirection);
                        iterator.replaceAt(".", iteratorPosition.X, iteratorPosition.Y);
                        iterator.replaceAt(player.getLabyrinthName(), iteratorPosition.X, iteratorPosition.Y - 1);
                    }
                    break;
                case 1:
                    movementDirection = MovementDirection.down;
                    if (canPlayerMove(movementDirection, iteratorPosition))
                    {
                        player.setMovementDirection(movementDirection);
                        iterator.replaceAt(".", iteratorPosition.X, iteratorPosition.Y);
                        iterator.replaceAt(player.getLabyrinthName(), iteratorPosition.X, iteratorPosition.Y + 1);
                    }
                    break;
                case 2:
                    movementDirection = MovementDirection.left;
                    if (canPlayerMove(movementDirection, iteratorPosition))
                    {
                        player.setMovementDirection(movementDirection);
                        iterator.replaceAt(".", iteratorPosition.X, iteratorPosition.Y);
                        iterator.replaceAt(player.getLabyrinthName(), iteratorPosition.X - 1, iteratorPosition.Y);
                    }
                    break;
                case 3:
                    movementDirection = MovementDirection.right;
                    if (canPlayerMove(movementDirection, iteratorPosition))
                    {
                        player.setMovementDirection(movementDirection);
                        iterator.replaceAt(".", iteratorPosition.X, iteratorPosition.Y);
                        iterator.replaceAt(player.getLabyrinthName(), iteratorPosition.X + 1, iteratorPosition.Y);
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
