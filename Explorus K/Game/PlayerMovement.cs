using Explorus_K.Controllers;
using Explorus_K.Game.Audio;
using Explorus_K.Game.Replay;
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
        private Player slimus;
        private Random rand = new Random();

        private int playerStepSize;
        private int bubbleStepSize;

        public PlayerMovement(Player slimus, GameDifficulty difficulty)
        {
            this.slimus = slimus;
            iterator = slimus.getIterator();
            playerStepSize = difficulty.getPlayerSpeed();
            bubbleStepSize = 4;
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

        public void moveAndAnimateBubbles(BubbleManager bubbleManager, AudioBabillard audioBabillard)
        {
            List<Bubble> bubbles = new List<Bubble>(bubbleManager.getBubbleList());

            foreach (Bubble bubble in bubbles)
            {
                animateBubble(bubble, bubbleManager, audioBabillard);
            }
        }

        public void moveAndAnimatePlayers(List<Player> players, Invoker commandInvoker, GameState gameState)
        {
            foreach(Player player in new List<Player>(players))
            {
                if(player.GetType() == typeof(ToxicSlime) && player.getMovementDirection() == MovementDirection.none && gameState != GameState.UNDO && gameState != GameState.REPLAY)
                {
                    setToxicSlimeMovementDirection(player, commandInvoker);
                }

                if(player.getMovementDirection() != MovementDirection.none)
                {
                    animatePlayer(player, player.getAnimationCount(), playerStepSize, gameState);
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
        }

        private void animatePlayer(Player player, int count, int stepSize, GameState gameState)
        {
            MovementDirection movementDirection = player.getMovementDirection();

            if (gameState != GameState.UNDO)
            {
                if (count < Constant.LARGE_SPRITE_DIMENSION && player.getMovementDirection() != MovementDirection.none)
                {
                    
                    movePlayer(player, movementDirection, stepSize);
                    count += stepSize;

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
                player.getIterator().Move(movementDirection);
                player.setMovementDirection(MovementDirection.none);
                count = 0;
            }

                player.setAnimationCount(count);
            }
            else
            {
                if (player.getAnimationCount() != 0)
                {
                    movePlayer(player, movementDirection, player.getAnimationCount());
                    player.setAnimationCount(0);
                }
                else
                {
                    movePlayer(player, movementDirection, Constant.LARGE_SPRITE_DIMENSION);
                }

                player.setImageType(player.getAnimationDictValue(movementDirection, ((int)AnimationEnum.BIG)));
                player.getIterator().Move(movementDirection);
                player.setMovementDirection(MovementDirection.none);               
            }
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

        private void setToxicSlimeMovementDirection(Player player, Invoker commandInvoker)
        {

            int randomInt = rand.Next(0, 4);

            MovementDirection movementDirection;

            switch (randomInt)
            {
                case 0:
                    movementDirection = MovementDirection.up;
                    if (canPlayerMove(movementDirection, player.getIterator().Current(),SpriteType.TOXIC_SLIME))
                    {
                        if (commandInvoker != null)
                        {
                            commandInvoker.executeCommand(new PlayerMovementCommand(player, movementDirection));
                        }
                    }
                    break;
                case 1:
                    movementDirection = MovementDirection.down;
                    if (canPlayerMove(movementDirection, player.getIterator().Current(), SpriteType.TOXIC_SLIME))
                    {
                        if (commandInvoker != null)
                        {
                            commandInvoker.executeCommand(new PlayerMovementCommand(player, movementDirection));
                        }
                    }
                    break;
                case 2:
                    movementDirection = MovementDirection.left;
                    if (canPlayerMove(movementDirection, player.getIterator().Current(), SpriteType.TOXIC_SLIME))
                    {
                        if (commandInvoker != null)
                        {
                            commandInvoker.executeCommand(new PlayerMovementCommand(player, movementDirection));
                        }
                    }
                    break;
                case 3:
                    movementDirection = MovementDirection.right;
                    if (canPlayerMove(movementDirection, player.getIterator().Current(), SpriteType.TOXIC_SLIME))
                    {
                        if (commandInvoker != null)
                        {
                            commandInvoker.executeCommand(new PlayerMovementCommand(player, movementDirection));
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void movementFollowSlimus(Player player)
        {

        }

        private void movementEmbushSlimus(Player player)
        {

        }

        private void movementRandom(Player player)
        {

        }

        private bool canPlayerSeeSlimus(Player player)
        {
            if (player.getPosX() != slimus.getPosX() || player.getPosY() != slimus.getPosY())
            {
                return false;
            }
            else if (player.getPosX() == slimus.getPosX())
            {
                if (isThereSomethingInBetweenVertically(new Point(player.getPosX(), player.getPosY()), new Point(slimus.getPosX(), slimus.getPosY())))
                    return false;
                else 
                    return true;
            }
            else if (player.getPosY() == slimus.getPosY())
            {
                if (isThereSomethingInBetweenHorizontally(new Point(player.getPosX(), player.getPosY()), new Point(slimus.getPosX(), slimus.getPosY())))
                    return false;
                else
                    return true;
            }
            
            return false;
        }

        private bool isThereSomethingInBetweenHorizontally(Point pos1, Point pos2)
        {
            int start = pos1.X > pos2.X ? pos2.X : pos1.X;
            int stop = pos1.X > pos2.X ? pos1.X : pos2.X;

            for (int i = start; i < stop; i++)
            {
                if ((string) slimus.getIterator().getMapEntryAt(i, pos1.Y) != ".")
                    return true; 
            }

            return false;
        }

        private bool isThereSomethingInBetweenVertically(Point pos1, Point pos2)
        {
            int start = pos1.Y > pos2.Y ? pos2.Y : pos1.Y;
            int stop = pos1.Y > pos2.Y ? pos1.Y : pos2.Y;

            for (int i = start; i < stop; i++)
            {
                if ((string) slimus.getIterator().getMapEntryAt(i, pos1.X) != ".")
                    return true;
            }

            return false;
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

        private void animateBubble(Bubble bubble, BubbleManager bubbleManager, AudioBabillard audioBabillard)
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
                    audioBabillard.AddMessage(AudioName.BUBBLE_HIT);
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

        public void setPlayerSpeed(int step)
        {
            playerStepSize = step;
        }
    }
}
