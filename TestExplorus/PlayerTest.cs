using Explorus_K.Models;
using Explorus_K;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestExplorus
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void givenNewPlayerObject_whenMovingLeftAndDow_thenShouldChangePlayerPosition()
        {
            int initialPosX = 5;
            int initialPosY = 5;
            int stepSize = 1;
            int expectedPosX = initialPosX - stepSize;
            int expectedPosY = initialPosY + stepSize;
            ImageType imageType = ImageType.SLIMUS_DOWN_ANIMATION_1;

            Player testPlayer = new Player(initialPosX, initialPosY, imageType);
            testPlayer.moveLeft(stepSize);
            testPlayer.moveDown(stepSize);
            Image2D playerImage = testPlayer.refreshPlayer();

            Assert.AreEqual(expectedPosX, playerImage.X);
            Assert.AreEqual(expectedPosY, playerImage.Y);
            Assert.AreEqual(expectedPosX, testPlayer.getPosX());
            Assert.AreEqual(expectedPosY, testPlayer.getPosY());
            Assert.AreEqual(imageType, playerImage.getType());
        }


        [TestMethod]
        public void givenNewPlayerObject_whenMovingRightandUp_thenShouldChangePlayerPosition()
        {
            int initialPosX = 5;
            int initialPosY = 5;
            int stepSize = 1;
            int expectedPosX = initialPosX + stepSize;
            int expectedPosY = initialPosY - stepSize;
            ImageType imageType = ImageType.SLIMUS_DOWN_ANIMATION_1;

            Player testPlayer = new Player(initialPosX, initialPosY, imageType);
            testPlayer.moveRight(stepSize);
            testPlayer.moveUp(stepSize);
            Image2D playerImage = testPlayer.refreshPlayer();

            Assert.AreEqual(expectedPosX, playerImage.X);
            Assert.AreEqual(expectedPosY, playerImage.Y);
            Assert.AreEqual(expectedPosX, testPlayer.getPosX());
            Assert.AreEqual(expectedPosY, testPlayer.getPosY());
            Assert.AreEqual(imageType, playerImage.getType());
        }
    }
}
