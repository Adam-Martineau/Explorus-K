using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestExplorus
{
    /*
    [TestClass]
    public class ActionManagerTest
    {
        ActionManager actionManager = new ActionManager();

        [TestMethod]
        public void testAllActions()
        {
            actionHandlerTest(Actions.pause);
            actionManager.systemActionsManagement();
            Assert.AreEqual(actionManager.Paused, true);

            actionHandlerTest(Actions.move_up);
            actionHandlerTest(Actions.move_down);
            actionHandlerTest(Actions.move_right);
            actionHandlerTest(Actions.move_left);
        }

        void actionHandlerTest(Actions action)
        {
            Iterator mockIterator = createMockIterator();

            actionManager.actionHandler(action, mockIterator);
            Assert.AreEqual(actionManager.CurrentAction, action);
        }

        Iterator createMockIterator()
        {
            Mock<Iterator> mockIterator = new Mock<Iterator>();

            mockIterator.CallBase = true;

            mockIterator.Setup(i => i.isAbleToMoveDown()).Returns(true);
            mockIterator.Setup(i => i.isAbleToMoveUp()).Returns(true);
            mockIterator.Setup(i => i.isAbleToMoveRight()).Returns(true);
            mockIterator.Setup(i => i.isAbleToMoveLeft()).Returns(true);

            mockIterator.Setup(i => i.GetDown()).Returns(".");
            mockIterator.Setup(i => i.GetUp()).Returns(".");
            mockIterator.Setup(i => i.GetRight()).Returns(".");
            mockIterator.Setup(i => i.GetLeft()).Returns(".");

            mockIterator.Setup(i => i.MoveDown());
            mockIterator.Setup(i => i.MoveUp());
            mockIterator.Setup(i => i.MoveRight());
            mockIterator.Setup(i => i.MoveLeft());

            return mockIterator.Object;
        }
    }
    */
}
