using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExplorus
{
    [TestClass]
    internal class ActionManagerTest
    {
        [TestMethod]
        public void actiionHandlerTestPause()
        {
            ActionManager actionManager = new ActionManager();
            Iterator mockIterator = createMockIterator();

            actionManager.actionHandler(Actions.pause, mockIterator);
            Assert.AreEqual(actionManager.CurrentAction, Actions.pause);
        }

        public Iterator createMockIterator()
        {
            Mock<Iterator> mockIterator = new Mock<Iterator>();

            mockIterator.Setup(i => i.isAbleToMoveDown()).Returns(true);
            mockIterator.Setup(i => i.isAbleToMoveUp()).Returns(true);
            mockIterator.Setup(i => i.isAbleToMoveRight()).Returns(true);
            mockIterator.Setup(i => i.isAbleToMoveLeft()).Returns(true);

            mockIterator.Setup(i => i.GetDown()).Returns(".");
            mockIterator.Setup(i => i.GetUp()).Returns(".");
            mockIterator.Setup(i => i.GetRight()).Returns(".");
            mockIterator.Setup(i => i.GetDown()).Returns(".");

            return mockIterator.Object;
        }
    }
}
