using Explorus_K.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace TestExplorus
{
    [TestClass]
    public class MapIteratorTest
    {
        String startingPoint = "s";
        String emptySpace = ".";
        String outOfBound = "";

        private MapCollection testMap = new MapCollection(new string[,]{
            {".", "."},
            {".", "s"} 
        });

        [TestMethod]
        public void givenEmptyToTheLeft_whenMovingLeft_thenShoudlBeAbleToMoveButStayPut()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = new Point(0,1);
            String expectedStringFromGet = emptySpace;

            bool result = iterator.isAbleToMoveLeft();
            String resultFromGet = (String)iterator.GetLeft();
            iterator.MoveLeft();

            Assert.IsTrue(result);
            Assert.AreEqual(expectedPosition, iterator.Current());
            Assert.AreEqual(expectedStringFromGet, resultFromGet);
        }

        [TestMethod]
        public void givenOutOfBoundToTheRight_whenMovingRight_thenShoudlNotBeAbleToMoveAndStayPut()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = iterator.Current();
            String expectedStringFromGet = outOfBound;

            bool result = iterator.isAbleToMoveRight();
            String resultFromGet = (String)iterator.GetRight();
            iterator.MoveRight();

            Assert.IsFalse(result);
            Assert.AreEqual(expectedPosition, iterator.Current());
            Assert.AreEqual(expectedStringFromGet, resultFromGet);
        }

        [TestMethod]
        public void givenOutOfBoundBelow_whenMovingDown_thenShoudlNotBeAbleToMoveAndStayPut()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = iterator.Current();
            String expectedStringFromGet = outOfBound;

            bool result = iterator.isAbleToMoveDown();
            String resultFromGet = (String)iterator.GetDown();
            iterator.MoveDown();

            Assert.IsFalse(result);
            Assert.AreEqual(expectedPosition, iterator.Current());
            Assert.AreEqual(expectedStringFromGet, resultFromGet);
        }

        [TestMethod]
        public void givenEmptySpaceAbove_whenMovingDUp_thenShoudlBeAbleToMoveAndUpdatePosition()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = new Point(1, 0);
            String expectedStringFromGet = emptySpace;

            bool result = iterator.isAbleToMoveUp();
            String resultFromGet = (String)iterator.GetUp();
            iterator.MoveUp();

            Assert.IsTrue(result);
            Assert.AreEqual(expectedPosition, iterator.Current());
            Assert.AreEqual(expectedStringFromGet, resultFromGet);
        }


        private Iterator givenValidMapIterator()
        {
            return testMap.CreateIterator(startingPoint);
        }
    }
}
