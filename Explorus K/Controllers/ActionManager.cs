using Explorus_K.Controllers;
using Explorus_K.Models;
using Explorus_K.Views;
using System;
using System.Collections.Generic;
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
			else if (action == Actions.shoot)
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
        public void characterActionsManagement(GameView view)
		{
            //Actions state machine
            if (currentAction == Actions.shoot)
            {
                if (count < view.largeSpriteDimension)
                {
                    count += 2;
                }
                else
                {
                    count = 0;
                    currentAction = Actions.none;
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
                    currentAction = Actions.none;
                    isMovementInitialized = false;
                }
            }
		}
	}
}
