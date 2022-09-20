using Explorus_K.Controllers;
using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
        private const int bubbleStepSize = 4;

        public PlayerMovement(Iterator iterator)
        {
            this.iterator = iterator;
        }

        public bool canPlayerMove(MovementDirection movementDirection, Point position, SpriteType sprite)
        {
            switch(movementDirection)
            {
                case MovementDirection.down:
                    return isWallOrDoor((String)iterator.getMapEntryAt(position.X, position.Y + 1), sprite);
                case MovementDirection.up:
                    return isWallOrDoor((String)iterator.getMapEntryAt(position.X, position.Y - 1), sprite);
                case MovementDirection.left:
                    return isWallOrDoor((String)iterator.getMapEntryAt(position.X - 1, position.Y), sprite);
                case MovementDirection.right:
                    return isWallOrDoor((String)iterator.getMapEntryAt(position.X + 1, position.Y), sprite);
                default:
                    return false;
            }
        }

        public void moveAndAnimateBubbles(BubbleManager bubbleManager)
        {
            List<Bubble> bubbles = new List<Bubble>(bubbleManager.getBubbleList());

            foreach (Bubble bubble in bubbles)
            {
                animateBubble(bubble, bubbleManager);
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


        private bool isWallOrDoor(string destinationObject, SpriteType type)
        {
            string wall = "w";
            string door = "p";
            string lockedDoor = "l";
            if (type == SpriteType.SLIMUS)
            {
                return (destinationObject != wall && destinationObject != door);
            }
            else
            {
                return (destinationObject != wall && destinationObject != door && destinationObject != lockedDoor);
            }
        }

        private void setToxicSlimeMovementDirection(Player player)
        {

            int randomInt = r.Next(0, 4);

            MovementDirection movementDirection;

            switch (randomInt)
            {
                case 0:
                    movementDirection = MovementDirection.up;
                    if (canPlayerMove(movementDirection, player.getIterator().Current(),SpriteType.TOXIC_SLIME))
                    {
                        player.setMovementDirection(movementDirection);
                    }
                    break;
                case 1:
                    movementDirection = MovementDirection.down;
                    if (canPlayerMove(movementDirection, player.getIterator().Current(), SpriteType.TOXIC_SLIME))
                    {
                        player.setMovementDirection(movementDirection);
                    }
                    break;
                case 2:
                    movementDirection = MovementDirection.left;
                    if (canPlayerMove(movementDirection, player.getIterator().Current(), SpriteType.TOXIC_SLIME))
                    {
                        player.setMovementDirection(movementDirection);
                    }
                    break;
                case 3:
                    movementDirection = MovementDirection.right;
                    if (canPlayerMove(movementDirection, player.getIterator().Current(), SpriteType.TOXIC_SLIME))
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

        private void moveBubbleIterator(Bubble bubble, MovementDirection movementDirection)
        {
            int bubbleIteratorX = bubble.getIteratorPosition().X;
            int bubbleIteratorY = bubble.getIteratorPosition().Y;

            switch (movementDirection)
            {
                case MovementDirection.down:
                    bubble.setIteratorPosition(new Point(bubbleIteratorX, bubbleIteratorY + 1));
                    break;
                case MovementDirection.up:
                    bubble.setIteratorPosition(new Point(bubbleIteratorX, bubbleIteratorY - 1));
                    break;
                case MovementDirection.left:
                    bubble.setIteratorPosition(new Point(bubbleIteratorX - 1, bubbleIteratorY));
                    break;
                case MovementDirection.right:
                    bubble.setIteratorPosition(new Point(bubbleIteratorX + 1, bubbleIteratorY));
                    break;
                default:
                    break;
            }
        }

        private void moveBubble(Bubble bubble)
        {
            MovementDirection movementDirection = bubble.getMovementDirection();

            switch (movementDirection)
            {
                case MovementDirection.down:
                    if(canPlayerMove(movementDirection, bubble.getIteratorPosition(), SpriteType.BUBBLE))
                    {
                        bubble.moveDown(bubbleStepSize);
                    }
                    else
                    {
                        bubble.popBubble();
                    }                    
                    break;
                case MovementDirection.up:
                    if (canPlayerMove(movementDirection, bubble.getIteratorPosition(), SpriteType.BUBBLE))
                    {
                        bubble.moveUp(bubbleStepSize);
                    }
                    else
                    {
                        bubble.popBubble();
                    }
                    break;
                case MovementDirection.left:
                    if (canPlayerMove(movementDirection, bubble.getIteratorPosition(), SpriteType.BUBBLE))
                    {
                        bubble.moveLeft(bubbleStepSize);
                    }
                    else
                    {
                        bubble.popBubble();
                    }
                    break;
                case MovementDirection.right:
                    if (canPlayerMove(movementDirection, bubble.getIteratorPosition(), SpriteType.BUBBLE))
                    {
                        bubble.moveRight(bubbleStepSize);
                    }
                    else
                    {
                        bubble.popBubble();
                    }
                    break;
                default:
                    break;
            }
        }

        private void animateBubble(Bubble bubble, BubbleManager bubbleManager)
        {
            int count = bubble.getAnimationCount();

            if(bubble.isPopped())
            {
                if (count < Constant.LARGE_SPRITE_DIMENSION)
                {
                    count += bubbleStepSize;
                }
                else
                {
                    bubbleManager.removeBubble(bubble);
                }

                bubble.setAnimationCount(count);
            }
            else
            {
                if (count < Constant.LARGE_SPRITE_DIMENSION)
                {
                    count += bubbleStepSize;

                    if (count < 8)
                        bubble.setImageType(ImageType.BUBBLE_BIG);
                    else if (count > 8 && count < 16)
                        bubble.setImageType(ImageType.BUBBLE_SMALL);
                    else if (count > 16 && count < 32)
                        bubble.setImageType(ImageType.BUBBLE_BIG);
                    else if (count > 32 && count < 40)
                        bubble.setImageType(ImageType.BUBBLE_SMALL);
                    else if (count > 40)
                        bubble.setImageType(ImageType.BUBBLE_BIG);

                    moveBubble(bubble);
                }
                else
                {
                    moveBubbleIterator(bubble, bubble.getMovementDirection());
                    count = 0;
                }

                bubble.setAnimationCount(count);
            }
        }
    }
}
