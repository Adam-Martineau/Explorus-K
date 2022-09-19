using Explorus_K.Controllers;
using Explorus_K.Models;
using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K.Game
{
    class ActionManager
    {
		private Actions currentAction = Actions.none;
		private int count = 0;
		private bool isMovementInitialized = false;
        private MovementDirection oldDirection;
		
		public Actions CurrentAction { get => currentAction; }

		GameEngine gameEngine;
		PlayerMovement movePlayer;

		public ActionManager(GameEngine gameEngine, PlayerMovement playerMovement)
		{
			this.gameEngine = gameEngine;
			movePlayer = playerMovement;	
		}

        //If we have a action bind to that kay, we check if that action can be done
        public void actionHandler(Actions action, Iterator slimusIterator)
		{
			if (action == Actions.pause || action == Actions.exit)
				currentAction = action;
			else if (action == Actions.shoot && currentAction == Actions.none)
                currentAction = action;
            else if (action == Actions.move_left && movePlayer.canPlayerMove(MovementDirection.left, slimusIterator.Current()) && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_right && movePlayer.canPlayerMove(MovementDirection.right, slimusIterator.Current()) && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_up && movePlayer.canPlayerMove(MovementDirection.up, slimusIterator.Current()) && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_down && movePlayer.canPlayerMove(MovementDirection.down, slimusIterator.Current()) && currentAction == Actions.none)
				currentAction = action;
		}

        public void systemActionsManagement()
        {
            if (currentAction == Actions.pause)
            {
                gameEngine.Paused = !gameEngine.Paused;
                currentAction = Actions.none;
            }
            else if (currentAction == Actions.exit)
            {
                Application.Exit();
            }
        }

        //If the action can be done, we use a state machine to wait until the action is over
        public void characterActionsManagement(GameView view, BubbleManager bubbleManager)
		{
            //Actions state machine
            if (currentAction == Actions.shoot)
            {
                if(view.getBubbleBarObject().getCurrent() > 0)
                {
                    Slimus slimus = (Slimus)view.getSlimusObject();
                    Iterator tempSlimusIterator = slimus.getIterator();
                    Point posBubble = tempSlimusIterator.Current();
                    Bubble bubble;

                    switch(oldDirection)
                    {
                        case MovementDirection.up:
                            if(movePlayer.canPlayerMove(oldDirection, posBubble))
                            {
                                bubble = new Bubble(slimus.getPosX(), slimus.getPosY() - Constant.LARGE_SPRITE_DIMENSION, ImageType.BUBBLE_BIG, oldDirection, new Point(posBubble.X, posBubble.Y - 1));
                                bubbleManager.addBubble(bubble);
                            }
                            break;
                        case MovementDirection.down:
                            if (movePlayer.canPlayerMove(oldDirection, posBubble))
                            {
                                bubble = new Bubble(slimus.getPosX(), slimus.getPosY() + Constant.LARGE_SPRITE_DIMENSION, ImageType.BUBBLE_BIG, oldDirection, new Point(posBubble.X, posBubble.Y + 1));
                                bubbleManager.addBubble(bubble);
                            }
                            break;
                        case MovementDirection.left:
                            if (movePlayer.canPlayerMove(oldDirection, posBubble))
                            {
                                bubble = new Bubble(slimus.getPosX() - Constant.LARGE_SPRITE_DIMENSION, slimus.getPosY(), ImageType.BUBBLE_BIG, oldDirection, new Point(posBubble.X - 1, posBubble.Y));
                                bubbleManager.addBubble(bubble);
                            }
                            break;
                        case MovementDirection.right:
                            if (movePlayer.canPlayerMove(oldDirection, posBubble))
                            {
                                bubble = new Bubble(slimus.getPosX() + Constant.LARGE_SPRITE_DIMENSION, slimus.getPosY(), ImageType.BUBBLE_BIG, oldDirection, new Point(posBubble.X + 1, posBubble.Y));
                                bubbleManager.addBubble(bubble);
                            }
                            break;
                    }
                    currentAction = Actions.none;
                    view.getBubbleBarObject().Decrease();
                }
                    

            }
            else if (currentAction == Actions.move_left)
			{
				if(!isMovementInitialized)
				{
					isMovementInitialized = true;
					view.getSlimusObject().setMovementDirection(MovementDirection.left);
				}

				if (view.getSlimusObject().getMovementDirection() == MovementDirection.none)
				{
                    oldDirection = MovementDirection.left;
                    currentAction = Actions.none;
                    isMovementInitialized = false;
                }
			}
			else if (currentAction == Actions.move_right)
			{
                if (!isMovementInitialized)
                {
                    isMovementInitialized = true;
                    view.getSlimusObject().setMovementDirection(MovementDirection.right);
                }

                if (view.getSlimusObject().getMovementDirection() == MovementDirection.none)
                {
                    oldDirection = MovementDirection.right;
                    currentAction = Actions.none;
                    isMovementInitialized = false;
                }

			}
			else if (currentAction == Actions.move_up)
			{

                if (!isMovementInitialized)
                {
                    isMovementInitialized = true;
                    view.getSlimusObject().setMovementDirection(MovementDirection.up);
                }

                if (view.getSlimusObject().getMovementDirection() == MovementDirection.none)
                {
                    oldDirection = MovementDirection.up;
                    currentAction = Actions.none;
                    isMovementInitialized = false;
                }
			}
			else if (currentAction == Actions.move_down)
			{
                if (!isMovementInitialized)
                {
                    isMovementInitialized = true;
                    view.getSlimusObject().setMovementDirection(MovementDirection.down);
                }

                if (view.getSlimusObject().getMovementDirection() == MovementDirection.none)
                {
                    oldDirection = MovementDirection.down;
                    currentAction = Actions.none;
                    isMovementInitialized = false;
                }
            }
		}
	}
}
