using Explorus_K.Game.Audio;
using Explorus_K.Game;
using Explorus_K.Models;
using Explorus_K.Threads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Explorus_K.Controllers;
using Explorus_K;
using System.Drawing;

namespace TestExplorus.TestsController
{
    [TestClass]
    public class CollisionStrategyTests
    {
        private GameEngine oController;
        private Thread oGameLoopThread;

        private object[,] testMap = new string[,]{
            { ".", ".", "."},
            { ".", "s" ,"."},
            { "t0", ".", "."}
        };

        [TestInitialize]
        public void initializeThread()
        {
            oController = new GameEngine(testMap);
            Assert.IsNotNull(oController);
            oController.IsWindowLess = true;

            oGameLoopThread = new Thread(oController.GameLoop);
            oGameLoopThread.Start();

            Assert.IsTrue(oGameLoopThread.IsAlive);
        }

        [TestCleanup]
        public void killAllThread()
        {
            if (oGameLoopThread.IsAlive)
            {
                oGameLoopThread.Abort();
                oGameLoopThread.Join();
            }
        }


        [TestMethod]
        public void executeStrategyGem()
        {
            clearSpriteAndPlayerList();
            makeSpritesCollide(SpriteType.SLIMUS, ImageType.SLIMUS_DOWN_ANIMATION_1, SpriteType.GEM, ImageType.GEM);

            Thread.Sleep(2000);

            Assert.AreEqual(oController.gameView.labyrinthImage.labyrinthImages.Count, 1);
            Assert.AreEqual(oController.gameView.labyrinthImage.GemBar.getCurrent(), 1);
        }

        [TestMethod]
        public void executeStrategyDoorWithNoKey()
        {
            clearSpriteAndPlayerList();
            makeSpritesCollide(SpriteType.SLIMUS, ImageType.SLIMUS_DOWN_ANIMATION_1, SpriteType.DOOR, ImageType.WALL);

            Thread.Sleep(2000);

            Assert.AreEqual(oController.gameView.labyrinthImage.labyrinthImages.Count, 2);
        }

        [TestMethod]
        public void executeStrategyDoorWithKey()
        {
            oController.gameView.labyrinthImage.labyrinthImages.Clear();
            oController.gameView.labyrinthImage.KeyState.RequestChangingState();
            makeSpritesCollide(SpriteType.SLIMUS, ImageType.SLIMUS_DOWN_ANIMATION_1, SpriteType.DOOR, ImageType.WALL);

            Thread.Sleep(2000);

            Assert.AreEqual(oController.gameView.labyrinthImage.labyrinthImages.Count, 1);
        }

        [TestMethod]
        public void executeStrategyToxicSlimeWithSlimus()
        {
            Slimus slimus = (Slimus)oController.gameView.getSlimusObject();
            int initialSlimusLives = slimus.getLifes();
            oController.gameView.labyrinthImage.labyrinthImages.Clear();
            makeSpritesCollide(SpriteType.SLIMUS, ImageType.SLIMUS_DOWN_ANIMATION_1, SpriteType.TOXIC_SLIME, ImageType.TOXIC_SLIME_UP_ANIMATION_1);

            Thread.Sleep(2000);

            Assert.AreNotEqual(slimus.getLifes(), initialSlimusLives);
            Assert.IsTrue(slimus.getInvincible());
        }

        [TestMethod]
        public void executeStrategyToxicSlimeWithBubble()
        {
            oController.gameView.labyrinthImage.labyrinthImages.Clear();
            Bubble bubble = new Bubble(500, 500, ImageType.BUBBLE_BIG, MovementDirection.down, new Point(1, 1));
            oController.getBubbleManager().addBubble(bubble);
            oController.gameView.labyrinthImage.labyrinthImages.Add(new Image2D(SpriteType.BUBBLE, ImageType.BUBBLE_BIG, 500, 500, bubble.id));
            oController.gameView.labyrinthImage.labyrinthImages.Add(new Image2D(SpriteType.TOXIC_SLIME, ImageType.TOXIC_SLIME_UP_ANIMATION_1, 495, 495, getToxicSlimeId()));

            Thread.Sleep(2000);

            Assert.IsTrue(assertToxicSlimeHasBeenHit());
        }

        private void makeSpritesCollide(SpriteType spriteOne, ImageType imageTypeOne, SpriteType spriteTwo, ImageType imageTypeTwo)
        {
            oController.gameView.labyrinthImage.labyrinthImages.Add(new Image2D(spriteOne, imageTypeOne, 500, 500));
            oController.gameView.labyrinthImage.labyrinthImages.Add(new Image2D(spriteTwo, imageTypeTwo, 495, 495));
        }

        private void clearSpriteAndPlayerList()
        {
            oController.gameView.labyrinthImage.labyrinthImages.Clear();
            oController.gameView.labyrinthImage.getPlayerList().Clear();
        }

        private bool assertToxicSlimeHasBeenHit()
        {
            foreach(Player player in oController.gameView.labyrinthImage.getPlayerList())
            {
                if(player.GetType() == typeof(ToxicSlime))
                {
                    if(player.getLifes() != 2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Guid getToxicSlimeId()
        {
            foreach (Player player in oController.gameView.labyrinthImage.getPlayerList())
            {
                if (player.GetType() == typeof(ToxicSlime))
                {
                    return player.GetGuid();
                }
            }

            return new Guid();
        }
    }
}
