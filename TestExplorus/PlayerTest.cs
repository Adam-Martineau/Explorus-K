using Explorus_K.Models;
using Explorus_K;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Explorus_K.Game;

namespace TestExplorus
{
    [TestClass]
    public class PlayerTest
    {
        private static string slimusName = "s";
        private static string toxicSlimeName = "t0";

        private int initialLiveCount = 6;

        private Iterator slimusIterator = new LabyrinthImage(new Labyrinth(), new BubbleManager()).Labyrinth.Map.CreateIterator(slimusName);
        private Iterator toxicSlimeIterator = new LabyrinthImage(new Labyrinth(), new BubbleManager()).Labyrinth.Map.CreateIterator(toxicSlimeName);




        [TestMethod]
        public void givenNewSlimusObject_whenMovingLeftAndDown_thenShouldChangePlayerPosition()
        {
            int initialPosX = 5;
            int initialPosY = 5;
            int stepSize = 1;
            int expectedPosX = initialPosX - stepSize;
            int expectedPosY = initialPosY + stepSize;
            ImageType imageType = ImageType.SLIMUS_DOWN_ANIMATION_1;

            Player testPlayer = new Slimus(initialPosX, initialPosY, imageType, 6, new Labyrinth().MapIterator);
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
        public void givenNewSlimusObject_whenMovingRightandUp_thenShouldChangePlayerPosition()
        {
            int initialPosX = 5;
            int initialPosY = 5;
            int stepSize = 1;
            int expectedPosX = initialPosX + stepSize;
            int expectedPosY = initialPosY - stepSize;
            ImageType imageType = ImageType.SLIMUS_DOWN_ANIMATION_1;

            Player testPlayer = new Slimus(initialPosX, initialPosY, imageType, 6, new Labyrinth().MapIterator);
            testPlayer.moveRight(stepSize);
            testPlayer.moveUp(stepSize);
            Image2D playerImage = testPlayer.refreshPlayer();

            Assert.AreEqual(expectedPosX, playerImage.X);
            Assert.AreEqual(expectedPosY, playerImage.Y);
            Assert.AreEqual(expectedPosX, testPlayer.getPosX());
            Assert.AreEqual(expectedPosY, testPlayer.getPosY());
            Assert.AreEqual(imageType, playerImage.getType());
        }

        [TestMethod]
        public void givenValidSlimusObject_whenDecreasingLives_thenLiveCountShouldDecrease()
        {
            Player slimus = givenValidSlimusObject();
            slimus.decreaseLife();

            Assert.AreEqual(slimus.getLifes(), initialLiveCount - 1);

        }

        [TestMethod]
        public void givenValidSlimusObject_whenChangingDirection_thenDirectionShouldChange()
        {
            Player slimus = givenValidSlimusObject();
            slimus.setMovementDirection(MovementDirection.up);

            Assert.AreEqual(slimus.getMovementDirection(), MovementDirection.up);

        }

        [TestMethod]
        public void givenValidSlimusObject_whenChangingAnimationCountAndName_thenAnimationCountAndLabyrinthNameShouldChange()
        {
            int expectedAnimationCount = 5;

            Player slimus = givenValidSlimusObject();
            slimus.setAnimationCount(expectedAnimationCount);
            slimus.setLabyrinthName(slimusName);

            Assert.AreEqual(slimus.getAnimationCount(), expectedAnimationCount);
            Assert.AreEqual(slimus.getLabyrinthName(), slimusName);

        }

        [TestMethod]
        public void givenValidSlimusObject_whenChangingInvicibleAndImage_thenTheseShouldChange()
        {
            ImageType expectedImageType = ImageType.SLIMUS_DOWN_ANIMATION_3;
            bool expectedInvicible = true;

            Slimus slimus = givenValidSlimusObject();
            slimus.setImageType(expectedImageType);
            slimus.setInvincible(expectedInvicible);
            Iterator slimusIt = slimus.getIterator();

            Assert.AreEqual(slimus.getImageType(), expectedImageType);
            Assert.AreEqual(slimus.getInvincible(), expectedInvicible);
            Assert.AreEqual(slimusIt, slimusIterator);
        }



        /// ========================= 
        /// TOXIC SLIME PART
        /// =========================



        [TestMethod]
        public void givenNewToxicSlimeObject_whenMovingLeftAndDown_thenShouldChangePlayerPosition()
        {
            int initialPosX = 5;
            int initialPosY = 5;
            int stepSize = 1;
            int expectedPosX = initialPosX - stepSize;
            int expectedPosY = initialPosY + stepSize;
            ImageType imageType = ImageType.SLIMUS_DOWN_ANIMATION_1;

            Player testPlayer = new ToxicSlime(initialPosX, initialPosY, imageType, 6, new Labyrinth().MapIterator);
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
        public void givenNewToxicSlimeObject_whenMovingRightandUp_thenShouldChangePlayerPosition()
        {
            int initialPosX = 5;
            int initialPosY = 5;
            int stepSize = 1;
            int expectedPosX = initialPosX + stepSize;
            int expectedPosY = initialPosY - stepSize;
            ImageType imageType = ImageType.SLIMUS_DOWN_ANIMATION_1;

            Player testPlayer = new ToxicSlime(initialPosX, initialPosY, imageType, 6, new Labyrinth().MapIterator);
            testPlayer.moveRight(stepSize);
            testPlayer.moveUp(stepSize);
            Image2D playerImage = testPlayer.refreshPlayer();

            Assert.AreEqual(expectedPosX, playerImage.X);
            Assert.AreEqual(expectedPosY, playerImage.Y);
            Assert.AreEqual(expectedPosX, testPlayer.getPosX());
            Assert.AreEqual(expectedPosY, testPlayer.getPosY());
            Assert.AreEqual(imageType, playerImage.getType());
        }

        [TestMethod]
        public void givenValidToxicSlimeObject_whenDecreasingLives_thenLiveCountShouldDecrease()
        {
            Player toxicSlime = givenValidToxicSlimeObject();
            toxicSlime.decreaseLife();

            Assert.AreEqual(toxicSlime.getLifes(), initialLiveCount - 1);

        }

        [TestMethod]
        public void givenValidToxicSlimeObject_whenChangingDirection_thenDirectionShouldChange()
        {
            Player toxicSlime = givenValidToxicSlimeObject();
            toxicSlime.setMovementDirection(MovementDirection.up);

            Assert.AreEqual(toxicSlime.getMovementDirection(), MovementDirection.up);

        }

        [TestMethod]
        public void givenValidToxicSlimeObject_whenChangingAnimationCountAndName_thenAnimationCountAndLabyrinthNameShouldChange()
        {
            int expectedAnimationCount = 5;

            Player toxicSlime = givenValidToxicSlimeObject();
            toxicSlime.setAnimationCount(expectedAnimationCount);
            toxicSlime.setLabyrinthName(toxicSlimeName);

            Assert.AreEqual(toxicSlime.getAnimationCount(), expectedAnimationCount);
            Assert.AreEqual(toxicSlime.getLabyrinthName(), toxicSlimeName);

        }

        [TestMethod]
        public void givenValidToxicSlimeObject_whenChangingInvicibleAndImage_thenTheseShouldChange()
        {
            ImageType expectedImageType = ImageType.SLIMUS_DOWN_ANIMATION_3;

            ToxicSlime toxicSlime = givenValidToxicSlimeObject();
            toxicSlime.setImageType(expectedImageType);

            Assert.AreEqual(toxicSlime.getImageType(), expectedImageType);
            Assert.AreEqual(toxicSlime.getIterator(), toxicSlimeIterator);
        }

        private Slimus givenValidSlimusObject()
        {
            return new Slimus(0, 0, ImageType.SLIMUS_DOWN_ANIMATION_1, initialLiveCount, slimusIterator);
        }

        private ToxicSlime givenValidToxicSlimeObject()
        {
            return new ToxicSlime(0, 0, ImageType.TOXIC_SLIME_DOWN_ANIMATION_1, initialLiveCount, toxicSlimeIterator);
        }
    }
}
