using Explorus_K.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace TestExplorus
{
    [TestClass]
    public class MapIteratorTest
    {
        private MapCollection testMap = new MapCollection(new string[,]{
            {"w", "w"},
            {".", "s"}
        });

        [TestMethod]
        public void givenWallToTheLeft_whenMovingLeft_thenShoudlNotBeAbleToMoveAndStayPut()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = iterator.Current();

            bool result = iterator.isAbleToMoveLeft();
            iterator.MoveLeft();

            Assert.IsFalse(result);
            Assert.AreEqual(expectedPosition, iterator.Current());
        }

        [TestMethod]
        public void givenWallToTheRight_whenMovingRight_thenShoudlNotBeAbleToMoveAndStayPut()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = iterator.Current();

            bool result = iterator.isAbleToMoveRight();
            iterator.MoveRight();

            Assert.IsFalse(result);
            Assert.AreEqual(expectedPosition, iterator.Current());
        }

        [TestMethod]
        public void givenWallBelow_whenMovingDown_thenShoudlNotBeAbleToMoveAndStayPut()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = iterator.Current();

            bool result = iterator.isAbleToMoveDown();
            iterator.MoveDown();

            Assert.IsFalse(result);
            Assert.AreEqual(expectedPosition, iterator.Current());
        }

        [TestMethod]
        public void givenEmptySpaceAbove_whenMovingDUp_thenShoudlBeAbleToMoveAndUpdatePosition()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = new Point(1, 0);

            bool result = iterator.isAbleToMoveUp();
            iterator.MoveUp();

            Assert.IsTrue(result);
            Assert.AreEqual(expectedPosition, iterator.Current());
        }


        private Iterator givenValidMapIterator()
        {
            return testMap.CreateIterator("s");
        }
    }
}
