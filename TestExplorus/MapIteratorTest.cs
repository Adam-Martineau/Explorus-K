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

            iterator.Move(Explorus_K.Game.MovementDirection.left);

            Assert.AreEqual(expectedPosition, iterator.Current());
        }

        [TestMethod]
        public void givenOutOfBoundToTheRight_whenMovingRight_thenShoudlNotBeAbleToMoveAndStayPut()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = iterator.Current();
            String expectedStringFromGet = outOfBound;

            iterator.Move(Explorus_K.Game.MovementDirection.right); ;

            Assert.AreEqual(expectedPosition, iterator.Current());
        }

        [TestMethod]
        public void givenOutOfBoundBelow_whenMovingDown_thenShoudlNotBeAbleToMoveAndStayPut()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = iterator.Current();
            String expectedStringFromGet = outOfBound;

            iterator.Move(Explorus_K.Game.MovementDirection.down);

            Assert.AreEqual(expectedPosition, iterator.Current());
        }

        [TestMethod]
        public void givenEmptySpaceAbove_whenMovingDUp_thenShoudlBeAbleToMoveAndUpdatePosition()
        {
            Iterator iterator = givenValidMapIterator();
            Point expectedPosition = new Point(1, 0);
            String expectedStringFromGet = emptySpace;

            iterator.Move(Explorus_K.Game.MovementDirection.up);

            Assert.AreEqual(expectedPosition, iterator.Current());
        }


        private Iterator givenValidMapIterator()
        {
            return testMap.CreateIterator(startingPoint);
        }
    }
}
