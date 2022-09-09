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
		private bool paused = false;

        public bool Paused { get => paused; }
		public Actions CurrentAction { get => currentAction; }

        //If we have a action bind to that kay, we check if that action can be done
        public void actionHandler(Actions action, Iterator mapIterator)
		{
			Console.WriteLine(mapIterator.GetUp());
			if (action == Actions.pause || action == Actions.exit)
				currentAction = action;
			else if (action == Actions.move_left && mapIterator.isAbleToMoveLeft() && mapIterator.GetLeft() != "w" && mapIterator.GetLeft() != "p" && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_right && mapIterator.isAbleToMoveRight() && mapIterator.GetRight() != "w" && mapIterator.GetRight() != "p" && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_up && mapIterator.isAbleToMoveUp() && mapIterator.GetUp() != "w" && mapIterator.GetUp() != "p" && currentAction == Actions.none)
				currentAction = action;
			else if (action == Actions.move_down && mapIterator.isAbleToMoveDown() && mapIterator.GetDown() != "w" && mapIterator.GetDown() != "p" && currentAction == Actions.none)
				currentAction = action;
		}

        public void systemActionsManagement()
        {
            if (currentAction == Actions.pause)
            {
                paused = !paused;
                currentAction = Actions.none;
            }
            else if (currentAction == Actions.exit)
            {
                Application.Exit();
            }
        }

        //If the action can be done, we use a state machine to wait until the action is over
        public void characterActionsManagement(GameView view, Iterator mapIterator)
		{
			//Actions state machine
			if (currentAction == Actions.move_left)
			{
				if (count < view.largeSpriteDimension)
				{
					count += 2;
					view.getSlimusObject().moveLeft(2);

					if (count < 8)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_1);
					else if (count > 8 && count < 16)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_2);
					else if (count > 16 && count < 32)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_3);
					else if (count > 32 && count < 40)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_2);
					else if (count > 40)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_LEFT_ANIMATION_1);
				}
				else
				{
					count = 0;
					currentAction = Actions.none;
					mapIterator.MoveLeft();
				}
			}
			else if (currentAction == Actions.move_right)
			{
				if (count < view.largeSpriteDimension)
				{
					count += 2;
					view.getSlimusObject().moveRight(2);

					if (count < 8)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_1);
					else if (count > 8 && count < 16)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_2);
					else if (count > 16 && count < 32)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_3);
					else if (count > 32 && count < 40)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_2);
					else if (count > 40)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_RIGHT_ANIMATION_1);
				}
				else
				{
					count = 0;
					currentAction = Actions.none;
					mapIterator.MoveRight();
				}

			}
			else if (currentAction == Actions.move_up)
			{
				if (count < view.largeSpriteDimension)
				{
					count += 2;
					view.getSlimusObject().moveUp(2);

					if (count < 8)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_1);
					else if (count > 8 && count < 16)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_2);
					else if (count > 16 && count < 32)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_3);
					else if (count > 32 && count < 40)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_2);
					else if (count > 40)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_UP_ANIMATION_1);
				}
				else
				{
					count = 0;
					currentAction = Actions.none;
					mapIterator.MoveUp();
				}
			}
			else if (currentAction == Actions.move_down)
			{
				if (count < view.largeSpriteDimension)
				{
					count += 2;
					view.getSlimusObject().moveDown(2);

					if (count < 8)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_1);
					else if (count > 8 && count < 16)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_2);
					else if (count > 16 && count < 32)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_3);
					else if (count > 32 && count < 40)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_2);
					else if (count > 40)
						view.getSlimusObject().setImageType(ImageType.SLIMUS_DOWN_ANIMATION_1);
				}
				else
				{
					count = 0;
					currentAction = Actions.none;
					mapIterator.MoveDown();
				}
			}
		}
	}
}
